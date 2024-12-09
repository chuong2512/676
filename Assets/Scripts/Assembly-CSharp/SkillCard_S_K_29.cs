using System;

public class SkillCard_S_K_29 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_29(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：获得防御buff进入防御状态，并且获得2层振奋buff");
		}
		player.GetBuff(new Buff_DefenceToAttack(player, 2));
		player.GetBuff(new Buff_Defence(player, int.MaxValue));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
