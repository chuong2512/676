using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_A_29 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	private bool isHaveExtraDmg;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_29(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler_ArcherComsume(1, isDrop: false);
		pointupHandler = new SingleEnemyUpHandler_ArrowComsume(1, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 1);
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		isHaveExtraDmg = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(enemyBase, BuffType.Buff_Shocked);
		realDmg = RealDmg(player, out atkDes);
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enTarget, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enTarget, out var buff))
		{
			buff.TakeEffect(enTarget);
			SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
		}
		else
		{
			player.PlayerAtkEnemy(enTarget, realDmg, isTrueDmg: false);
			SkillCard_Archer.ComsumeSpecialAttr(player, new EntityBase[1] { enTarget }, 1, isDrop: false);
			if (isHaveExtraDmg)
			{
				ExtraDmg(player);
			}
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	private void ExtraDmg(Player player)
	{
		List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].TakeDamage(10, player, isAbsDmg: true);
		}
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
