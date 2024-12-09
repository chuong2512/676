using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_32 : BaseGameEvent
{
	private List<string> tmpStatisfiedEqiuipsList;

	public override string GameEventCode => "Event_32";

	protected override void OnStartEvent()
	{
		tmpStatisfiedEqiuipsList = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		int index = UnityEngine.Random.Range(0, tmpStatisfiedEqiuipsList.Count);
		BaseGameEvent.Event_GetEquipment(tmpStatisfiedEqiuipsList[index]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		tmpStatisfiedEqiuipsList.Clear();
	}
}
