using System;
using System.Collections.Generic;

public class GameEvent_52 : BaseGameEvent
{
	public override string GameEventCode => "Event_52";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}
}
