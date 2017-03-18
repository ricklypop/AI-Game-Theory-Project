using UnityEngine;
using System.Collections;

public class FightingGame : GameState<float> {
	public Vector2 maxBounds {get; set;}

	public FightingGame() : base(2){

	}
}
