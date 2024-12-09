using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_27 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_27(UsualCardAttr usualCardAttr)
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
		Buff_Bleeding buff_Bleeding = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(enemyBase, BuffType.Buff_Bleeding) as Buff_Bleeding;
		if (!buff_Bleeding.IsNull())
		{
			enemyBase.GetBuff(new Buff_Bleeding(enemyBase, buff_Bleeding.BuffRemainRound));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：使得当前目标的流血层数翻倍");
		}
		handler?.Invoke();
	}
}
