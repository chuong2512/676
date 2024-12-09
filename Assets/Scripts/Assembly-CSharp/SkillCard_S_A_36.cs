using System;

public class SkillCard_S_A_36 : SkillCard_Archer
{
	private int arrowAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_36(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler_ArrowComsume(5, isDrop: true);
		pointupHandler = new SelfUpHandler_ArrowComsume(5, isDrop: true);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		arrowAmount = player.PlayerAttr.SpecialAttr;
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, arrowAmount);
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.GetBuff(new Buff_Cover(player, arrowAmount));
		SkillCard_Archer.ComsumeSpecialAttr(player, null, arrowAmount, isDrop: false);
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
