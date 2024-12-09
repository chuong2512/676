using System;
using System.Collections.Generic;

public class GameEvent_57 : BaseGameEvent
{
	public override string GameEventCode => "Event_57";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		int amount = Singleton<GameManager>.Instance.Player.PlayerAttr.Health / 2;
		BaseGameEvent.Event_ReduceHealth(amount, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(amount);
			BaseGameEvent.Event_GetSpecialUsualCard("BC_P_51");
			VarifyRoomInfo();
		});
		BaseGameEvent.StartFollowEvent("Event_62", null);
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void VarifyRoomInfo()
	{
		BaseGameEvent.VarifyEventRoomInfo(eventBlockPosition, "Event_57", "Event_62");
	}
}
