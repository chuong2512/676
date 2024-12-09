using System;

public class SkillCard_S_A_32 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_32(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		if (player.PlayerAttr.SpecialAttr == 0)
		{
			SkillCard.HandleEffect(base.EffectConfig, null, delegate
			{
				Effect(player, handler);
			});
		}
		else
		{
			handler?.Invoke();
		}
	}

	private void Effect(Player player, Action handler)
	{
		player.GetBuff(new Buff_Power(player, 1));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
