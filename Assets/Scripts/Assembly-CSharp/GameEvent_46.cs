using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_46 : BaseGameEvent
{
	public override string GameEventCode => "Event_46";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		string skill = list[UnityEngine.Random.Range(0, list.Count)];
		BaseGameEvent.Event_RecoveryHealth(3, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(3);
			BaseGameEvent.Event_GetSkill(skill);
			EventOver();
		});
	}
}
