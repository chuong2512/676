using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_27 : BaseGameEvent
{
	public override string GameEventCode => "Event_27";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		List<string> list = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
		if (list.Count > 0)
		{
			BaseGameEvent.Event_GetEquipment(list[UnityEngine.Random.Range(0, list.Count)]);
		}
		BaseGameEvent.HideGameEventView();
		EventOver();
	}
}
