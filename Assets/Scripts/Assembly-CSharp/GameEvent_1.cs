using System;
using System.Collections.Generic;

public class GameEvent_1 : BaseGameEvent
{
	public override string GameEventCode => "Event_1";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomBlessingGift(), 0, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}
}
