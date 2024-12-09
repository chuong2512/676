using System;

[Serializable]
public class GameEventSaveSeedData : GameEventSaveData
{
	public int randomSeed;

	public GameEventSaveSeedData(int _x, int _y, string _eventCode, int _randomSeed)
		: base(_x, _y, _eventCode)
	{
		randomSeed = _randomSeed;
	}
}
