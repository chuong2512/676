using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_0 : BaseGameEvent
{
	private List<string> tmpSatisfiedSkillList;

	private List<string> tmpSatisfiedCardList;

	public override string GameEventCode => "Event_0";

	protected override void OnStartEvent()
	{
		GameEventUI obj = SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI;
		tmpSatisfiedSkillList = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		tmpSatisfiedCardList = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		obj.LoadEvent(GameEventCode, new List<Action>(3) { Option0, Option1, Option2 }, new List<bool>(3)
		{
			tmpSatisfiedSkillList.Count > 0,
			tmpSatisfiedCardList.Count > 0,
			true
		}, new List<bool>(3) { true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetSkill(tmpSatisfiedSkillList[UnityEngine.Random.Range(0, tmpSatisfiedSkillList.Count)]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	private void Option1()
	{
		BaseGameEvent.Event_GetSpecialUsualCard(tmpSatisfiedCardList[UnityEngine.Random.Range(0, tmpSatisfiedCardList.Count)]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	private void Option2()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		tmpSatisfiedSkillList.Clear();
		tmpSatisfiedCardList.Clear();
	}
}
