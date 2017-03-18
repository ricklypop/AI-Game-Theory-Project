using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TicTacToeState {
	public int x {get; set;}
	public int y {get; set;}

	public override bool Equals (object obj)
	{
		return ((TicTacToeState)obj).x == x && ((TicTacToeState)obj).y == y;
	}

	public override string ToString ()
	{
		return string.Format ("[TicTacToeState: x={0}, y={1}]", x, y);
	}
}
