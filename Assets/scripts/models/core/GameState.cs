using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState {
	public Player[] players {get; set;}
	public Action[] possibleActions {get; set;}
	public List<GameState> childGameStates {get; set;}
	public GameState fatherGameState {get; set;}
	public Rules rules {get; set;}
	public bool winState {get; set;}
	public int time {get; set;}
	
	public void simulateGameState(){

		foreach(Player player in players){

			if(!player.lastAction.actionStarted())
				player.lastAction.startAction(this);

			player.lastAction.updateEffect(this);

		}

		rules.applyUniversalRules(this);

		foreach(Player player in players){

			if(player.lastAction.actionEffective()){
				player.lastAction.act(this, player);
			}

		}

		if(rules.stateAbidesRules(this)){
			rules.adjustRules(this);
		}else{
			fatherGameState.childGameStates.Remove(this);
		}

	}

	public List<GameState> generateFuture(){

		if(!winState){

			List<GameState> future = new List<GameState>();

			HashSet<Player[]> futurePossibilities = futurePossibilitiesMatrixes(futureStateMatrix());

			foreach(Player[] possibilityMatrix in futurePossibilities){
				GameState futureGameState = new GameState();
				futureGameState.players = possibilityMatrix;

				foreach(Player player in possibilityMatrix){
					player.lastAction = player.lastAction.clone ();
				}

				futureGameState.time = time + 1;
				futureGameState.rules = rules;
				futureGameState.fatherGameState = this;
				future.Add(futureGameState);
			}

			foreach(GameState futureGameState in new List<GameState>(future)){

				if(rules.stateAbidesRules(futureGameState)){

					futureGameState.simulateGameState();

				}else{

					future.Remove(futureGameState);

				}

			}

			childGameStates = future;
			return future;

		}else{
			return new List<GameState>();
		}

	}

	Player[,] futureStateMatrix(){
		Player[,] futurePlayers = new Player[players.Length, possibleActions.Length];
		for(int x = 0; x < players.Length; x ++){
			
			for(int y = 0; y < possibleActions.Length; y ++){
				
				
				if(players[x].lastAction.actionCompleted()){
					
					Player futurePlayer = new Player();
					futurePlayer.copyState(players[x]);
					futurePlayer.lastAction = possibleActions[y];
					futurePlayers[x, y] = futurePlayer;
					
				}else if( players[x].lastAction.actionIsException(possibleActions[y])){
					
					Player futurePlayer = new Player();
					futurePlayer.copyState(players[x]);
					futurePlayer.lastAction = players[x].lastAction.modifyBasedOnException(possibleActions[y]);
					futurePlayers[x, y] = futurePlayer;
					
				}else{
					futurePlayers[x, y] = null;
				}
				
				
			}
			
			x ++;
		}
		return futurePlayers;
	}

	HashSet<Player[]> futurePossibilitiesMatrixes(Player[,] futureStateMatrix){
		int width = futureStateMatrix.GetLength(0);
		int height = futureStateMatrix.GetLength(1);
		int[] currentMatrix = new int[width];
		HashSet<Player[]> matrixes = new HashSet<Player[]>();

		for(int times = 0; times < Mathf.Pow(height, currentMatrix.Length); times++){
			Player[] current = new Player[currentMatrix.Length];

			for(int index = 0; index < currentMatrix.Length; index ++){
				current[index] = futureStateMatrix[index, currentMatrix[index]];
				if(current[index] == null)
					current[index] = players[index];
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

}
