using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_42 : SpecialUsualCard
{
	private static int RoundTime;

	private static bool isEverCal;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override int ApCost => specialUsualCardAttr.ApCost - RoundTime;

	public SpecialUsualCard_BC_P_42(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enemyBase, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, 12, isTrueDmg: true);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次真实的{12}点伤害");
		}
		handler?.Invoke();
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		ResetVariables();
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	private void OnBattleStart(EventData data)
	{
		ResetVariables();
		EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void OnPlayerStoringForce(EventData data)
	{
		ResetVariables();
	}

	private void OnBattleEnd(EventData data)
	{
		ResetVariables();
	}

	private void ResetVariables()
	{
		RoundTime = 0;
		isEverCal = false;
	}

	private void OnPlayerRound(EventData data)
	{
		isEverCal = false;
	}

	private void OnPlayerEndRound(EventData data)
	{
		if (!isEverCal)
		{
			PlayerBattleInfo playerBattleInfo = Singleton<GameManager>.Instance.Player.PlayerBattleInfo;
			if (playerBattleInfo.EnoughMainCards(base.CardCode, 1) || playerBattleInfo.EnoughSupCards(base.CardCode, 1))
			{
				isEverCal = true;
				RoundTime++;
				EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
				EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
			}
		}
	}
}
