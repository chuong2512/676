using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_39 : BaseGameEvent
{
	public override string GameEventCode => "Event_39";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool>(1) { true });
	}

	private void Option0()
	{
		List<string> list = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		BaseGameEvent.Event_GetEquipment(list[UnityEngine.Random.Range(0, list.Count)]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}
}
