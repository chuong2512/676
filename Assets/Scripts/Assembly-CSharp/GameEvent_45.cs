using System;
using System.Collections.Generic;

public class GameEvent_45 : BaseGameEvent
{
	public override string GameEventCode => "Event_45";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GainMoney(20, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(20);
			EventOver();
		});
	}
}
