using System;


public class User
{
	private string username;
	private string token;

	public User (string username, string token)
	{
		this.username = username;
		this.token = token;
	}

	public string getUsername() {
		return this.username;
	}

	public string getToken() {
		return this.token;
	}
}

