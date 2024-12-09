using System;
using System.Collections;
using UnityEngine;

public class SpecialUsualCard_BC_P_45 : SpecialUsualCard
{
	private static int CardUseTimeInBattle;

	private int realDmg;

	private string atkDes;

	public override int ApCost => specialUsualCardAttr.ApCost + CardUseTimeInBattle;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_45(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 5;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		Singleton<GameManager>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose, handler));
	}

	private IEnumerator Effect_IE(Player player, EnemyBase enemyBase, Action handler)
	{
		int i = 0;
		while (i < 2 && !enemyBase.IsDead)
		{
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, enemyBase);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		handler?.Invoke();
		CardUseTimeInBattle++;
		EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
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
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害({5})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 5, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 5, IntData);
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	private void OnBattleStart(EventData data)
	{
		CardUseTimeInBattle = 0;
		EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void OnBattleEnd(EventData data)
	{
		CardUseTimeInBattle = 0;
	}
}
