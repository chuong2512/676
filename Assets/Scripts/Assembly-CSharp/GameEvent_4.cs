using System;
using System.Collections.Generic;

public class GameEvent_4 : BaseGameEvent
{
	public override string GameEventCode => "Event_4";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 1 }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		int value = ((Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 8) ? 8 : (Singleton<GameManager>.Instance.Player.PlayerAttr.Health - 1));
		if (value > 0)
		{
			BaseGameEvent.Event_ReduceHealth(value, isHideEventView: true, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(value);
			});
		}
		else
		{
			BaseGameEvent.HideGameEventView();
		}
		EventOver();
	}
}
