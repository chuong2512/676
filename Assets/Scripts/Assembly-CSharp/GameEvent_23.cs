using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_23 : BaseGameEvent
{
	private List<string> tmpHelmetList;

	private List<string> tmpGloveList;

	private List<string> tmpBreastplateList;

	private List<string> tmpShoesList;

	public override string GameEventCode => "Event_23";

	protected override void OnStartEvent()
	{
		tmpHelmetList = AllRandomInventory.Instance.AllStatisfiedConditionEquips(2, EquipmentType.Helmet);
		tmpGloveList = AllRandomInventory.Instance.AllStatisfiedConditionEquips(2, EquipmentType.Glove);
		tmpBreastplateList = AllRandomInventory.Instance.AllStatisfiedConditionEquips(2, EquipmentType.Breastplate);
		tmpShoesList = AllRandomInventory.Instance.AllStatisfiedConditionEquips(2, EquipmentType.Shoes);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option1, Option2, Option3 }, new List<bool>(4)
		{
			tmpHelmetList.Count > 0,
			tmpGloveList.Count > 0,
			tmpBreastplateList.Count > 0,
			tmpShoesList.Count > 0
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_GetEquipment(tmpHelmetList[UnityEngine.Random.Range(0, tmpHelmetList.Count)]);
		GiftManager.Instace.GetSpecificGift(BaseGift.GiftName.SilenceDamnation, out var gift);
		BaseGameEvent.Event_GetGift(gift, 0, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}

	private void Option1()
	{
		BaseGameEvent.Event_GetEquipment(tmpGloveList[UnityEngine.Random.Range(0, tmpGloveList.Count)]);
		GiftManager.Instace.GetSpecificGift(BaseGift.GiftName.OldDamnation, out var gift);
		BaseGameEvent.Event_GetGift(gift, 1, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}

	private void Option2()
	{
		BaseGameEvent.Event_GetEquipment(tmpBreastplateList[UnityEngine.Random.Range(0, tmpBreastplateList.Count)]);
		GiftManager.Instace.GetSpecificGift(BaseGift.GiftName.BreakDamnation, out var gift);
		BaseGameEvent.Event_GetGift(gift, 2, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}

	private void Option3()
	{
		BaseGameEvent.Event_GetEquipment(tmpShoesList[UnityEngine.Random.Range(0, tmpShoesList.Count)]);
		GiftManager.Instace.GetSpecificGift(BaseGift.GiftName.FragileDamnation, out var gift);
		BaseGameEvent.Event_GetGift(gift, 3, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}
}
