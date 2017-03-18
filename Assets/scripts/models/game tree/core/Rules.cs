using UnityEngine;
using System.Collections;

public interface Rules<T> {
	bool stateAbidesRules(GameState<T> state);
	void applyUniversalRules(GameState<T> state);
	void adjustRules(GameState<T> state);
}
