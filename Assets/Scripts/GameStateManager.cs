using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
	MENU_MAIN,
	MENU_LOGIN,
	MENU_SETTINGS,
	MENU_HELP,
	CHARACTER_SELECT,
};

public enum ConnectionState {
	DISCONNECTED,
	CONNECTED,
}

public class GameStateManager : MonoBehaviour, IWebsocketMessageReceiver {


	/** IMPORTED PREFABS / GAMEOBJECTS */
	public GameObject WebsocketManagerPrefab;
	public GameObject MainMenuPrefab;
	public GameObject HelpMenuPrefab;
	public GameObject AudioMenuPrefab;
	public GameObject ConnectionStatusPrefab;
	public GameObject LoginMenuPrefab;
	public GameObject CharacterSelectPrefab;
	public GameObject UserManagerPrefab;

	public GameObject SpriteObject;

	private bool hasLoadedImage = false;
	private Texture2D image;
	/**PRIVATE VARIABLES */
	private static string connectionPanelName = "CONNECTION_PANEL";

	private ConnectionState connectionState;
	private GameState currentState;

	private List<GameObject> displayedGUI;
	private WebsocketManager wsManager;

	public void ReceiveMessage<T> (T message) {


	}

	public void ReceiveManager(WebsocketManager wsManager) {
		this.wsManager = wsManager;
	}

	public GameState GetCurrentGameState() {
		return currentState;
	}

	public void ChangeState(GameState newState) {
		LeaveState ();
		EnterState (newState);
	}

	private void LeaveState(){
		foreach (GameObject obj in displayedGUI) {
			Destroy (obj);
		}
		displayedGUI.Clear ();
	}

	private void EnterState(GameState newState){
		currentState = newState;

		switch (currentState) {
		case GameState.MENU_MAIN:
			{
				//Main menu prefab
				GameObject newObject = Instantiate (MainMenuPrefab, this.transform);
				newObject.transform.SetParent (this.transform);

				displayedGUI.Add (newObject);
				newObject = Instantiate (ConnectionStatusPrefab, this.transform);
				newObject.transform.SetParent (this.transform);
				newObject.name = connectionPanelName;
				displayedGUI.Add (newObject);

				SetStatus ();
				break;
			}
		case GameState.MENU_SETTINGS:
			{
				GameObject newObject = Instantiate (AudioMenuPrefab, this.transform);
				newObject.transform.SetParent (this.transform);
				displayedGUI.Add (newObject);
				break;
			}
		case GameState.MENU_HELP:
			{
				GameObject newObject = Instantiate (HelpMenuPrefab, this.transform);
				newObject.transform.SetParent (this.transform);
				displayedGUI.Add (newObject);
				break;
			}
		case GameState.MENU_LOGIN:
			{
				GameObject newObject = Instantiate (LoginMenuPrefab, this.transform);
				newObject.transform.SetParent (this.transform);
				displayedGUI.Add (newObject);
				break;
			}
		case GameState.CHARACTER_SELECT:
			{
				GameObject newObject = Instantiate (CharacterSelectPrefab, this.transform);
				newObject.transform.SetParent (this.transform);
				displayedGUI.Add (newObject);

				wsManager.RegisterReceiver (newObject.GetComponent<CharacterSelectScreen>());
				//Retrieve character list
				wsManager.RequestCharacters();

				StartCoroutine (LoadSpriteFromWeb(Constants.BASE_ASSET_URL + Constants.TEST_SPRITE)); 
				break;
			}
		default:
			{
				ChangeState (GameState.MENU_MAIN);
				break;
			}
		}

	}

	IEnumerator LoadSpriteFromWeb(string url) {
		Debug.Log ("Trying to load sprite");
		WWW www = new WWW(url);
		yield return www;

		image = www.texture;
		//image.alphaIsTransparency = true;

		SpriteObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(image, new Rect(0,0,32, 48), new Vector2(0.5f,0.5f), 6.4f);
		hasLoadedImage = true;
	}

	public ConnectionState GetCurrentConnectionState() {
		return connectionState;
	}

	public void ChangeConnectionState(ConnectionState state) {
		LeaveConnectionState ();
		EnterConnectionState (state);
	}

	private void LeaveConnectionState() {
		
	}

	private void EnterConnectionState(ConnectionState state) {
		connectionState = state;

		SetStatus ();
	}


	public void SetStatus() {
		//Find connection state panel
		GameObject connectionPanel = null;

		foreach (GameObject obj in displayedGUI) {
			if (obj.name.Equals(connectionPanelName)) {
				connectionPanel = obj;
				break;
			}
		}

		if (connectionPanel != null) {

			Text serverStatusLabel = null;
			Button reconnectButton = null;

			//get reconnect button and server status label
			foreach (Transform t in connectionPanel.transform) {
				if (t.gameObject.name.Equals ("ServerStatusLabel")) {
					serverStatusLabel = t.gameObject.GetComponent<Text>();
				} else if (t.gameObject.name.Equals ("ReconnectButton")) {
					t.gameObject.SetActive (true);
					reconnectButton = t.gameObject.GetComponent<Button>();
				}

				if (serverStatusLabel != null && reconnectButton != null) {
					break;
				}
			}

			if (serverStatusLabel != null && reconnectButton != null) {

				switch (connectionState) {
				case ConnectionState.CONNECTED:
					{
						serverStatusLabel.text = "Connected to server.";
						serverStatusLabel.color = Color.green;
						reconnectButton.gameObject.SetActive (false);
						break;
					}
				case ConnectionState.DISCONNECTED:
					{
						serverStatusLabel.text = "Disconnected.";
						serverStatusLabel.color = Color.red;
						reconnectButton.gameObject.SetActive(true);
						reconnectButton.onClick.AddListener(delegate {
							wsManager.OpenConnection();
						});
						break;
					}
				}
			}
		}
	}

	// Use this for initialization
	void Start () {

		GameObject ws = Instantiate (WebsocketManagerPrefab, this.transform);
		ws.name = "WebsocketManager";

		GameObject um = Instantiate (UserManagerPrefab, this.transform);
		um.name = "UserStateManager";

		UserStateManager usm = um.GetComponent<UserStateManager>();
		usm.RegisterGameStateManager(this);

		ws.GetComponent<WebsocketManager> ().RegisterReceiver(this);
		ws.GetComponent<WebsocketManager> ().RegisterReceiver(usm);

		displayedGUI = new List<GameObject> ();

		EnterState(GameState.MENU_MAIN);
	}

	private float timeSpent = 0.0f;
	private int frame = 0;
	private int row = 0;

	// Update is called once per frame
	void Update () {
		timeSpent += Time.deltaTime;

		if (timeSpent > 0.25f && hasLoadedImage) {
			frame++;
			if (frame > 3) {
				frame = 0;
				row++;
				if (row > 3) {
					row = 0;
				}
			}
			SpriteObject.GetComponent<SpriteRenderer> ().sprite = Sprite.Create(image, new Rect((frame * 32),(row*48),32, 48), new Vector2(0.5f,0.5f), 6.4f);
			timeSpent = 0;
		}
	}
}


