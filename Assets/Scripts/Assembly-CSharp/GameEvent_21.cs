using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_21 : BaseGameEvent
{
	private List<string> allEquips;

	public override string GameEventCode => "Event_21";

	protected override void OnStartEvent()
	{
		allEquips = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(8);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			allEquips.Count > 0,
			true
		}, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetEquipment(allEquips[UnityEngine.Random.Range(0, allEquips.Count)]);
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 0, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		allEquips.Clear();
	}
}
