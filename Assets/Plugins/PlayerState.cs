using System;

public enum GameState {
	UNINITIALIZED,
	LOGGED_IN,
	CHARACTER_SELECT,
	WORLD_MAP,
	BATTLE
}

public class PlayerState
{
	public GameState gameState;

	public PlayerState() {
		this.gameState = GameState.UNINITIALIZED;
	}
}

