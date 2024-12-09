using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_5 : BaseGameEvent
{
	private List<string> tmpStatisfiedSkillList;

	private List<string> tmpStatisfiedCardList;

	public override string GameEventCode => "Event_5";

	protected override void OnStartEvent()
	{
		tmpStatisfiedSkillList = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		tmpStatisfiedCardList = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(5) { Option0, Option1, Option2, Option4, Option3 }, new List<bool>(5)
		{
			tmpStatisfiedSkillList.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5,
			true,
			tmpStatisfiedCardList.Count > 0 && Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 5,
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
		string skill = tmpStatisfiedSkillList[UnityEngine.Random.Range(0, tmpStatisfiedSkillList.Count)];
		BaseGameEvent.Event_ReduceHealth(5, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(5);
			BaseGameEvent.Event_GetSkill(skill);
		});
		EventOver();
	}

	private void Option1()
	{
		BaseGameEvent.Event_RecoveryHealth(5, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(5);
			EventOver();
		});
	}

	private void Option2()
	{
		string card = tmpStatisfiedCardList[UnityEngine.Random.Range(0, tmpStatisfiedCardList.Count)];
		BaseGameEvent.Event_ReduceHealth(5, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(5);
			BaseGameEvent.Event_GetSpecialUsualCard(card);
		});
		EventOver();
	}

	private void Option3()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void Option4()
	{
		BaseGameEvent.StartFollowEvent("Event_46", null);
		EventOver();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		tmpStatisfiedCardList.Clear();
		tmpStatisfiedSkillList.Clear();
	}
}
