using System;

[Serializable]
public class Character
{
	public string mapid;
	public string name;
	public string ownerid;
	public string spriteid;
	public Position position;
}

[Serializable]
public class Position {
	public int x;
	public int y;
}


