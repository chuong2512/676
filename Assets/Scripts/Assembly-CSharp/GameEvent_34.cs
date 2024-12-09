using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_34 : BaseGameEvent
{
	private List<string> allCards;

	public override string GameEventCode => "Event_34";

	protected override void OnStartEvent()
	{
		allCards = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			allCards.Count > 0,
			true
		}, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetSpecialUsualCard(allCards[UnityEngine.Random.Range(0, allCards.Count)]);
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
		allCards.Clear();
	}
}
