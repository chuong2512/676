using System;
using System.Collections.Generic;

public class GameEvent_37 : BaseGameEvent
{
	public override string GameEventCode => "Event_37";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 0, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}
}
