using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_0 : SpecialUsualCard
{
	private static int CardUseTimeInBattle;

	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_0(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
	}

	private void Effect(Player player, EnemyBase enTarget, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enTarget, out var buff))
		{
			buff.TakeEffect(enTarget);
		}
		else
		{
			player.PlayerAtkEnemy(enTarget, realDmg, isTrueDmg: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		handler?.Invoke();
		CardUseTimeInBattle++;
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 1 + CardUseTimeInBattle * 3;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), CardUseTimeInBattle, BaseCard.GetValueColor(baseValue, num), num);
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"基础伤害：({1 + CardUseTimeInBattle * 3})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 1 + CardUseTimeInBattle * 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 1 + CardUseTimeInBattle * 3, IntData);
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		CardUseTimeInBattle = 0;
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}
}
