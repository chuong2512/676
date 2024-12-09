using System;
using System.Collections.Generic;

public class GameEvent_22 : BaseGameEvent
{
	public override string GameEventCode => "Event_22";

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
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseEquipUI") as ChooseEquipUI).ShowAllEquipments("ChooseEquipmentToOfferUp".LocalizeText(), 1, isMustEqualLimit: true, OnComfirm);
	}

	private void OnComfirm(List<string> allEquips)
	{
		int price = DataManager.Instance.GetEquipmentCardAttr(allEquips[0]).Price;
		string empty = string.Empty;
		empty = ((price <= 10) ? "Event_37" : ((price > 75) ? "Event_39" : "Event_38"));
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveEquipment(allEquips[0]);
		BaseGameEvent.StartFollowEvent(empty, null);
		EventOver();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}
}
