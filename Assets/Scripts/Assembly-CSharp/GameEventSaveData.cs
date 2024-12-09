using System;

[Serializable]
public class GameEventSaveData
{
	public int x;

	public int y;

	public string eventCode;

	public GameEventSaveData(int _x, int _y, string _eventCode)
	{
		x = _x;
		y = _y;
		eventCode = _eventCode;
	}
}
