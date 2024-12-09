using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_40 : BaseGameEvent
{
	public override string GameEventCode => "Event_40";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(1) { Option0 }, new List<bool>(1) { true }, new List<bool> { true });
	}

	private void Option0()
	{
		List<string> list = new List<string>(4);
		for (int i = 0; i < 4; i++)
		{
			List<string> list2 = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
			string text = list2[UnityEngine.Random.Range(0, list2.Count)];
			AllRandomInventory.Instance.RemoveEquipment(text);
			list.Add(text);
		}
		BaseGameEvent.Event_GetEquipments(list);
		EventOver();
		BaseGameEvent.HideGameEventView();
	}
}
