using System;
using UnityEngine;

public class SkillCard_S_A_38 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_38(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		EnemyBase enemyBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, enemyBase, handler);
		});
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "3", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 3, IntData);
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
			Buff_Shocked buff_Shocked;
			if ((buff_Shocked = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(enemyBase, BuffType.Buff_Shocked) as Buff_Shocked) != null)
			{
				int buffRemainRound = buff_Shocked.BuffRemainRound;
				enemyBase.GetBuff(new Buff_Power(enemyBase, -buffRemainRound));
			}
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int baseValue = 3;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}
}
