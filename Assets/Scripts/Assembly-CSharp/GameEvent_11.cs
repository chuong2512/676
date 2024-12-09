using System;
using System.Collections.Generic;

public class GameEvent_11 : BaseGameEvent
{
	public override string GameEventCode => "Event_11";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(3) { Option0, Option1, Option2 }, new List<bool>(3) { true, true, true }, new List<bool>(3)
		{
			true,
			Singleton<GameManager>.Instance.Player.PlayerOccupation == PlayerOccupation.Knight,
			true
		});
	}

	private void Option0()
	{
		BaseGameEvent.Event_RecoveryHealth(15, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.EntityAttr.RecoveryHealth(15);
			EventOver();
		});
	}

	private void Option1()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomBlessingGift(), 1, null);
		int health = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth - Singleton<GameManager>.Instance.Player.PlayerAttr.Health;
		if (health > 0)
		{
			BaseGameEvent.Event_RecoveryHealth(health, 1, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(health);
				EventOver();
				BaseGameEvent.StartFollowEvent("Event_12", null);
			});
		}
		else
		{
			EventOver();
			BaseGameEvent.StartFollowEvent("Event_12", null);
		}
	}

	private void Option2()
	{
		BaseGameEvent.HideGameEventView();
	}
}
