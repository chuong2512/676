using System;
using UnityEngine;

public class SkillCard_S_K_43 : SkillCard
{
	private int realDmg;

	private string atkDes;

	private const int BaseDmg = 5;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_43(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return true;
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

	private void Effect(Player player, EnemyBase target, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(target, out var buff))
		{
			buff.TakeEffect(target);
		}
		else
		{
			player.PlayerAtkEnemy(target, realDmg, isTrueDmg: false);
		}
		player.PlayerBattleInfo.TryDrawMainHandCards(2);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{target.EntityName}造成1次非真实的{realDmg}点伤害({atkDes}), 主手抽取2张牌");
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "基础伤害(5)", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 5, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 5, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int baseValue = 5;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}
}
