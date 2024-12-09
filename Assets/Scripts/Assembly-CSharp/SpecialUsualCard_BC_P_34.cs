using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_34 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_34(UsualCardAttr usualCardAttr)
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
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, 20, isTrueDmg: true);
			if (enemyBase.IsDead)
			{
				player.PlayerAttr.RecoveryApAmount(3);
				if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent(base.CardName + "效果触发：因为击杀了怪物获得3点体力");
				}
			}
		}
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}目标造成1次{20}点的真实伤害");
		}
		handler?.Invoke();
	}
}
