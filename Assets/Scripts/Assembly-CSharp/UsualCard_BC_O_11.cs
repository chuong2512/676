using System;
using System.Collections.Generic;
using UnityEngine;

public class UsualCard_BC_O_11 : UsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public UsualCard_BC_O_11(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 3;
		int num = RealDmg(player);
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[allEnemies.Count];
		realDmg = RealDmg(player, out atkDes);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对所有敌人造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		for (int i = 0; i < allEnemies.Count; i++)
		{
			array[i] = allEnemies[i].EnemyCtrl.transform;
		}
		UsualCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, allEnemies, handler);
		});
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
			allEnemies[i].GetBuff(new Buff_Shocked(allEnemies[i], 1));
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害(3)", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 3, IntData);
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}
}
