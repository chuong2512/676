using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillCard_S_K_8 : SkillCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_8(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对所有目标施加{player.PlayerAttr.SpecialAttr}回合的破甲");
		}
		realDmg = RealDmg(player, out atkDes);
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[allEnemies.Count];
		for (int i = 0; i < allEnemies.Count; i++)
		{
			array[i] = allEnemies[i].EnemyCtrl.transform;
		}
		SkillCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, allEnemies, handler);
		});
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害：{3}", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 3, IntData);
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, Action handler)
	{
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (BaseCard.IsEntityCanDodgeAttack(allEnemies[i], out var buff))
			{
				buff.TakeEffect(allEnemies[i]);
				continue;
			}
			player.PlayerAtkEnemy(allEnemies[i], realDmg, isTrueDmg: false);
			allEnemies[i].GetBuff(new Buff_BrokenArmor(allEnemies[i], 3));
		}
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(3, num), num, player.PlayerAttr.SpecialAttr);
	}
}
