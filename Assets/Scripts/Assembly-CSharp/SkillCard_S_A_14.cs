using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_A_14 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_14(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler_ArrowComsume(1, isDrop: false);
		pointupHandler = new AllEnemyUpHandler_ArrowComsume(1, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 1);
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[allEnemies.Count];
		for (int i = 0; i < allEnemies.Count; i++)
		{
			array[i] = allEnemies[i].EntityTransform;
		}
		realDmg = RealDmg(player, out atkDes);
		SkillCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, allEnemies, handler);
		});
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, Action handler)
	{
		List<EntityBase> list = new List<EntityBase>(allEnemies.Count);
		for (int i = 0; i < allEnemies.Count; i++)
		{
			EnemyBase enemyBase = allEnemies[i];
			if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
			{
				buff.TakeEffect(enemyBase);
				continue;
			}
			list.Add(enemyBase);
			player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
			Buff_DeadPoison buff_DeadPoison;
			if ((buff_DeadPoison = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(enemyBase, BuffType.Buff_DeadPoison) as Buff_DeadPoison) != null)
			{
				int poisonAmount = buff_DeadPoison.PoisonAmount;
				enemyBase.TakeDamage(poisonAmount, null, isAbsDmg: true);
			}
		}
		SkillCard_Archer.ComsumeSpecialAttr(player, list.ToArray(), 1, isDrop: false);
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.AtkDmg})", IntData, out var pwdBuff, out var rate);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int atkDmg = player.PlayerAttr.AtkDmg;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), atkDmg, BaseCard.GetValueColor(atkDmg, num), num);
	}
}
