using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScreen : MonoBehaviour, IWebsocketMessageReceiver {

	WebsocketManager wsManager;
	public void ReceiveMessage<T>(T message) {
		if (typeof(T) == typeof(RequestCharacterResponse)) {
			RequestCharacterResponse characters = message as RequestCharacterResponse;

			Debug.Log ("Character Select");
			for (int i = 0; i < characters.payload.characters.Length; i++) {
				Debug.Log (characters.payload.characters [i].name);
				Debug.Log (characters.payload.characters [i].ownerid);
				Debug.Log (characters.payload.characters [i].position.x);
				Debug.Log (characters.payload.characters [i].position.y);
				Debug.Log (characters.payload.characters [i].mapid);
				Debug.Log (characters.payload.characters [i].spriteid);
			}
		}
	}


	public void ReceiveManager(WebsocketManager wsManager) {
		this.wsManager = wsManager;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
