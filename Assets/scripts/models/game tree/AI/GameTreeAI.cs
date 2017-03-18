using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Priority_Queue;
using System;

public class GameTreeAI<T> {
	public int playerID {get; set;}
	private GameTreeAIEvaluator<T> evaluator{get; set;}
	public GameState<T> totalTree{get; set;}
	public GameState<T> currentTree{get; set;}
	public GameState<T> currentGame {get; set;}
	public int currentPlayerID {get; set;}

	public GameTreeAI (string loadTreeFromPath, int playerID, GameTreeAIEvaluator<T> evaluator) {

		if(loadTreeFromPath != null && loadTreeFromPath != "")
			loadGameTree(loadTreeFromPath);
		this.playerID = playerID;
		this.evaluator = evaluator;

	}

	public GameTreeAI(string loadTreeFromPath, int playerID){
		if(loadTreeFromPath != null && loadTreeFromPath != "")
			loadGameTree(loadTreeFromPath);
		this.playerID = playerID;
		this.evaluator = new BaseAIEvaluation<T>();
	}

	public T nextMove(){

		List<GameState<T>> possible = new List<GameState<T>>();
		foreach(GameState<T> child in currentTree.childGameStates){
			if(child.possibilityOf(currentGame))
				possible.Add(child);
		}
		SimplePriorityQueue<T> queue = new SimplePriorityQueue<T>();
		Dictionary<string, float> dic = new Dictionary<string, float>();
		foreach(GameState<T> child in possible){
			if(queue.Contains(child.players[playerID].state)){
				dic[child.players[playerID].state.ToString()] = dic[child.players[playerID].state.ToString()] 
				                                                                  * evaluator.aiEvaluation(child, playerID);
				queue.UpdatePriority(child.players[playerID].state, 1 - dic[child.players[playerID].state.ToString()]);
			}else{
				float eval = evaluator.aiEvaluation(child, playerID);
				dic.Add(child.players[playerID].state.ToString(), eval);
				queue.Enqueue(child.players[playerID].state, 1 - eval);
			}
		}

		foreach(T t in queue){
			Debug.Log ((dic[t.ToString()]) + ":" + t);
		}

		return queue.Dequeue();
	}

	public void feedMove(T state){

		currentGame.players[currentPlayerID].state = state;
		currentPlayerID ++;

		if(currentPlayerID == currentGame.players.Length){

			currentTree = findMatch(currentGame);
			currentGame = new GameState<T>(totalTree.players.Length);
			currentPlayerID = 0;
		
		}
	}

	public void reset(){
		
		currentGame = new GameState<T>(totalTree.players.Length);
		currentTree = totalTree;
		currentPlayerID = 0;

	}

	GameState<T> findMatch(GameState<T> currentGame){

		foreach(GameState<T> child in currentTree.childGameStates){
			bool isMatch = true;
			for(int player = 0; player < currentGame.players.Length; player ++){
				if(currentGame.players[player].state == null || !currentGame.players[player].state.Equals(child.players[player].state)){
					isMatch = false;
					break;
				}
			}

			if(isMatch)
				return child;

		}

		return currentTree;

	}

	void loadGameTree(string loc){

		MultiThreading.startNewThread((int)((SystemInfo.systemMemorySize * ThreadingConstants.MB_IN_BYTES) / 8));

		MultiThreading.loadBalanceTask(() => {
			using (StreamReader r = new StreamReader(loc))
			{
				string json = r.ReadToEnd();
				totalTree = JsonConvert.DeserializeObject<GameState<T>>(json);
				currentTree = totalTree;
			}

			currentGame = new GameState<T>(totalTree.players.Length);
			Debug.Log("Loaded tree with " + allPossibleNum(totalTree) + " possibilities.");
			MultiThreading.stopAll();
		});

	}

	public static void generateMinimapHueristic(GameState<T> game){
		for(int playerID = 0; playerID < game.players.Length; playerID++)
			generateMinimap(game, playerID);
	}
	
	static float generateMinimap(GameState<T> game, int playerID){
		
		float max = 0;
		foreach(GameState<T> child in game.childGameStates)
			max += generateMinimap(child, playerID);
		
		if(game.childGameStates.Count != 0){
			game.heuristic[playerID] = max;
		}else if(game.players[playerID].winner){
			game.heuristic[playerID] = 1f;
		}else if(game.winState){
			game.heuristic[playerID] = -1f;
		}else{
			game.heuristic[playerID] = 0.0f;
		}
		
		return game.heuristic[playerID];
	}

	public static void generatePercentHeuristic(GameState<T> game){
		for(int playerID = 0; playerID < game.players.Length; playerID++)
			generatePercent(game, playerID);
	}

	static float generatePercent(GameState<T> game, int playerID){
		
		float max = 0;
		foreach(GameState<T> child in game.childGameStates)
			max += generatePercent(child, playerID);
		
		if(game.childGameStates.Count != 0){
			game.heuristic[playerID] = max / game.childGameStates.Count;
		}else if(game.players[playerID].winner){
			game.heuristic[playerID] = 1f;
		}else if(game.winState){
			game.heuristic[playerID] = 0f;
		}else{
			game.heuristic[playerID] = 0.5f;
		}
		
		return game.heuristic[playerID];
	}

	public void logCurrentOptions(){
		foreach(GameState<T> state in currentTree.childGameStates)
			Debug.Log(state);
	}

	public int allPossibleNum(GameState<T> state){
		int total = 0;
		foreach(GameState<T> child in state.childGameStates)
			total += allPossibleNum(child);

		return total + 1;
	}

}
