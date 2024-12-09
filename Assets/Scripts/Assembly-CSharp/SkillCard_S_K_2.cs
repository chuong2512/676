using System;
using System.Collections;
using UnityEngine;

public class SkillCard_S_K_2 : SkillCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_2(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Defence);
	}

	public override bool IsCanCast(Player player, out string failResult)
	{
		bool flag = base.IsCanCast(player, out failResult);
		if (!flag)
		{
			return flag;
		}
		if (!IsSatifySpecialStatus(player))
		{
			failResult = "LackOfDefenceBuff".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		int num = 1;
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingUsualCardEffectTimeAdd, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		num += IntData;
		handler?.Invoke();
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose, num));
	}

	private IEnumerator Effect_IE(Player player, EnemyBase enemyBase, int atkTime)
	{
		int i = 0;
		while (i < atkTime && !enemyBase.IsDead)
		{
			SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, enemyBase);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
	}

	private void Effect(Player player, EnemyBase entityBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(entityBase, out var buff))
		{
			buff.TakeEffect(entityBase);
		}
		else
		{
			player.PlayerAtkEnemy(entityBase, realDmg, isTrueDmg: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		Singleton<GameManager>.Instance.Player.PlayerUseASkillCardPowUp(this, Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose, out var IntData);
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"当前的格挡值({defenceAttr})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, defenceAttr, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		Singleton<GameManager>.Instance.Player.PlayerUseASkillCardPowUp(this, Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose, out var IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.DefenceAttr, IntData, player.PowerBuff, player.PowUpRate());
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		int num = RealDmg(player);
		int baseValue = defenceAttr;
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), defenceAttr, BaseCard.GetValueColor(baseValue, num), num);
	}
}
