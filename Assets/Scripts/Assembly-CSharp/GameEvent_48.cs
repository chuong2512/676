using System;
using System.Collections.Generic;

public class GameEvent_48 : BaseGameEvent
{
	public override string GameEventCode => "Event_48";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetSkill("S_A_45");
		EventOver();
		BaseGameEvent.HideGameEventView();
	}
}
