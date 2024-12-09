using System;

public class GameEvent_14 : BaseGameEvent
{
	public override string GameEventCode => "Event_14";

	public int Coin { get; set; } = 5;


	public int RandomSeed { get; set; }

	public override void StartEvent(int randomSeed, Action eventOverAction)
	{
		if (RandomSeed == 0)
		{
			RandomSeed = randomSeed;
		}
		base.eventOverAction = eventOverAction;
		OnStartEvent();
	}

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GamblingUI") as GamblingUI).ShowCoinGambling(this);
	}

	public override void InitEventByData(GameEventSaveData saveData)
	{
		GameEventSave14Data gameEventSave14Data;
		if ((gameEventSave14Data = saveData as GameEventSave14Data) != null)
		{
			Coin = gameEventSave14Data.coin;
			RandomSeed = gameEventSave14Data.randomSeed;
		}
	}

	public override void ClearEvent()
	{
		base.ClearEvent();
		Coin = 5;
	}

	public override GameEventSaveData GetGameEventData()
	{
		return new GameEventSave14Data(eventBlockPosition.x, eventBlockPosition.y, GameEventCode, RandomSeed, Coin);
	}
}
