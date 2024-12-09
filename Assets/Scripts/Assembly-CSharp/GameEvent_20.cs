using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_20 : BaseGameEvent
{
	public override string GameEventCode => "Event_20";

	public int AccumulatePrice { get; set; }

	public int RandomSeed { get; set; }

	public override void StartEvent(int randomSeed, Action eventOverAction)
	{
		if (RandomSeed == 0)
		{
			RandomSeed = randomSeed;
		}
		UnityEngine.Random.InitState(RandomSeed);
		base.eventOverAction = eventOverAction;
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("遇到事件");
		OnStartEvent();
	}

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2)
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.AllEquipmentCount > 0,
			true
		}, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseEquipUI") as ChooseEquipUI).ShowAllEquipments("chooseequipmenttoMelt".LocalizeText(), -1, isMustEqualLimit: false, OnChooseEquip);
	}

	private void OnChooseEquip(List<string> equip)
	{
		if (equip != null)
		{
			for (int i = 0; i < equip.Count; i++)
			{
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equip[i]);
				Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment(equip[i]);
				AccumulatePrice += equipmentCardAttr.Price;
			}
		}
		if (AccumulatePrice >= 300)
		{
			(GameEventManager.Instace.GetEvent("Event_36") as GameEvent_36).OnStartEvent(AccumulatePrice, EndEvent);
		}
		else
		{
			(GameEventManager.Instace.GetEvent("Event_35") as GameEvent_35).OnStartEvent(Option0, Option1);
		}
		RandomSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		UnityEngine.Random.InitState(RandomSeed);
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
		GameSave.SaveGame();
	}

	private void EndEvent()
	{
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	public override void InitEventByData(GameEventSaveData saveData)
	{
		GameEventSave20Data gameEventSave20Data;
		if ((gameEventSave20Data = saveData as GameEventSave20Data) != null)
		{
			RandomSeed = gameEventSave20Data.randomSeed;
			AccumulatePrice = gameEventSave20Data.accumulatePrice;
		}
	}

	public override GameEventSaveData GetGameEventData()
	{
		return new GameEventSave20Data(eventBlockPosition.x, eventBlockPosition.y, GameEventCode, RandomSeed, AccumulatePrice);
	}
}
