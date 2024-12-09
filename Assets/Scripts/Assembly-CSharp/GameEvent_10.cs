using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_10 : BaseGameEvent
{
	private List<string> allSatisfiedSpecialCards;

	private List<string> allSatisfiedSkills;

	private List<string> allSatisfiedOrnaments;

	public override string GameEventCode => "Event_10";

	protected override void OnStartEvent()
	{
		allSatisfiedSpecialCards = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(7);
		allSatisfiedSkills = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		allSatisfiedOrnaments = AllRandomInventory.Instance.AllSatisfiedRandomOrnament();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option2, Option3, Option1 }, new List<bool>(4)
		{
			allSatisfiedSpecialCards.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 30,
			allSatisfiedSkills.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 80,
			allSatisfiedOrnaments.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= 150,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option2()
	{
		string skill = allSatisfiedSkills[UnityEngine.Random.Range(0, allSatisfiedSkills.Count)];
		BaseGameEvent.Event_LossMoney(80, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(80);
			BaseGameEvent.Event_GetSkill(skill);
			EventOver();
		});
	}

	private void Option3()
	{
		string equips = allSatisfiedOrnaments[UnityEngine.Random.Range(0, allSatisfiedOrnaments.Count)];
		BaseGameEvent.Event_LossMoney(150, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(150);
			BaseGameEvent.Event_GetEquipment(equips);
			EventOver();
		});
	}

	private void Option0()
	{
		string card = allSatisfiedSpecialCards[UnityEngine.Random.Range(0, allSatisfiedSpecialCards.Count)];
		BaseGameEvent.Event_LossMoney(30, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(30);
			BaseGameEvent.Event_GetSpecialUsualCard(card);
			EventOver();
		});
	}

	private void Option1()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}
}
