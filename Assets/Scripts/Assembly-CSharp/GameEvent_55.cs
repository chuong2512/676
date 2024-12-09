using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_55 : BaseGameEvent
{
	private List<string> allSatisfiedEquips;

	public override string GameEventCode => "Event_55";

	protected override void OnStartEvent()
	{
		allSatisfiedEquips = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option1, Option2, Option3 }, new List<bool>(4)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth > 20,
			allSatisfiedEquips.Count > 0,
			true,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option0()
	{
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		int maxHealthChange = 20;
		BaseGameEvent.Event_ReduceHealth(maxHealthChange, isHideEventView: false, delegate
		{
			playerAttr.VarifyMaxHealth(-maxHealthChange);
			playerAttr.RecoveryHealth(playerAttr.MaxHealth);
			VarifyRoomInfo();
		});
		BaseGameEvent.StartFollowEvent("Event_60", null);
	}

	private void Option1()
	{
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		int maxHealthChange = playerAttr.MaxHealth / 2;
		BaseGameEvent.Event_ReduceHealth(maxHealthChange, isHideEventView: false, delegate
		{
			playerAttr.VarifyMaxHealth(-maxHealthChange);
			BaseGameEvent.Event_GetEquipment(allSatisfiedEquips[UnityEngine.Random.Range(0, allSatisfiedEquips.Count)]);
			VarifyRoomInfo();
		});
		BaseGameEvent.StartFollowEvent("Event_60", null);
	}

	private void Option2()
	{
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		int maxHealthChange = playerAttr.MaxHealth / 2;
		BaseGameEvent.Event_ReduceHealth(maxHealthChange, isHideEventView: false, delegate
		{
			playerAttr.VarifyMaxHealth(-maxHealthChange);
		});
		GiftManager.Instace.GetSpecificGift(BaseGift.GiftName.EvilProtectBlessing, out var gift);
		BaseGameEvent.Event_GetGift(gift, 2, VarifyRoomInfo);
		BaseGameEvent.StartFollowEvent("Event_60", null);
	}

	private void Option3()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void VarifyRoomInfo()
	{
		BaseGameEvent.VarifyEventRoomInfo(eventBlockPosition, "Event_55", "Event_60");
	}
}
