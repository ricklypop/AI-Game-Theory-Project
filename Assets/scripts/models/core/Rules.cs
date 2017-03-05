using UnityEngine;
using System.Collections;

public interface Rules {
	bool stateAbidesRules(GameState state);
	void applyUniversalRules(GameState state);
	void adjustRules(GameState state);
}
