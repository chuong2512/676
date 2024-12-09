using System;
using System.Collections.Generic;

public class GameEvent_30 : BaseGameEvent
{
	private int MaxSubscribAmount = 4;

	private int accumulateSubscribAmount;

	public override string GameEventCode => "Event_30";

	protected override void OnStartEvent()
	{
		PlayerInventory playerInventory = Singleton<GameManager>.Instance.Player.PlayerInventory;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(5) { Option0, Option1, Option2, Option3, Option4 }, new List<bool>(5)
		{
			playerInventory.IsHaveEquipment("E_Head_9", EquipmentType.Helmet),
			playerInventory.IsHaveEquipment("E_Hands_9", EquipmentType.Glove),
			playerInventory.IsHaveEquipment("E_Armor_9", EquipmentType.Breastplate),
			playerInventory.IsHaveEquipment("E_Shoes_11", EquipmentType.Shoes),
			true
		}, new List<bool>(5) { true, true, true, true, true });
	}

	private void Option0()
	{
		if (accumulateSubscribAmount != MaxSubscribAmount)
		{
			accumulateSubscribAmount++;
			BaseGameEvent.Event_GainMoney(10, 0, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(10);
				GameSave.SaveGame();
			}, isBtnActive: true);
		}
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment("E_Head_9");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).SetBtnInteractive(0, interavtive: false);
	}

	private void Option1()
	{
		if (accumulateSubscribAmount != MaxSubscribAmount)
		{
			accumulateSubscribAmount++;
			BaseGameEvent.Event_GainMoney(10, 1, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(10);
				GameSave.SaveGame();
			}, isBtnActive: true);
		}
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment("E_Hands_9");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).SetBtnInteractive(1, interavtive: false);
	}

	private void Option2()
	{
		if (accumulateSubscribAmount != MaxSubscribAmount)
		{
			accumulateSubscribAmount++;
			BaseGameEvent.Event_GainMoney(10, 2, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(10);
				GameSave.SaveGame();
			}, isBtnActive: true);
		}
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment("E_Armor_9");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).SetBtnInteractive(2, interavtive: false);
	}

	private void Option3()
	{
		if (accumulateSubscribAmount != MaxSubscribAmount)
		{
			accumulateSubscribAmount++;
			BaseGameEvent.Event_GainMoney(10, 3, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(10);
				GameSave.SaveGame();
			}, isBtnActive: true);
		}
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment("E_Shoes_11");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).SetBtnInteractive(3, interavtive: false);
	}

	private void Option4()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI).SetBtnActive(isActive: false);
		if (accumulateSubscribAmount == MaxSubscribAmount)
		{
			BaseGameEvent.StartFollowEvent("Event_40", null);
			EventOver();
		}
		else
		{
			BaseGameEvent.HideGameEventView();
		}
	}

	public override void InitEventByData(GameEventSaveData saveData)
	{
		GameEventSave30Data gameEventSave30Data;
		if ((gameEventSave30Data = saveData as GameEventSave30Data) != null)
		{
			accumulateSubscribAmount = gameEventSave30Data.accumulateSubscribAmount;
		}
	}

	public override GameEventSaveData GetGameEventData()
	{
		return new GameEventSave30Data(eventBlockPosition.x, eventBlockPosition.y, GameEventCode, accumulateSubscribAmount);
	}
}
