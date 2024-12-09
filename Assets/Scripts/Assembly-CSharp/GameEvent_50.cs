using System;
using System.Collections.Generic;

public class GameEvent_50 : BaseGameEvent
{
	public override string GameEventCode => "Event_50";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5,
			true
		}, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_ReduceHealth(5, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(5);
		});
		BaseGameEvent.StartFollowEvent("Event_51", eventOverAction);
	}

	private void Option1()
	{
		BaseGameEvent.StartFollowEvent("Event_52", eventOverAction);
	}
}
