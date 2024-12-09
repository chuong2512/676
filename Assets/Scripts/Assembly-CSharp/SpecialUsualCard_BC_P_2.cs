using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_2 : SpecialUsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_2(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyRandomDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 2;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, allEnemies));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, List<EnemyBase> allEnemies)
	{
		int i = 0;
		while (i < 3)
		{
			if (allEnemies.Count == 0)
			{
				break;
			}
			EnemyBase enTarget = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
			{
				Effect(player, enTarget);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
	}

	private void Effect(Player player, EnemyBase enemyBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
			return;
		}
		player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害({2})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 2, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 2, IntData);
	}
}
