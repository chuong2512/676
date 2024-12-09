using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_26 : BaseGameEvent
{
	public override string GameEventCode => "Event_26";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		if (list.Count > 0)
		{
			BaseGameEvent.Event_GetSkill(list[UnityEngine.Random.Range(0, list.Count)]);
		}
		BaseGameEvent.HideGameEventView();
		EventOver();
	}
}
