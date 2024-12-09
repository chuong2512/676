using System;
using System.Collections.Generic;

public class GameEvent_24 : BaseGameEvent
{
	public override string GameEventCode => "Event_24";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(5) { Option0, Option1, Option2, Option3, Option4 }, new List<bool>(5)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 1,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 5,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 10,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 20,
			true
		}, new List<bool>(5) { true, true, true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_LossMoney(1, 0, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(1);
			EventOver();
		});
		BaseGameEvent.StartFollowEvent("Event_28", null);
	}

	private void Option1()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomBlessingGift(), 1, null);
		BaseGameEvent.Event_LossMoney(5, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(5);
			EventOver();
		});
		BaseGameEvent.StartFollowEvent("Event_25", null);
	}

	private void Option2()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomBlessingGift(), 1, null);
		BaseGameEvent.Event_LossMoney(10, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(10);
			EventOver();
		});
		BaseGameEvent.StartFollowEvent("Event_26", null);
	}

	private void Option3()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomBlessingGift(), 1, null);
		BaseGameEvent.Event_LossMoney(20, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(20);
			EventOver();
		});
		BaseGameEvent.StartFollowEvent("Event_27", null);
	}

	private void Option4()
	{
		BaseGameEvent.StartFollowEvent("Event_28", null);
		EventOver();
	}
}
