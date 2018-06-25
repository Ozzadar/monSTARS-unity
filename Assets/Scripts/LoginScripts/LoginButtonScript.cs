using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class LoginButtonScript : MonoBehaviour {

	#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string GetToken();
	#endif

	public InputField usernameInput;
	public InputField passwordInput;

	private Button button;

	// Use this for initialization
	void Start () {
		button = this.GetComponent<Button> ();
		button.onClick.AddListener (delegate {
			SubmitForm();
		});

		#if UNITY_WEBGL && !UNITY_EDITOR
		//Try and get login token
		GetToken();
		#endif

	}

	void SubmitForm() {


		#if !UNITY_WEBGL || UNITY_EDITOR
		string username = usernameInput.text;
		string password = passwordInput.text;

		GameObject go = GameObject.Find ("WebsocketManager");
		WebsocketManager wsMan = (WebsocketManager)go.GetComponent(typeof(WebsocketManager));
		Debug.Log (wsMan);
		wsMan.SendLoginRequest (username, password);
		#endif
	}

	// Update is called once per frame
	void Update () {
		
	}
}
