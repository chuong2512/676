using System;

public class SpecialUsualCard_BC_P_14 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_14(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
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
		UsualCard.HandleEffect(isMain ? base.EffectConfig : base.SupEffectConfig, null, delegate
		{
			Effect(player, isMain, handler);
		});
	}

	private void Effect(Player player, bool isMain, Action handler)
	{
		if (isMain)
		{
			player.PlayerBattleInfo.TryDrawMainHandCards(3);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(base.CardName + "效果触发：主手抽3张牌");
			}
		}
		else
		{
			player.PlayerBattleInfo.TryDrawSupHandCards(3);
			GameReportUI gameReportUI2 = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI2 != null)
			{
				gameReportUI2.AddGameReportContent(base.CardName + "效果触发：副手抽3张牌");
			}
		}
		handler?.Invoke();
	}
}
