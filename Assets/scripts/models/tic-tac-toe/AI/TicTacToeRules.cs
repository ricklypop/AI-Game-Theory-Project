using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class TicTacToeRules : Rules<TicTacToeState>
{


	public bool stateAbidesRules (GameState<TicTacToeState> state)
	{

		int count = 0;
		foreach (int val in ((TicTacToeGameState)state).board) {
			
			if (val > 0)
				count += 1;
			
		}

		bool abides = (count % 2 == 0 || state.players.Length > state.parentGameState.possibleActions.Length);

		List<TicTacToeState> list = new List<TicTacToeState>();
		if(state.players.Length > state.parentGameState.possibleActions.Length){
			TicTacToeState order = null;
			foreach(Player<TicTacToeState> player in state.players){
				TicTacToeState pos = new TicTacToeState();

				pos.x =((Mark)player.lastAction).x;
				pos.y =((Mark)player.lastAction).y;

				if(!list.Contains(pos)){
					list.Add(pos);
				}else if(order == null){
					order = pos;
				}else if(order != pos){
					abides = false;
					break;
				}

			}
		}
		
		return abides;

	}

	public void applyUniversalRules (GameState<TicTacToeState> state)
	{

	}

	public void adjustRules (GameState<TicTacToeState> state)
	{
		
		TicTacToeGameState tttState = (TicTacToeGameState)state;
		List<PlayerAction<TicTacToeState>> actions = new List<PlayerAction<TicTacToeState>> ();

		for (int x = 0; x < tttState.board.GetLength (0); x++) {
			for (int y = 0; y < tttState.board.GetLength (1); y++) {
				
				if (tttState.board [x, y] == 0) {

					Mark mark = new Mark();
					mark.x = x;
					mark.y = y;
					actions.Add(mark);

				}

			}
		}

		List<TicTacToeState> list = new List<TicTacToeState>();
		if(state.parentGameState != null 
		   && state.players.Length > state.parentGameState.possibleActions.Length){
			int playerNum = 1;
			foreach(Player<TicTacToeState> player in state.players){
				TicTacToeState pos = new TicTacToeState();
				
				pos.x =((Mark)player.lastAction).x;
				pos.y =((Mark)player.lastAction).y;
				
				if(!list.Contains(pos)){
					list.Add(pos);
					tttState.board[pos.x, pos.y] = playerNum;
				}
				playerNum ++;
			}
		}


		tttState.possibleActions = actions.ToArray ();
		TicTacToeGame.checkWin (tttState);

	}

}
