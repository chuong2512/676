using System;
using System.Collections.Generic;

public class GameEvent_38 : BaseGameEvent
{
	public override string GameEventCode => "Event_38";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_RecoveryHealth(10, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(10);
			EventOver();
		});
	}
}
