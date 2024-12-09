using System;
using UnityEngine;

public class SkillCard_S_K_17 : SkillCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_17(SkillCardAttr skillCardAttr)
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
		EnemyBase entityBase = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
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
			player.PlayerAtkEnemy(entityBase, RealDmg(player), isTrueDmg: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ThrowShield(Singleton<GameManager>.Instance.Player, 1));
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"三倍格挡值({defenceAttr * 3})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, defenceAttr * 3, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.DefenceAttr * 3, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		int baseValue = defenceAttr * 3;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), defenceAttr, BaseCard.GetValueColor(baseValue, num), num);
	}
}
