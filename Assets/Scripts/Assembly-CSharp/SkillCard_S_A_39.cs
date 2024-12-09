using System;
using UnityEngine;

public class SkillCard_S_A_39 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_39(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enemyBase, Action handler)
	{
		Buff_Bleeding buff_Bleeding = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(enemyBase, BuffType.Buff_Bleeding) as Buff_Bleeding;
		if (buff_Bleeding == null)
		{
			handler?.Invoke();
			return;
		}
		int buffRemainRound = buff_Bleeding.BuffRemainRound;
		enemyBase.GetBuff(new Buff_DeadPoison(enemyBase, buffRemainRound));
		enemyBase.RemoveBuff(buff_Bleeding);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
