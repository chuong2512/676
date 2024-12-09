using System;
using UnityEngine;

public class SkillCard_S_A_20 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_20(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
	}

	private void Effect(Player player, EntityBase target, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(target, out var buff))
		{
			buff.TakeEffect(target);
		}
		else if (player.PlayerAtkEnemy(target, realDmg, isTrueDmg: false) > 0)
		{
			player.GetBuff(new Buff_Dodge(player, 1));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{target.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害(3)", IntData, out var pwdBuff, out var rate);
		return SkillCard_Archer.GetRealDamage_Arrow(player, 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		return SkillCard_Archer.GetRealDamage_Arrow(player, 3, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int num = 3;
		int num2 = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), num, BaseCard.GetValueColor(num, num2), num2);
	}
}
