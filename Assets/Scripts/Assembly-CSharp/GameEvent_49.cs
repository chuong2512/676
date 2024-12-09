using System;
using System.Collections.Generic;

public class GameEvent_49 : BaseGameEvent
{
	public override string GameEventCode => "Event_49";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		if (Singleton<GameManager>.Instance.Player.IsPlayerHaveSkill("S_A_45"))
		{
			BaseGameEvent.StartFollowEvent("Event_53", base.EventOver);
		}
		else
		{
			BaseGameEvent.StartFollowEvent("Event_50", base.EventOver);
		}
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}
}
