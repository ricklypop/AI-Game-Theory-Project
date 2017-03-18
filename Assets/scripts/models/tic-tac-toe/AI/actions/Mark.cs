using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Mark : PlayerAction<TicTacToeState> {
	public int x {get; set;}
	public int y {get; set;}

	public override void act (GameState<TicTacToeState> game, Player<TicTacToeState> commiting)
	{
		(game as TicTacToeGameState).board[x, y] = Array.IndexOf(game.players, commiting) + 1;

		TicTacToeState newState = new TicTacToeState();
		newState.x = x;
		newState.y = y;
		commiting.state = newState;

		effective = false;
		completed = true;

	}
	
	public override void startAction (GameState<TicTacToeState> state)
	{
		started = true;
	}

	public override void updateEffect (GameState<TicTacToeState> state)
	{
		effective = true;
	}

	public override bool actionIsException (PlayerAction<TicTacToeState> action)
	{
		return false;
	}
	public override PlayerAction<TicTacToeState> modifyBasedOnException (PlayerAction<TicTacToeState> action)
	{
		return null;
	}
	
	public override PlayerAction<TicTacToeState> clone (){
		return ObjectCopier.Clone<Mark>(this);
	}

	public override string ToString ()
	{
		return string.Format ("[Mark: x={0}, y={1}]", x, y);
	}
}
