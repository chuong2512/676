using System;

public class SkillCard_S_A_0 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_0(SkillCardAttr skillCardAttr)
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
		Arrow[] array = new Arrow[RealArrowAmount()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new FireArrow();
		}
		((ArcherPlayerAttr)player.PlayerAttr).AddSpecialArrow(array);
		handler?.Invoke();
	}

	private int RealArrowAmount()
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponSkillCreateSpecialArrow, (BattleEffectData)null, out int IntData);
		return 3 + IntData;
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int num = 3;
		Buff_ShootStrengthen buff_ShootStrengthen;
		if ((buff_ShootStrengthen = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_ShootStrengthen) as Buff_ShootStrengthen) != null)
		{
			num += buff_ShootStrengthen.StrengthenAmount;
		}
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponArrowEffectTakeEffect, (BattleEffectData)new SimpleEffectData
		{
			intData = 2
		}, out int IntData);
		num += IntData;
		int num2 = RealArrowAmount();
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), num2, BaseCard.GetValueColor(3, num), num);
	}
}
