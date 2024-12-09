using System;

[Serializable]
public class GameEventSave33Data : GameEventSaveData
{
	public int randomIndex;

	public GameEventSave33Data(int _x, int _y, string _eventCode, int _randomIndex)
		: base(_x, _y, _eventCode)
	{
		randomIndex = _randomIndex;
	}
}
