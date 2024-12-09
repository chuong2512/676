using System;
using System.Collections.Generic;

public class GameEvent_59 : BaseGameEvent
{
	public override string GameEventCode => "Event_59";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { false, true }, new List<bool>(2) { false, true });
	}

	private void Option0()
	{
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}
}
