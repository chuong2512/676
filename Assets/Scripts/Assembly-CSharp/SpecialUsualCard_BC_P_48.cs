using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_48 : SpecialUsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_48(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override bool IsCanCast(Player player, bool isMain, out int finalApValue, out string failResult)
	{
		bool flag = base.IsCanCast(player, isMain, out finalApValue, out failResult);
		if (!flag)
		{
			return flag;
		}
		return CheckLastHandCard(player, isMain, out failResult);
	}

	private bool CheckLastHandCard(Player player, bool isMain, out string failResult)
	{
		if ((isMain && player.PlayerBattleInfo.MainHandCardAmount == 1) || (!isMain && player.PlayerBattleInfo.SupHandCardAmount == 1))
		{
			failResult = string.Empty;
			return true;
		}
		failResult = "NotLastHandCardKey".LocalizeText();
		return false;
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 25;
		int num = RealDmg(player);
		return string.Format(isMain ? specialUsualCardAttr.DesKeyOnBattle.LocalizeText() : specialUsualCardAttr.DesKeyOnBattleSupHand.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		realDmg = RealDmg(player, out atkDes);
		Transform[] array = new Transform[allEnemies.Count];
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
			}
			else
			{
				player.PlayerAtkEnemy(allEnemies[i], realDmg, isTrueDmg: false);
			}
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害(25)", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 25, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 25, IntData);
	}
}
