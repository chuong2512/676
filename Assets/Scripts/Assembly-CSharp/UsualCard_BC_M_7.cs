using System;
using System.Collections.Generic;
using UnityEngine;

public class UsualCard_BC_M_7 : UsualCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public UsualCard_BC_M_7(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler_ArrowComsume(1, isDrop: false);
		pointupHandler = new AllEnemyUpHandler_ArrowComsume(1, isDrop: false);
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int atkDmg = player.PlayerAttr.AtkDmg;
		int num = RealDmg(player);
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.AtkDmg, BaseCard.GetValueColor(atkDmg, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		UsualCard_Archer.ComsumeSpecialAttrNotUpdate(player, 1);
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
			EntityBase[] targets = new EnemyBase[1] { allEnemies[i] };
			UsualCard_Archer.ActiveArrowEffect(player, targets);
		}
		UsualCard_Archer.ComsumeSpecialArrow(player, null, 1, isDrop: false);
		UsualCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.AtkDmg})", IntData, out var pwdBuff, out var rate);
		return UsualCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return UsualCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData);
	}

	protected override bool IsSatisfiedSpecialAttr(Player player, out string failResult)
	{
		return UsualCard_Archer.CheckSpecialAttr(player, 1, out failResult);
	}
}
