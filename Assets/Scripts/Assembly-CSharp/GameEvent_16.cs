using System;
using System.Collections.Generic;

public class GameEvent_16 : BaseGameEvent
{
	public override string GameEventCode => "Event_16";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(3) { Option0, Option1, Option2 }, new List<bool>(3) { true, true, true }, new List<bool>(3)
		{
			true,
			Singleton<GameManager>.Instance.Player.PlayerOccupation == PlayerOccupation.Knight,
			true
		});
	}

	private void Option0()
	{
		int maxHealthReduce = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth / 2;
		BaseGameEvent.Event_ReduceHealth(maxHealthReduce, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(-maxHealthReduce);
			BaseGameEvent.Event_GetEquipment("E_Armor_11");
		});
		EventOver();
	}

	private void Option1()
	{
		int healthReduce;
		if (Singleton<GameManager>.Instance.Player.PlayerAttr.Health > 10)
		{
			healthReduce = 10;
		}
		else
		{
			healthReduce = Singleton<GameManager>.Instance.Player.PlayerAttr.Health - 1;
		}
		if (healthReduce > 0)
		{
			BaseGameEvent.Event_ReduceHealth(healthReduce, isHideEventView: false, delegate
			{
				Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceHealth(healthReduce);
			});
		}
		BaseGameEvent.StartFollowEvent("Event_17", null);
		EventOver();
	}

	private void Option2()
	{
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 2, base.EventOver);
		BaseGameEvent.HideGameEventView();
	}
}
