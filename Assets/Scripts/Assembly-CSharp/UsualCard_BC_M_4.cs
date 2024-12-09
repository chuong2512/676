using System;

public class UsualCard_BC_M_4 : UsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public UsualCard_BC_M_4(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：主手抽两张牌");
		}
		player.PlayerBattleInfo.TryDrawMainHandCards(2);
		handler?.Invoke();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return usualCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
