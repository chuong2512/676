using System;
using System.Collections;
using UnityEngine;

public class SkillCard_S_K_4 : SkillCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_4(SkillCardAttr skillCardAttr)
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
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Stable);
		if (specificBuff != null)
		{
			num += ((Buff_Stable)specificBuff).BlockAdd;
		}
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(num, player, enemyPlayerChoose));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(int time, Player player, EnemyBase entityBase)
	{
		int i = 0;
		while (i < time)
		{
			if (entityBase.IsDead)
			{
				break;
			}
			int tmp = i;
			SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, entityBase, tmp);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
	}

	private void Effect(Player player, EnemyBase enemyBase, int i)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成第{i}次非真实的{realDmg}伤害({atkDes})");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		player.PlayerUseASkillCardPowUp(this, Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"基础伤害({defenceAttr})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, defenceAttr, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseASkillCardPowUp(this, Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose, out var IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.DefenceAttr, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		int baseValue = defenceAttr;
		int num = RealDmg(player);
		int num2 = 1;
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Stable);
		if (specificBuff != null)
		{
			num2 += ((Buff_Stable)specificBuff).BlockAdd;
		}
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), defenceAttr, BaseCard.GetValueColor(baseValue, num), num, num2);
	}
}
