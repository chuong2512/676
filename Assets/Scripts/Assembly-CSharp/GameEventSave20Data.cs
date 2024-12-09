using System;

[Serializable]
public class GameEventSave20Data : GameEventSaveData
{
	public int accumulatePrice;

	public int randomSeed;

	public GameEventSave20Data(int _x, int _y, string _eventCode, int _randomSeed, int accumulatePrice)
		: base(_x, _y, _eventCode)
	{
		randomSeed = _randomSeed;
		this.accumulatePrice = accumulatePrice;
	}
}
