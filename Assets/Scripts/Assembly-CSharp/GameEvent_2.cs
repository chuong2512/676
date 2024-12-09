using System;
using System.Collections.Generic;

public class GameEvent_2 : BaseGameEvent
{
	public override string GameEventCode => "Event_2";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(5) { Option0, Option1, Option2, Option4, Option3 }, new List<bool>(5)
		{
			true,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 1,
			true,
			true,
			true
		}, new List<bool>(5)
		{
			true,
			true,
			true,
			Singleton<GameManager>.Instance.Player.PlayerOccupation == PlayerOccupation.Archer,
			true
		});
	}

	private void Option0()
	{
		BaseGameEvent.Event_RecoveryHealth(10, 0, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(10);
			EventOver();
			BaseGameEvent.StartFollowEvent("Event_3", null);
		});
	}

	private void Option1()
	{
		BaseGameEvent.Event_RecoveryHealth(3, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(3);
			EventOver();
			BaseGameEvent.StartFollowEvent("Event_3", null);
		});
		BaseGameEvent.Event_LossMoney(1, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(1);
		});
	}

	private void Option2()
	{
		BaseGameEvent.Event_GainMoney(15, 2, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(15);
			EventOver();
			BaseGameEvent.StartFollowEvent("Event_4", null);
		});
	}

	private void Option3()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void Option4()
	{
		BaseGameEvent.StartFollowEvent("Event_45", null);
		EventOver();
	}
}
