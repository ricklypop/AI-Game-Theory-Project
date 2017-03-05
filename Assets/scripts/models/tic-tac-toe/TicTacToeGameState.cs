using UnityEngine;
using System.Collections;

public class TicTacToeGameState : GameState {
	public int[,] board {get; set;}

	public TicTacToeGameState(){
		board = new int[GameConstants.TIC_TAC_TOE_SIZE, GameConstants.TIC_TAC_TOE_SIZE];
	}

}
