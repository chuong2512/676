using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_29 : BaseGameEvent
{
	private List<string> tmpStatisfiedEqiuipsList;

	private List<string> tmpSattisfiedCardList;

	public override string GameEventCode => "Event_29";

	protected override void OnStartEvent()
	{
		tmpStatisfiedEqiuipsList = AllRandomInventory.Instance.AllSatisfiedEquipsPlayerNotHave(2);
		tmpSattisfiedCardList = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(1);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(4) { Option0, Option1, Option2, Option3 }, new List<bool>(4)
		{
			true,
			tmpStatisfiedEqiuipsList.Count > 0,
			tmpSattisfiedCardList.Count > 0,
			true
		}, new List<bool>(4) { true, true, true, true });
	}

	private void Option0()
	{
		BaseGameEvent.Event_RecoveryHealth(5, 0, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(5);
			EventOver();
		});
	}

	private void Option1()
	{
		int index = UnityEngine.Random.Range(0, tmpStatisfiedEqiuipsList.Count);
		BaseGameEvent.Event_GetEquipment(tmpStatisfiedEqiuipsList[index]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	private void Option2()
	{
		BaseGameEvent.Event_GetSpecialUsualCard(tmpSattisfiedCardList[UnityEngine.Random.Range(0, tmpSattisfiedCardList.Count)]);
		BaseGameEvent.HideGameEventView();
		EventOver();
	}

	private void Option3()
	{
		BaseGameEvent.Event_GainMoney(20, 3, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(20);
			EventOver();
		});
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		tmpStatisfiedEqiuipsList.Clear();
	}
}
