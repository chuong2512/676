using System;

public class GameEvent_15 : BaseGameEvent
{
	public override string GameEventCode => "Event_15";

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
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GamblingUI") as GamblingUI).ShowHealthGambling(this);
	}

	public override void InitEventByData(GameEventSaveData saveData)
	{
		GameEventSaveSeedData gameEventSaveSeedData;
		if ((gameEventSaveSeedData = saveData as GameEventSaveSeedData) != null)
		{
			RandomSeed = gameEventSaveSeedData.randomSeed;
		}
	}

	public override GameEventSaveData GetGameEventData()
	{
		return new GameEventSaveSeedData(eventBlockPosition.x, eventBlockPosition.y, GameEventCode, RandomSeed);
	}
}
