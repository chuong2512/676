using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_A_15 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_15(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler_ArrowComsume(3, isDrop: false);
		pointupHandler = new AllEnemyUpHandler_ArrowComsume(3, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 3);
		List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[list.Count];
		realDmg = RealDmg(player, out atkDes);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对所有敌人造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i].EnemyCtrl.transform;
		}
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, array, list));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, Transform[] allTrans, List<EnemyBase> allEnemies)
	{
		int i = 0;
		while (i < 3)
		{
			if (allEnemies.Count == 0)
			{
				yield break;
			}
			int tmp = i;
			SkillCard.HandleEffect(base.EffectConfig, allTrans, delegate
			{
				Effect(player, allEnemies, tmp);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, int i)
	{
		for (int j = 0; j < allEnemies.Count; j++)
		{
			if (BaseCard.IsEntityCanDodgeAttack(allEnemies[j], out var buff))
			{
				buff.TakeEffect(allEnemies[j]);
				continue;
			}
			player.PlayerAtkEnemy(allEnemies[j], realDmg, isTrueDmg: false);
			SkillCard_Archer.ActiveSpecialAttr(player, new EntityBase[1] { allEnemies[j] });
		}
		SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
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
