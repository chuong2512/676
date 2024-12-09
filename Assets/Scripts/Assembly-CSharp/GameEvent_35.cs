using System;
using System.Collections.Generic;

public class GameEvent_35 : BaseGameEvent
{
	public override string GameEventCode => "Event_35";

	public override void StartEvent(int randomSeed, Action eventOverAction)
	{
	}

	protected override void OnStartEvent()
	{
	}

	public void OnStartEvent(Action Option0, Action Option1)
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AllEquipmentCount > 0,
			true
		}, new List<bool>(2) { true, true });
	}
}
