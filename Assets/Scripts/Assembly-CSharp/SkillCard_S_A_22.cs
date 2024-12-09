using System;

public class SkillCard_S_A_22 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_22(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler_ArrowComsume(1, isDrop: false);
		pointupHandler = new SelfUpHandler_ArrowComsume(1, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 1);
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.GetBuff(new Buff_Agile(player, 1));
		player.GetBuff(new Buff_Cover(player, 3));
		SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
