using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_8 : BaseGameEvent
{
	private List<string> allSkills;

	private List<string> allSatisfiedEquips;

	private List<string> allCards;

	public override string GameEventCode => "Event_8";

	protected override void OnStartEvent()
	{
		allSatisfiedEquips = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
		allSkills = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		allCards = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option1, Option2, Option3 }, new List<bool>(4)
		{
			allSatisfiedEquips.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 10,
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 10 && allSkills.Count > 0,
			allCards.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 10,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option0()
	{
		string spoilEquip = allSatisfiedEquips[UnityEngine.Random.Range(0, allSatisfiedEquips.Count)];
		BaseGameEvent.Event_LossMoney(10, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(10);
			BaseGameEvent.Event_GetEquipment(spoilEquip);
			EventOver();
		});
	}

	private void Option1()
	{
		string skill = allSkills[UnityEngine.Random.Range(0, allSkills.Count)];
		BaseGameEvent.Event_LossMoney(10, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(10);
			BaseGameEvent.Event_GetSkill(skill);
			EventOver();
		});
	}

	private void Option2()
	{
		string card = allCards[UnityEngine.Random.Range(0, allCards.Count)];
		BaseGameEvent.Event_LossMoney(10, 2, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(10);
			BaseGameEvent.Event_GetSpecialUsualCard(card);
			EventOver();
		});
	}

	private void Option3()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		allSkills.Clear();
		allSatisfiedEquips.Clear();
		allCards.Clear();
	}
}
