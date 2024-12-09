using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_44 : SpecialUsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_44(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int health = player.PlayerAttr.Health;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), health, BaseCard.GetValueColor(health, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enemyBase, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
		}
		player.GetBuff(new Buff_Shocked(player, 1));
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"当前生命值({player.PlayerAttr.Health})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.Health, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.Health, IntData);
	}
}
