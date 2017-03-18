using UnityEngine;
using System.Collections;

public class TicTacToeEvaluator : GameTreeAIEvaluator<TicTacToeState> {

	public float aiEvaluation (GameState<TicTacToeState> evalState, int playerID)
	{

		float eval = evalState.heuristic[playerID];

		foreach(GameState<TicTacToeState> child in evalState.childGameStates){
			if(!child.players[playerID].winner && child.winState){
				return 0;
			}
		}

		return eval;
	
	}
	
}
