using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

[Serializable]
public class TicTacToeGameState : GameState<TicTacToeState> {
	[JsonIgnore, NonSerialized]
	public int[,] board;

	public TicTacToeGameState() : base(GameConstants.TIC_TAC_TOE_PLAYERS){

		board = new int[GameConstants.TIC_TAC_TOE_SIZE, GameConstants.TIC_TAC_TOE_SIZE];
		possibleActions = new PlayerAction<TicTacToeState>[GameConstants.TIC_TAC_TOE_ACTIONS];
		Mark mark = new Mark ();
		mark.x = 0;
		mark.y = 0;
		possibleActions [0] = mark;
		mark = new Mark ();
		mark.x = 1;
		mark.y = 0;
		possibleActions [1] = mark;
		mark = new Mark ();
		mark.x = 2;
		mark.y = 0;
		possibleActions [2] = mark;
		mark = new Mark ();
		mark.x = 0;
		mark.y = 1;
		possibleActions [3] = mark;
		mark = new Mark ();
		mark.x = 1;
		mark.y = 1;
		possibleActions [4] = mark;
		mark = new Mark ();
		mark.x = 2;
		mark.y = 1;
		possibleActions [5] = mark;
		mark = new Mark ();
		mark.x = 0;
		mark.y = 2;
		possibleActions [6] = mark;
		mark = new Mark ();
		mark.x = 1;
		mark.y = 2;
		possibleActions [7] = mark;
		mark = new Mark ();
		mark.x = 2;
		mark.y = 2;
		possibleActions [8] = mark;

		rules = new TicTacToeRules ();
	}

	protected override GameState<TicTacToeState> cloneType(){
		TicTacToeGameState cloned = new TicTacToeGameState ();
		cloned.board =  ((int[,])board.Clone());
		return cloned;
	}

	public override string ToString ()
	{
		
		string toString = "[";
		for (int y = 0; y < board.GetLength (1); y++) {

			for (int x = 0; x < board.GetLength (0); x++) {
				toString += board[x, y] + ", ";
			}
			toString += "\n";

		}

		toString = toString.Substring (0, toString.Length - 3) + "]";

		return toString;
	}

}
