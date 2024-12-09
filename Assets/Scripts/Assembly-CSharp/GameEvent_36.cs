using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_36 : BaseGameEvent
{
	private Action eventOver;

	private int accumulatePrice;

	public override string GameEventCode => "Event_36";

	public override void StartEvent(int randomSeed, Action eventOverAction)
	{
	}

	protected override void OnStartEvent()
	{
	}

	public void OnStartEvent(int accumulatePrice, Action eventOver)
	{
		this.accumulatePrice = accumulatePrice;
		this.eventOver = eventOver;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { GetEquipments }, new List<bool>(1) { true }, new List<bool>(2) { true });
	}

	private void GetEquipments()
	{
		int num = 0;
		num = ((accumulatePrice < 400) ? 1 : ((accumulatePrice >= 500) ? 3 : 2));
		List<string> list = new List<string>(3);
		for (int i = 0; i < num; i++)
		{
			List<string> list2 = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation);
			if (list2.Count > 0)
			{
				string text = list2[UnityEngine.Random.Range(0, list2.Count)];
				AllRandomInventory.Instance.RemoveEquipment(text);
				list.Add(text);
			}
		}
		BaseGameEvent.Event_GetEquipments(list);
		eventOver?.Invoke();
	}
}
