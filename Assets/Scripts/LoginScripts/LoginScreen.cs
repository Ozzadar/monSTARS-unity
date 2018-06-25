using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreen : MonoBehaviour, IWebsocketMessageReceiver {

	WebsocketManager wsManager;

	public void ReceiveMessage<T>(T message) {

	}

	public void ReceiveManager(WebsocketManager wsManager) {
		this.wsManager = wsManager;
	}

	// Use this for initialization
	void Start () {
		wsManager = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
