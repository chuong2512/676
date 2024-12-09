using System;
using System.Collections.Generic;

public class GameEvent_31 : BaseGameEvent
{
	public override string GameEventCode => "Event_31";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option1, Option2, Option3 }, new List<bool>(4)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 20,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 50,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 70,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_LossMoney(20, 0, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(20);
		});
		int health = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth / 2;
		BaseGameEvent.Event_RecoveryHealth(health, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(health);
			EventOver();
		});
	}

	private void Option1()
	{
		BaseGameEvent.Event_LossMoney(50, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(50);
		});
		int health = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth;
		BaseGameEvent.Event_RecoveryHealth(health, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(health);
			EventOver();
		});
	}

	private void Option2()
	{
		BaseGameEvent.Event_RecoveryHealth(20, 2, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(20);
			EventOver();
		});
		BaseGameEvent.Event_LossMoney(70, 2, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(70);
		});
	}

	private void Option3()
	{
		BaseGameEvent.HideGameEventView();
	}
}
