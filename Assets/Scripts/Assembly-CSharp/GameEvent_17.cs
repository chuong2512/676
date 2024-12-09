using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_17 : BaseGameEvent
{
	private List<string> tmpStatisfiedSkillList;

	public override string GameEventCode => "Event_17";

	protected override void OnStartEvent()
	{
		GameEventUI obj = SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI;
		tmpStatisfiedSkillList = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		obj.LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		if (tmpStatisfiedSkillList.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, tmpStatisfiedSkillList.Count);
			BaseGameEvent.Event_GetSkill(tmpStatisfiedSkillList[index]);
		}
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		tmpStatisfiedSkillList.Clear();
	}
}
