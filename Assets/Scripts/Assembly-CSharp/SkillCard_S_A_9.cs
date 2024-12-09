using System;

public class SkillCard_S_A_9 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_9(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
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
		Buff_Cover buff_Cover;
		if ((buff_Cover = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Cover) as Buff_Cover) != null)
		{
			player.GetBuff(new Buff_Cover(player, buff_Cover.DmgReduceAmount));
		}
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
