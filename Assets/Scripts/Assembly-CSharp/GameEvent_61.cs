using System;
using System.Collections.Generic;

public class GameEvent_61 : BaseGameEvent
{
	public override string GameEventCode => "Event_61";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		BaseGameEvent.HideGameEventView();
	}
}
