using System;

public class SkillCard_S_A_28 : SkillCard_Archer
{
	private int coverAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_28(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		coverAmount = player.PlayerAttr.Armor;
		player.PlayerAttr.ReduceArmor(coverAmount);
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.GetBuff(new Buff_Cover(player, coverAmount));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
