using System;
using UnityEngine;

public class SkillCard_S_A_12 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_12(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		EnemyBase target = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { target.EntityTransform }, delegate
		{
			Effect(player, target, handler);
		});
	}

	private void Effect(Player player, EnemyBase target, Action handler)
	{
		int num = Mathf.Min(target.EntityAttr.Armor, 10);
		target.TakeDirectoryArmorDmg(num);
		player.PlayerAttr.AddArmor(num);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
