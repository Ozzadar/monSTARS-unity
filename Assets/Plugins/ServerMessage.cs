using System;
using UnityEngine;
using System.Runtime.InteropServices;


/**
 * SWITCH ON TYPE TO DISCOVER TYPE
 */ 
[Serializable]
public class ServerMessage {
	public string type;
}


/**
 *	LOGIN MESSAGES 
 */
//IN
[Serializable]
public class LoginRequestMessage {
	public LoginRequestPayload payload;
}

[Serializable]
public class LoginRequestPayload {
	public string message;
	public string token;
	public string username;
}
	

//OUT
[Serializable]
public class LoginMessage {
	public string type;
	public LoginCredentials payload;
}
	
[Serializable]
public class LoginCredentials {
	public string username;
	public string password;
}

[Serializable]
public class LoginTokenMessage {
	public string type;
	public LoginToken payload;
}

[Serializable]
public class LoginToken {
	public string token;
}


//Character Request
[Serializable]
public class RequestCharacterResponse {
	public RequestCharacterResponsePayload payload;
}

[Serializable]
public class RequestCharacterResponsePayload {
	public string message;
	public Character[] characters;
}