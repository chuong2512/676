using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_A_11 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	private int shootTime;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_11(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new AllEnemyRandomDownHandler_ArrowComsume(5, isDrop: false);
		pointupHandler = new AllEnemyUpHandler_ArrowComsume(5, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		shootTime = player.PlayerAttr.SpecialAttr;
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, shootTime);
		realDmg = RealDmg(player, out atkDes);
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, allEnemies));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, List<EnemyBase> allEnemies)
	{
		int i = 0;
		while (i < shootTime)
		{
			if (allEnemies.Count == 0)
			{
				yield break;
			}
			EnemyBase enTarget = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
			SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
			{
				Effect(player, enTarget);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
	}

	private void Effect(Player player, EnemyBase enemyBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
			SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
			return;
		}
		player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
		SkillCard_Archer.ComsumeSpecialAttr(player, new EntityBase[1] { enemyBase }, 1, isDrop: false);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
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
