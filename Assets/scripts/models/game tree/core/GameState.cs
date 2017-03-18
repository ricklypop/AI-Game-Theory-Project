using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Threading;

[Serializable]
public class GameState<T> {
	[JsonIgnore, NonSerialized]
	public GameState<T> parentGameState;
	[JsonIgnore, NonSerialized]
	public PlayerAction<T>[] possibleActions;
	[JsonIgnore, NonSerialized]
	public Rules<T> rules;
	[JsonIgnore, NonSerialized]
	public int time;


	public Player<T>[] players {get; set;}
	public List<GameState<T>> childGameStates {get; set;}
	public float[] heuristic {get; set;}
	public bool winState {get; set;}

	public GameState(int playerNum){
		childGameStates = new List<GameState<T>>();

		players = new Player<T>[playerNum];
		for(int index = 0; index < playerNum; index++)
			players [index] = new Player<T> ();

		heuristic = new float[playerNum];
		
	}

	public void simulateGameState(){

		foreach(Player<T> player in players){

			if(player.lastAction != null && !player.lastAction.started)
				player.lastAction.startAction(this);

			player.lastAction.updateEffect(this);

		}

		rules.applyUniversalRules(this);

		foreach(Player<T> player in players){

			if(player.lastAction != null && player.lastAction.effective){
				player.lastAction.act(this, player);
			}

		}

		if(rules.stateAbidesRules(this)){
			rules.adjustRules(this);
		}else{
			parentGameState.childGameStates.Remove(this);
		}

	}

	public List<GameState<T>> generateFuture(){

		if(!winState){

			List<GameState<T>> future = new List<GameState<T>>();

			HashSet<Player<T>[]> futurePossibilities = futurePossibilitiesMatrixes(futureStateMatrix());
			
			foreach(Player<T>[] possibilityMatrix in futurePossibilities){
				GameState<T> futureGameState = cloneType();
				futureGameState.players = possibilityMatrix;

				foreach(Player<T> player in possibilityMatrix){
					player.lastAction = player.lastAction.clone ();
				}

				futureGameState.time = time + 1;
				futureGameState.rules = rules;
				futureGameState.parentGameState = this;
				future.Add(futureGameState);
			}

			childGameStates = future;

			foreach(GameState<T> futureGameState in new List<GameState<T>>(future)){
				
				if(rules.stateAbidesRules(futureGameState)){

					futureGameState.simulateGameState();

				}else{
					
					future.Remove(futureGameState);
				}

			}
				
			return future;

		}else{
			return new List<GameState<T>>();
		}

	}

	Player<T>[,] futureStateMatrix(){
		Player<T>[,] futurePlayers = new Player<T>[players.Length, possibleActions.Length];
		for(int x = 0; x < players.Length; x ++){
			
			for(int y = 0; y < possibleActions.Length; y ++){
				
				
				if(players[x].lastAction == null || players[x].lastAction.completed){
					
					Player<T> futurePlayer = new Player<T>();
					futurePlayer.copyState(players[x]);
					futurePlayer.lastAction = possibleActions[y];
					futurePlayers[x, y] = futurePlayer;
					
				}else if( players[x].lastAction.actionIsException(possibleActions[y])){
					
					Player<T> futurePlayer = new Player<T>();
					futurePlayer.copyState(players[x]);
					futurePlayer.lastAction = players[x].lastAction.modifyBasedOnException(possibleActions[y]);
					futurePlayers[x, y] = futurePlayer;
					
				}else{
					futurePlayers[x, y] = null;
				}
				
				
			}
		}
		return futurePlayers;
	}

	HashSet<Player<T>[]> futurePossibilitiesMatrixes(Player<T>[,] futureStateMatrix){
		int width = futureStateMatrix.GetLength(0);
		int height = futureStateMatrix.GetLength(1);
		int[] currentMatrix = new int[width];
		HashSet<Player<T>[]> matrixes = new HashSet<Player<T>[]>();

		for(int times = 0; times < Mathf.Pow(height, currentMatrix.Length); times++){
			Player<T>[] current = new Player<T>[currentMatrix.Length];

			for(int index = 0; index < currentMatrix.Length; index ++){
				current[index] = futureStateMatrix[index, currentMatrix[index]];
				if (current [index] == null) {
					current [index] = players [index];
				}
			}

			matrixes.Add(current);

			for(int index = 0; index < currentMatrix.Length; index ++){

				if(currentMatrix[index] != height - 1){
					currentMatrix[index] += 1;
					break;
				}else{
					currentMatrix[index] = 0;
				}

			}
		}

		return matrixes;

	}

	public static void generateJsonGameTree(GameState<T> state, string outName, int playerID){

		state.generateFuture();

		for(int max = 0; max < SystemInfo.processorCount / 2; max++)
			MultiThreading.startNewThread((int)(ThreadingConstants.MB_IN_BYTES * 40));

		foreach(GameState<T> child in state.childGameStates){
			GameState<T> c = child;
			MultiThreading.loadBalanceTask(() => {
				generate(c);
				Debug.Log ("GENERATED:" + c);
			});
		}

		string loc = Application.persistentDataPath;

		int thread = 0;
		thread = MultiThreading.loadBalanceTask(() => {
		 	while(MultiThreading.threadsIdle(thread)){
				Thread.Sleep(1000);
		 	}

			Debug.Log ("AI STARTED");
			foreach(GameState<T> child in state.childGameStates){
				GameState<T> c = child;
				MultiThreading.loadBalanceTask(() => GameTreeAI<T>.generatePercentHeuristic(c));
			}
		
			thread = MultiThreading.loadBalanceTask(() => {
				while(MultiThreading.threadsIdle(thread)){
					Thread.Sleep(1000);
				}

				Debug.Log ("WRITING JSON");
				using (StreamWriter file = File.CreateText(loc + "/" +  outName + ".json"))
				{
				     JsonSerializer serializer = new JsonSerializer();
					 serializer.Serialize(file, state);
				}

				Debug.Log ("DONE");
				MultiThreading.stopAll();
			});

		});

	}

	static void generate(GameState<T> state){
		state.generateFuture ();

		foreach (GameState<T> future in state.childGameStates) {
			generate (future);
		}
	}

	public virtual bool possibilityOf(GameState<T> game){
		for(int index = 0; index < game.players.Length; index ++){
			if(game.players[index].state != null 
			   && !game.players[index].state.Equals(players[index].state)){
				return false;
			}
		}
		
		return true;
	}

	protected virtual GameState<T> cloneType(){
		return new GameState<T> (players.Length);
	}

	public override string ToString ()
	{
		string str = "";
		for (int i = 0; i < players.Length; i++){
			str = str + "|" + players[i].ToString();
		}
		return string.Format ("[GameState: players={0}, heuristic={1}, winState={2}]", str, heuristic, winState);
	}

}
