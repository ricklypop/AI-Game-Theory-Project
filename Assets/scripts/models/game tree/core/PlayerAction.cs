using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using System;

[Serializable]
public abstract class PlayerAction<T> {
	public bool started;
	public bool effective;
	public bool completed;

	public abstract void act(GameState<T> state, Player<T> commiting);
	public abstract void startAction(GameState<T> state);
	public abstract void updateEffect(GameState<T> state);
	public abstract PlayerAction<T> clone();
	public abstract bool actionIsException(PlayerAction<T> action);
	public abstract PlayerAction<T> modifyBasedOnException(PlayerAction<T> action);
}
