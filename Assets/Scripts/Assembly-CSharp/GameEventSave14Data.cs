using System;

[Serializable]
public class GameEventSave14Data : GameEventSaveData
{
	public int coin;

	public int randomSeed;

	public GameEventSave14Data(int _x, int _y, string _eventCode, int _randomSeed, int _coin)
		: base(_x, _y, _eventCode)
	{
		randomSeed = _randomSeed;
		coin = _coin;
	}
}
