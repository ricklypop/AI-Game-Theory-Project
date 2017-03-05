using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Mark : Action {
	public int x {get; set;}
	public int y {get; set;}
	public bool started {get; set;}
	public bool effective {get; set;}
	public bool completed {get; set;}

	public void act (GameState state, Player commiting)
	{
		((TicTacToeGameState)state).board[x, y] = Array.IndexOf(state.players, commiting) + 1;
		effective = false;
		completed = true;

	}
	public bool actionStarted ()
	{
		return started;
	}
	public bool actionEffective ()
	{
		return effective;
	}
	public bool actionCompleted ()
	{
		return completed;
	}
	
	public void startAction (GameState state)
	{
		started = true;
	}

	public void updateEffect (GameState state)
	{
		effective = true;
	}

	public bool actionIsException (Action action)
	{
		return false;
	}
	public Action modifyBasedOnException (Action action)
	{
		return null;
	}
	
	public Action clone (){
		return ObjectCopier.Clone<Mark>(this);
	}

}
