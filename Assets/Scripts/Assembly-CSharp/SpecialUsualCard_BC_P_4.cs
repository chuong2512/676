using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_4 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_4(UsualCardAttr usualCardAttr)
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
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
	}

	private void Effect(Player player, EnemyBase enTarget, Action handler)
	{
		enTarget.GetBuff(new Buff_DeadPoison(enTarget, 2));
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：对" + enTarget.EntityName + "施加2层致命毒药");
		}
		handler?.Invoke();
	}
}
