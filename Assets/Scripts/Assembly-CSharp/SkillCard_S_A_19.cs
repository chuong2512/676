using System;
using UnityEngine;

public class SkillCard_S_A_19 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_19(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler_ArcherComsume(1, isDrop: false);
		pointupHandler = new SingleEnemyUpHandler_ArrowComsume(1, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 1);
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enTarget, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enTarget, out var buff))
		{
			buff.TakeEffect(enTarget);
			SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
		}
		else
		{
			player.PlayerAtkEnemy(enTarget, realDmg, isTrueDmg: false);
			SkillCard_Archer.ComsumeSpecialAttr(player, new EntityBase[1] { enTarget }, 1, isDrop: false);
			enTarget.GetBuff(new Buff_Power(enTarget, -1));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes}), 并使得力量-1");
		}
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.AtkDmg})", IntData, out var pwdBuff, out var rate);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int atkDmg = player.PlayerAttr.AtkDmg;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), atkDmg, BaseCard.GetValueColor(atkDmg, num), num);
	}
}
