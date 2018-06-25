using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserStateManager : MonoBehaviour, IWebsocketMessageReceiver {
	private WebsocketManager wsManager;
	private GameStateManager gsManager;

	private User currentUser;

	public void ReceiveMessage<T> (T message) {
		
		if (typeof(T) == typeof(LoginRequestMessage)) {

		}

		if (typeof(T) == typeof(LoginRequestMessage)) {
			LoginRequestMessage payload = message as LoginRequestMessage;

			Debug.Log (payload.payload.message);

			if (payload.payload.message == "LOGIN_SUCCESSFUL") {
				currentUser = new User (payload.payload.username, payload.payload.token);
				if (this.gsManager) {
					this.gsManager.ChangeState (GameState.CHARACTER_SELECT);
				}
			}
		}
	}

	public void ReceiveManager(WebsocketManager wsManager) {
		this.wsManager = wsManager;
	}

	public void RegisterGameStateManager(GameStateManager gsManager) {
		this.gsManager = gsManager;
	}

	public void ReceiveToken(string token) {
		if (token != null && token != "") {
			this.wsManager.SendLoginToken(token);
		}
	}

	public void Init() {
		this.currentUser = null;
		this.gsManager = null;
		this.wsManager = null;
	}
}
