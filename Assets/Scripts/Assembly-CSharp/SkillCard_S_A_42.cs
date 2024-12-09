using System;

public class SkillCard_S_A_42 : SkillCard_Archer
{
	private int arrowDrop;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_42(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler_ArrowComsume(5, isDrop: true);
		pointupHandler = new SelfUpHandler_ArrowComsume(5, isDrop: true);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		arrowDrop = player.PlayerAttr.SpecialAttr;
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, arrowDrop);
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		int ap = arrowDrop / 3;
		SkillCard_Archer.ComsumeSpecialAttr(player, null, arrowDrop, isDrop: true);
		player.GetBuff(new Buff_DefenceToAttack(player, ap));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
