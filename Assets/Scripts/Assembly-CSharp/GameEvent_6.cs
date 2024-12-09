using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_6 : BaseGameEvent
{
	public override string GameEventCode => "Event_6";

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
		List<string> allSatisfiedEquips = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
		BaseGameEvent.Event_GainMoney(10, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(10);
			if (allSatisfiedEquips.Count > 0)
			{
				BaseGameEvent.Event_GetEquipment(allSatisfiedEquips[UnityEngine.Random.Range(0, allSatisfiedEquips.Count)]);
			}
			EventOver();
		});
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 0, null);
	}

	private void Option1()
	{
		BaseGameEvent.Event_RecoveryHealth(10, 1, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(10);
			EventOver();
			BaseGameEvent.StartFollowEvent("Event_7", null);
		});
	}

	private void Option2()
	{
		BaseGameEvent.HideGameEventView();
		EventOver();
	}
}
