using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStateButton : MonoBehaviour {

	public void ChangeState(int newState) {
		GameObject go = GameObject.Find ("GameStateManager");

		if (go != null) {
			GameStateManager stateManager = go.GetComponent<GameStateManager> ();

			stateManager.ChangeState ((GameState)newState);

		}
	}
}
