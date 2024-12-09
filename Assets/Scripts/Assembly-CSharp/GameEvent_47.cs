using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent_47 : BaseGameEvent
{
	public override string GameEventCode => "Event_47";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(2) { Option0, Option1 }, new List<bool>(2) { true, true }, new List<bool>(2) { true, true });
	}

	private void Option0()
	{
		EnemyHeapData specificEnemyHeap = DataManager.Instance.GetSpecificEnemyHeap("MHeap_102");
		Singleton<GameManager>.Instance.StartBattle(new BattleSystem.EventBattleHandler(specificEnemyHeap), Vector2.one * 0.5f);
		EventManager.RegisterEvent(EventEnum.E_BattleFinallyEnd, BattleFinallyEnd);
		BaseGameEvent.HideGameEventView();
	}

	private void Option1()
	{
		BaseGameEvent.HideGameEventView();
	}

	private void BattleFinallyEnd(EventData data)
	{
		if (!Singleton<GameManager>.Instance.Player.IsDead)
		{
			BaseGameEvent.StartFollowEvent("Event_48", null);
			EventOver();
		}
		EventManager.UnregisterEvent(EventEnum.E_BattleFinallyEnd, BattleFinallyEnd);
	}
}
