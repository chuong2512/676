using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_33 : BaseGameEvent
{
	private List<Action> AllRandomActions = new List<Action>(4);

	private int currentIndex = -1;

	public override string GameEventCode => "Event_33";

	public GameEvent_33()
	{
		AllRandomActions.Add(Option2);
		AllRandomActions.Add(Option3);
		AllRandomActions.Add(Option4);
		AllRandomActions.Add(Option5);
	}

	protected override void OnStartEvent()
	{
		int num = 0;
		num = ((currentIndex >= 0) ? currentIndex : (currentIndex = UnityEngine.Random.Range(0, 4)));
		GameEventData gameEventData = DataManager.Instance.GetGameEventData(GameEventCode);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2)
		{
			Option0,
			AllRandomActions[num],
			Option1
		}, new List<OptionData>(3)
		{
			gameEventData.OptionDatas[0],
			gameEventData.OptionDatas[num + 2],
			gameEventData.OptionDatas[1]
		}, new List<bool>(3) { true, true, true }, new List<bool>(3) { true, true, true });
	}

	private void Option0()
	{
		int health = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth - Singleton<GameManager>.Instance.Player.PlayerAttr.Health;
		if (health > 0)
		{
			BaseGameEvent.Event_RecoveryHealth(health, 0, isHideEventView: true, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(health);
				EventOver();
			});
		}
		else
		{
			BaseGameEvent.HideGameEventView();
			EventOver();
		}
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void Option2()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(4);
		BaseGameEvent.Event_GetSpecialUsualCard(list[UnityEngine.Random.Range(0, list.Count)]);
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	private void Option3()
	{
		List<string> list = AllRandomInventory.Instance.AllStatisfiedConditionSkills(Singleton<GameManager>.Instance.CurrentMapLevel, Singleton<GameManager>.Instance.Player.PlayerOccupation);
		BaseGameEvent.Event_GetSkill(list[UnityEngine.Random.Range(0, list.Count)]);
		EventOver();
		BaseGameEvent.HideGameEventView();
	}

	private void Option4()
	{
		BaseGameEvent.Event_RecoveryHealth(10, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(10);
			EventOver();
		});
	}

	private void Option5()
	{
		BaseGameEvent.Event_GainMoney(40, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(40);
			EventOver();
		});
	}

	public override GameEventSaveData GetGameEventData()
	{
		return new GameEventSave33Data(eventBlockPosition.x, eventBlockPosition.y, GameEventCode, currentIndex);
	}

	public override void InitEventByData(GameEventSaveData saveData)
	{
		base.InitEventByData(saveData);
		currentIndex = ((GameEventSave33Data)saveData).randomIndex;
	}

	protected override void OnEventOver()
	{
		base.OnEventOver();
		currentIndex = -1;
	}
}
