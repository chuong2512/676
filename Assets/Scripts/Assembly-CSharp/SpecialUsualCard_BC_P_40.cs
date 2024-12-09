using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_40 : SpecialUsualCard
{
	private int realDmg;

	private string atkDes;

	private int atkTime;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_40(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyRandomDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int num = 6;
		int num2 = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), num, BaseCard.GetValueColor(num, num2), num2);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawCard_IE(player, isMain, handler));
	}

	private IEnumerator WaitDrawCard_IE(Player player, bool isMain, Action handler)
	{
		while (BattleUI.isDrawingSupCard || BattleUI.IsDrawingMainCard)
		{
			yield return null;
		}
		FinalEffect(player, handler);
	}

	private void FinalEffect(Player player, Action handler)
	{
		atkTime = player.PlayerBattleInfo.PlayerCurrentCardAmount;
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.PlayerBattleInfo.ClearSupHandCards(null);
		Singleton<GameManager>.Instance.StartCoroutine(Effect_IE(player, Singleton<EnemyController>.Instance.AllEnemies, handler));
	}

	private IEnumerator Effect_IE(Player player, List<EnemyBase> allEnemies, Action handler)
	{
		int i = 0;
		while (i < atkTime)
		{
			if (allEnemies.Count == 0)
			{
				handler?.Invoke();
				yield break;
			}
			EnemyBase target = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { target.EnemyCtrl.transform }, delegate
			{
				Effect(player, target);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		handler?.Invoke();
	}

	private void Effect(Player player, EnemyBase enemyBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害({6})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 6, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 6, IntData);
	}
}
