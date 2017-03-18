using UnityEngine;
using System.Collections;

public class MarkOnPress : MonoBehaviour {

	public int x;
	public int y;
	public TicTacToeGame game;

	public void mark(){
		if(game.aiFirst)
			game.makeMove(x, y, 1);
		else
			game.makeMove(x, y, 0);
	}

}
