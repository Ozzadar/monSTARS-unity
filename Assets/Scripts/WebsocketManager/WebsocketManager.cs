using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class WebsocketManager : MonoBehaviour {


	private GameStateManager stateManager;
	private WebSocket w;

	private bool wsopen;
	ArrayList receivers;

	void Awake() {
		receivers = new ArrayList();
	}

	// Use this for initialization
	void Start () {

		GameObject go = GameObject.Find ("GameStateManager");
		stateManager = (GameStateManager)go.GetComponent (typeof(GameStateManager));

		StartCoroutine (OpenConnectionToServer ());
	}

	private IEnumerator OpenConnectionToServer() {
		w = new WebSocket(new Uri("ws://localhost:3000/ws"));

		yield return StartCoroutine(w.Connect());
		w.SendString ("{\"type\":\"CONNECTED\"}");
		stateManager.ChangeConnectionState (ConnectionState.CONNECTED);

		while (true)
		{ 
			
			string reply = w.RecvString();




			ServerMessage message = JsonUtility.FromJson<ServerMessage>(reply);
			if (reply != null)
			{
				if (message.type == "LoginRequest") {
					
					DispatchMessage(JsonUtility.FromJson<LoginRequestMessage> (reply));


				}
				if (message.type == "LoginSuccessful") {
					DispatchMessage (JsonUtility.FromJson<LoginRequestMessage> (reply));

				}

				if (message.type == "RequestCharacterResponse") {
					DispatchMessage (JsonUtility.FromJson<RequestCharacterResponse> (reply));
					RequestCharacterResponse characters = JsonUtility.FromJson<RequestCharacterResponse> (reply);

					for (int i = 0; i < characters.payload.characters.Length; i++) {
						Debug.Log (characters.payload.characters[i].name);
						Debug.Log (characters.payload.characters[i].ownerid);
						Debug.Log (characters.payload.characters[i].position.x);
						Debug.Log (characters.payload.characters[i].position.y);
						Debug.Log (characters.payload.characters[i].mapid);
						Debug.Log (characters.payload.characters[i].spriteid);
					}
				}

			}
			if (w.error != null)
			{
				Debug.LogError ("Error: "+w.error);
				wsopen = false;
				stateManager.ChangeConnectionState (ConnectionState.DISCONNECTED);
				break;
			}
			yield return 0;
		}
		w.Close ();
	}

	private void DispatchMessage<T>(T message) {
		if (receivers.Count != 0) {
			foreach (IWebsocketMessageReceiver receiver in receivers) {
				receiver.ReceiveMessage (message);
			}

		}
	}

	public bool isConnected() {
		return wsopen;
	}

	public void CloseConnection() {
		if (wsopen) {
			wsopen = false;
		}
	}

	public void OpenConnection() {
		if (!wsopen) {
			wsopen = true;
			StartCoroutine (OpenConnectionToServer ());
		}
	}

	public void RegisterReceiver(IWebsocketMessageReceiver newReceiver) {
		newReceiver.ReceiveManager (this);
		receivers.Add (newReceiver);
	}

	public void SendLoginRequest(string username, string password) {
		LoginMessage req = new LoginMessage();

		LoginCredentials creds = new LoginCredentials ();
		creds.username = username;
		creds.password = password;

		req.type = "LoginRequest";
		req.payload = creds;
		String json = JsonUtility.ToJson (req);
		w.SendString (json);

	}

	public void SendLoginToken(string token) {
		LoginTokenMessage req = new LoginTokenMessage();

		LoginToken creds = new LoginToken ();
		creds.token = token;

		req.type = "LoginTokenRequest";
		req.payload = creds;
		String json = JsonUtility.ToJson (req);
		w.SendString (json);
	}



	public void RequestCharacters() {
		LoginMessage req = new LoginMessage();
		req.type = "RequestCharacters";
		req.payload = null;

		String json = JsonUtility.ToJson(req);
		w.SendString(json);
	}
}
