using System;
using System.Collections.Generic;

public class GameEvent_13 : BaseGameEvent
{
	public override string GameEventCode => "Event_13";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI).ForceOpenAllRoomBlocks(isNeedAnim: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("瞭望塔");
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}
}
