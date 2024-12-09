using System;

[Serializable]
public class GameEventSave30Data : GameEventSaveData
{
	public int accumulateSubscribAmount;

	public GameEventSave30Data(int _x, int _y, string _eventCode, int _accumulateSubscribAmount)
		: base(_x, _y, _eventCode)
	{
		accumulateSubscribAmount = _accumulateSubscribAmount;
	}
}
