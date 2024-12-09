using System;

public class SkillCard_S_A_3 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_3(SkillCardAttr skillCardAttr)
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
			array[i] = new BluntArrow();
		}
		((ArcherPlayerAttr)player.PlayerAttr).AddSpecialArrow(array);
		handler?.Invoke();
	}

	private int RealArrowAmount()
	{
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponSkillCreateSpecialArrow, (BattleEffectData)null, out int IntData);
		return 2 + IntData;
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int num = RealArrowAmount();
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(2, num), num);
	}
}
