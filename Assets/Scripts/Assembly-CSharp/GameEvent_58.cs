using System;
using System.Collections.Generic;

public class GameEvent_58 : BaseGameEvent
{
	public override string GameEventCode => "Event_58";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(5) { Option0, Option1, Option2, Option3, Option4 }, new List<bool>(5) { true, true, true, true, true }, new List<bool>(5) { true, true, true, true, true });
	}

	private void Option0()
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.ClearAllEquipments();
		BaseGameEvent.Event_GetEquipment("E_Head_26");
		VarifyRoomInfo();
		BaseGameEvent.StartFollowEvent("Event_63", null);
	}

	private void Option1()
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.ClearAllEquipments();
		BaseGameEvent.Event_GetEquipment("E_Hands_25");
		VarifyRoomInfo();
		BaseGameEvent.StartFollowEvent("Event_63", null);
	}

	private void Option2()
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.ClearAllEquipments();
		BaseGameEvent.Event_GetEquipment("E_Armor_26");
		VarifyRoomInfo();
		BaseGameEvent.StartFollowEvent("Event_63", null);
	}

	private void Option3()
	{
		Singleton<GameManager>.Instance.Player.PlayerInventory.ClearAllEquipments();
		BaseGameEvent.Event_GetEquipment("E_Shoes_25");
		VarifyRoomInfo();
		BaseGameEvent.StartFollowEvent("Event_63", null);
	}

	private void Option4()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void VarifyRoomInfo()
	{
		BaseGameEvent.VarifyEventRoomInfo(eventBlockPosition, "Event_58", "Event_63");
	}
}
