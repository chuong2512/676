using System;
using UnityEngine;

public class UsualCard_BC_M_6 : UsualCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public UsualCard_BC_M_6(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler_ArcherComsume(1, isDrop: false);
		pointupHandler = new SingleEnemyUpHandler_ArrowComsume(1, isDrop: false);
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
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
	}

	private void Effect(Player player, EntityBase enTarget, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enTarget, out var buff))
		{
			buff.TakeEffect(enTarget);
			UsualCard_Archer.ComsumeSpecialArrow(player, null, 1, isDrop: false);
		}
		else
		{
			player.PlayerAtkEnemy(enTarget, realDmg, isTrueDmg: false);
			UsualCard_Archer.ComsumeSpecialArrow(player, new EntityBase[1] { enTarget }, 1, isDrop: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		UsualCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
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

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}

	protected override bool IsSatisfiedSpecialAttr(Player player, out string failResult)
	{
		return UsualCard_Archer.CheckSpecialAttr(player, 1, out failResult);
	}
}
