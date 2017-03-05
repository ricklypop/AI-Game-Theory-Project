using UnityEngine;
using System.Collections;

public class Player {
	public Action lastAction {get; set;}
	public float state {get; set;}

	public void copyState(Player player){
		lastAction = player.lastAction;
		state = player.state;
	}
}
