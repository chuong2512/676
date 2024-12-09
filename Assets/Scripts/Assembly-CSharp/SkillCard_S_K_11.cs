using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_K_11 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_11(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Defence);
	}

	public override bool IsCanCast(Player player, out string failResult)
	{
		bool flag = base.IsCanCast(player, out failResult);
		if (!flag)
		{
			return flag;
		}
		if (!IsSatifySpecialStatus(player))
		{
			failResult = "LackOfDefenceBuff".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		string atkDes;
		int realDmg = RealDmg(player, out atkDes);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对所有敌人造成1次非真实的{realDmg}点伤害({atkDes}),并对每个目标上三层破甲buff");
		}
		Transform[] array = new Transform[allEnemies.Count];
		for (int i = 0; i < allEnemies.Count; i++)
		{
			array[i] = allEnemies[i].EnemyCtrl.transform;
		}
		SkillCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, allEnemies, realDmg, handler);
		});
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, int dmg, Action handler)
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (BaseCard.IsEntityCanDodgeAttack(allEnemies[i], out var buff))
			{
				buff.TakeEffect(allEnemies[i]);
				continue;
			}
			player.PlayerAtkEnemy(allEnemies[i], dmg, isTrueDmg: false);
			allEnemies[i].GetBuff(new Buff_BrokenArmor(allEnemies[i], 3));
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"双倍当前格挡值({defenceAttr * 2})", IntData, out var pwdBuff, out var rate);
		return Mathf.FloorToInt((float)(defenceAttr * 2 + IntData + pwdBuff) * (1f + rate));
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return Mathf.FloorToInt((float)(player.PlayerAttr.DefenceAttr * 2 + IntData + player.PowerBuff) * (1f + player.PowUpRate()));
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int baseValue = player.PlayerAttr.DefenceAttr * 2;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.DefenceAttr, BaseCard.GetValueColor(baseValue, num), num);
	}
}
