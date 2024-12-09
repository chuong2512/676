using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_56 : BaseGameEvent
{
	public override string GameEventCode => "Event_56";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		int coinAmount = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney;
		BaseGameEvent.Event_LossMoney(coinAmount, 0, isHideEventView: false, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(coinAmount);
			if (coinAmount > 200)
			{
				List<string> list = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
				BaseGameEvent.Event_GetEquipment(list[UnityEngine.Random.Range(0, list.Count)]);
			}
			else if (coinAmount > 150)
			{
				List<string> list2 = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(8);
				BaseGameEvent.Event_GetEquipment(list2[UnityEngine.Random.Range(0, list2.Count)]);
			}
			else if (coinAmount > 100)
			{
				List<string> list3 = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(4);
				BaseGameEvent.Event_GetSpecialUsualCard(list3[UnityEngine.Random.Range(0, list3.Count)]);
			}
			VarifyRoomInfo();
		});
		BaseGameEvent.StartFollowEvent("Event_61", null);
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void VarifyRoomInfo()
	{
		BaseGameEvent.VarifyEventRoomInfo(eventBlockPosition, "Event_56", "Event_61");
	}
}
