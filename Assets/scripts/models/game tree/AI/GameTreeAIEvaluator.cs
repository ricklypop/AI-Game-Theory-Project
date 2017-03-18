using UnityEngine;
using System.Collections;

public interface GameTreeAIEvaluator<T> {
	float aiEvaluation(GameState<T> evalState, int playerID);
	
}

public class BaseAIEvaluation<T>: GameTreeAIEvaluator<T>{
	public float aiEvaluation(GameState<T> evalState, int playerID){
		return evalState.heuristic[playerID];
	}
}
