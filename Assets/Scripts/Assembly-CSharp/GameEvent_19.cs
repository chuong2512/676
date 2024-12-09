using System;
using System.Collections.Generic;

public class GameEvent_19 : BaseGameEvent
{
	public override string GameEventCode => "Event_19";

	protected override void OnStartEvent()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("GameEventUI") as GameEventUI).LoadEvent(GameEventCode, new List<Action>(3) { Option0, Option1, Option2 }, new List<bool>(3) { true, true, true }, new List<bool>(3) { true, true, true });
	}

	private void Option0()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("GameEventUI") as GameEventUI).SetBtnActive(isActive: false);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseSkillUI") as ChooseSkillUI).ShowSkillChoose("chooseskillyouwannaforget".LocalizeText(), Singleton<GameManager>.Instance.Player.GetPlayerAllSkills(), 1, isMustEqualLimit: true, OnConfirm);
	}

	private void OnConfirm(List<string> allSkills)
	{
		string text = allSkills[0];
		if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.CurrentSkillList.Contains(text))
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseSkill(text);
		}
		Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveSkill(text);
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
		BaseGameEvent.Event_GetGift(GiftManager.Instace.GetRandomDamnationGift(), 1, null);
		BaseGameEvent.Event_GainMoney(20, 1, isHideEventView: true, delegate
		{
			Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerEarnMoney(20);
			EventOver();
		});
	}

	private void Option2()
	{
		BaseGameEvent.HideGameEventView();
	}
}
