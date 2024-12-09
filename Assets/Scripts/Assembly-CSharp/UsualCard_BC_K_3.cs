using System;

public class UsualCard_BC_K_3 : UsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public UsualCard_BC_K_3(UsualCardAttr usualCardAttr)
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
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：玩家的信仰翻倍，获得的信仰值为：{player.PlayerAttr.SpecialAttr}");
		}
		player.PlayerAttr.AddSpecialAttr(player.PlayerAttr.SpecialAttr);
		handler?.Invoke();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.SpecialAttr);
	}
}
