using System;
using System.Collections.Generic;

public class GameEvent_18 : BaseGameEvent
{
	public override string GameEventCode => "Event_18";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(3) { Option0, Option1, Option2 }, new List<bool>(3) { true, true, true }, new List<bool>(3) { true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 0, null);
		BaseGameEvent.Event_RecoveryHealth(5, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(5);
			EventOver();
		});
	}

	private void Option1()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 1, null);
		BaseGameEvent.Event_GainMoney(40, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(40);
			EventOver();
		});
	}

	private void Option2()
	{
		Singleton<GameManager>.Instance.SwitchToNextRoom();
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 2, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}
}
