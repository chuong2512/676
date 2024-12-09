using System;

public class SkillCard_S_K_41 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_41(SkillCardAttr skillCardAttr)
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
		player.PlayerAttr.AddArmor(20);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.Armor);
	}
}
