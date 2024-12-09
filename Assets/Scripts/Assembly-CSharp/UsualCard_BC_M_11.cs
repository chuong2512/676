using System;
using UnityEngine;

public class UsualCard_BC_M_11 : UsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public UsualCard_BC_M_11(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
		player.PlayerBattleInfo.TryDrawSupHandCards(1);
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
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes}),并且让副手抽一张牌");
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "2", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 2, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, 2, IntData);
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int num = RealDmg(player);
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(2, num), num);
	}
}
