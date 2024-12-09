using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_9 : BaseGameEvent
{
	private List<string> allSatisfiedEquips;

	private List<string> allSatisfiedOrnaments;

	private List<string> allSatisfiedSuit;

	public override string GameEventCode => "Event_9";

	protected override void OnStartEvent()
	{
		allSatisfiedEquips = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(15);
		allSatisfiedOrnaments = AllRandomInventory.Instance.AllSatisfiedRandomOrnament();
		allSatisfiedSuit = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option2, Option3, Option1 }, new List<bool>(4)
		{
			allSatisfiedEquips.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 50,
			allSatisfiedOrnaments.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 150,
			allSatisfiedSuit.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 300,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option2()
	{
		string spoilEquip = allSatisfiedOrnaments[UnityEngine.Random.Range(0, allSatisfiedOrnaments.Count)];
		BaseGameEvent.Event_LossMoney(150, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(150);
			BaseGameEvent.Event_GetEquipment(spoilEquip);
			EventOver();
		});
	}

	private void Option3()
	{
		string spoilEquip = allSatisfiedSuit[UnityEngine.Random.Range(0, allSatisfiedSuit.Count)];
		BaseGameEvent.Event_LossMoney(300, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(300);
			BaseGameEvent.Event_GetEquipment(spoilEquip);
			EventOver();
		});
	}

	private void Option0()
	{
		string spoilEquip = allSatisfiedEquips[UnityEngine.Random.Range(0, allSatisfiedEquips.Count)];
		BaseGameEvent.Event_LossMoney(50, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(50);
			BaseGameEvent.Event_GetEquipment(spoilEquip);
			EventOver();
		});
	}

	private void Option1()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		allSatisfiedEquips.Clear();
	}
}
