using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_20 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_20(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		if (!isMain)
		{
			return specialUsualCardAttr.DesKeyOnBattleSupHand.LocalizeText();
		}
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, isMain, enemyBase, handler);
		});
	}

	private void Effect(Player player, bool isMain, EnemyBase enemyBase, Action handler)
	{
		if (isMain)
		{
			player.PlayerAtkEnemy(enemyBase, 12, isTrueDmg: true);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(base.CardName + "效果触发：对" + enemyBase.EntityName + "造成1次真实的12点伤害");
			}
		}
		else
		{
			enemyBase.GetBuff(new Buff_Freeze(enemyBase, 1));
			GameReportUI gameReportUI2 = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI2 != null)
			{
				gameReportUI2.AddGameReportContent(base.CardName + "效果触发：对" + enemyBase.EntityName + "施加1回合的冻结buff");
			}
		}
		handler?.Invoke();
	}
}
