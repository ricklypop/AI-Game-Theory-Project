using UnityEngine;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class GenerateTicTacToe : MonoBehaviour {

	void Start () {

		TicTacToeGameState state = new TicTacToeGameState ();

		GameState<TicTacToeState>.generateJsonGameTree (state, "TicTacToeTree", 0);
	}

}
