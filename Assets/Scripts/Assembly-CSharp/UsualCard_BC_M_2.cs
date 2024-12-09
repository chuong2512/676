using System;
using UnityEngine;

public class UsualCard_BC_M_2 : UsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public UsualCard_BC_M_2(UsualCardAttr usualCardAttr)
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
		EnemyBase entityBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, entityBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase entityBase, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(entityBase, out var buff))
		{
			buff.TakeEffect(entityBase);
		}
		else
		{
			player.PlayerAtkEnemy(entityBase, realDmg, isTrueDmg: false);
			entityBase.GetBuff(new Buff_Shocked(entityBase, 1));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes}), 并且为{entityBase.EntityName}上一层震荡Buff");
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
