using UnityEngine;
using System.Collections;

public interface Action {
	void act(GameState state, Player commiting);
	void startAction(GameState state);
	void updateEffect(GameState state);
	Action clone();
	bool actionStarted();
	bool actionEffective();
	bool actionCompleted();
	bool actionIsException(Action action);
	Action modifyBasedOnException(Action action);
}
