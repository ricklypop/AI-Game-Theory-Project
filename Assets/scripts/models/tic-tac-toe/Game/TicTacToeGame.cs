using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TicTacToeGame : MonoBehaviour
{
	public Button aiStart;
	public Button playerStart;
	public Button resetButton;
	public List<Button> buttons;

	private GameTreeAI<TicTacToeState> ai { get; set; }
	private int playerTurn = 0;
	private TicTacToeGameState game { get; set; }
	private bool gameStarted {get; set;}

	public bool aiFirst { get; set; }

	void Start ()
	{
		game = new TicTacToeGameState ();
		ai = new GameTreeAI<TicTacToeState> (
			Application.dataPath + "/TicTacToeTree.json", 0, new TicTacToeEvaluator());
	}

	void Update ()
	{
		if(!gameStarted)
			return;

		if (aiFirst && playerTurn == 0) {
			TicTacToeState state = ai.nextMove ();
			makeMove (state.x, state.y, 0);
		} else if (!aiFirst && playerTurn == 1) {
			TicTacToeState state = ai.nextMove ();
			makeMove (state.x, state.y, 1);
		}

		for(int index = 0; index < buttons.Count; index++){
			int xy = game.board[index % 3, index / 3];

			if(xy == 1)
				buttons[index].transform.GetChild(0).GetComponent<Text>().text = "X";
			else if(xy == 2)
				buttons[index].transform.GetChild(0).GetComponent<Text>().text = "O";
			else
				buttons[index].transform.GetChild(0).GetComponent<Text>().text = "";
		
			TicTacToeState s = new TicTacToeState();
			s.x = index % 3;
			s.y = index / 3;
			if(game.winState && checkWin(game).Contains(s))
				buttons[index].transform.GetChild(0).GetComponent<Text>().color = Color.green;
			else
				buttons[index].transform.GetChild(0).GetComponent<Text>().color = Color.black;
		}

	}

	public void makeMove (int x, int y, int player)
	{
		if (game.board [x, y] == 0 && player == playerTurn && gameStarted && !game.winState) {
			TicTacToeState state = new TicTacToeState ();
			state.x = x;
			state.y = y;
			game.board [x, y] = playerTurn + 1;
			ai.feedMove (state);
			playerTurn ++;

			if (playerTurn == game.players.Length)
				playerTurn = 0;

			checkWin(game);
		}

	}

	public void startGame(bool aiFirst){
		gameStarted = true;
		this.aiFirst = aiFirst;

		if(aiFirst)
			ai.playerID = 0;
		else
			ai.playerID = 1;

		aiStart.gameObject.SetActive(false);
		playerStart.gameObject.SetActive(false);
		resetButton.gameObject.SetActive(true);
	}

	public void resetGame(){
		gameStarted = false;

		game = new TicTacToeGameState ();
		playerTurn = 0;
		ai.reset();
		
		aiStart.gameObject.SetActive(true);
		playerStart.gameObject.SetActive(true);
		resetButton.gameObject.SetActive(false);
	}
	public static List<TicTacToeState> checkWin (TicTacToeGameState state)
	{
		List<TicTacToeState> states = new List<TicTacToeState>();
		for (int player = 0; player < state.players.Length; player++) {
			bool win = false;
			for (int x = 0; x < state.board.GetLength (0); x++) {
				
				for (int y = 0; y < state.board.GetLength (1); y++) {
					
					if (state.board [x, y] != player + 1) {
						win = false;
						states = new List<TicTacToeState>();
						break;
					}

					TicTacToeState s = new TicTacToeState();
					s.x = x;
					s.y = y;
					states.Add(s);
					win = true;
					
				}
				
				if (win) {
					state.winState = true;
					state.players[player].winner = true;
					return states;
				}
			}
			
			for (int y = 0; y < state.board.GetLength (1); y++) {
				
				for (int x = 0; x < state.board.GetLength (0); x++) {
					
					if (state.board [x, y] != player + 1) {
						win = false;
						states = new List<TicTacToeState>();
						break;
					}
					
					TicTacToeState s = new TicTacToeState();
					s.x = x;
					s.y = y;
					states.Add(s);
					win = true;
					
				}
				
				if (win) {
					state.winState = true;
					state.players[player].winner = true;
					return states;
				}
			}
			
			for (int x = 0; x < state.board.GetLength (0) && x < state.board.GetLength (1); x++) {
				
				if (state.board [x, x] != player + 1) {
					win = false;
					states = new List<TicTacToeState>();
					break;
				}
				
				TicTacToeState s = new TicTacToeState();
				s.x = x;
				s.y = x;
				states.Add(s);
				win = true;
				
			}
			
			if (win) {
				state.winState = true;
				state.players[player].winner = true;
				return states;
			}
			
			for (int x = state.board.GetLength (0) - 1; x >= 0; x--) {
				
				if (state.board [x, x] != player + 1) {
					win = false;
					states = new List<TicTacToeState>();
					break;
				}
				
				TicTacToeState s = new TicTacToeState();
				s.x = x;
				s.y = x;
				states.Add(s);
				win = true;
				
			}
			
			if (win) {
				state.winState = true;
				state.players[player].winner = true;
				return states;
			}
			
		}
		
		return new List<TicTacToeState>();
	}

}
