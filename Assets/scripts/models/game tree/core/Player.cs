using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;

[Serializable]
public class Player<T> {
	[JsonIgnore, NonSerialized]
	public PlayerAction<T> lastAction;

	public bool winner {get; set;}
	public T state {get; set;}

	public void copyState(Player<T> player){
		lastAction = player.lastAction;
		state = player.state;
	}

	public override string ToString ()
	{
		return string.Format ("[Player: winner={0}, state={1}]", winner, state);
	}
}
