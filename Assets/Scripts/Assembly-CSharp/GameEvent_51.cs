using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_51 : BaseGameEvent
{
	private float rate = 0.3f;

	public override string GameEventCode => "Event_51";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5,
			true
		}, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_ReduceHealth(5, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(5);
		});
		if (UnityEngine.Random.value <= rate)
		{
			BaseGameEvent.StartFollowEvent("Event_53", eventOverAction);
			return;
		}
		BaseGameEvent.StartFollowEvent("Event_51", eventOverAction);
		rate += 0.1f;
	}

	private void Option1()
	{
		BaseGameEvent.StartFollowEvent("Event_52", null);
	}

	public override void ClearEvent()
	{
		base.ClearEvent();
		rate = 0.3f;
	}
}
