using System;
using System.Collections;
using UnityEngine;

public class SkillCard_S_A_35 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	private int atkTime;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_35(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		atkTime = 1;
		realDmg = RealDmg(player, out atkDes);
		Buff_Dodge buff_Dodge = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Dodge) as Buff_Dodge;
		if (buff_Dodge != null)
		{
			atkTime += buff_Dodge.BuffRemainRound;
		}
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		Singleton<GameManager>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose, new Transform[1] { enemyPlayerChoose.EntityTransform }, handler));
	}

	private IEnumerator Effect_IE(Player player, EnemyBase enemyBase, Transform[] allTargets, Action handler)
	{
		int i = 0;
		while (i < atkTime)
		{
			SkillCard.HandleEffect(base.EffectConfig, allTargets, delegate
			{
				Effect(player, enemyBase);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, "3", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.Armor, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 3, IntData);
	}

	private void Effect(Player player, EnemyBase target)
	{
		if (BaseCard.IsEntityCanDodgeAttack(target, out var buff))
		{
			buff.TakeEffect(target);
		}
		else
		{
			player.PlayerAtkEnemy(target, realDmg, isTrueDmg: true);
		}
		player.PlayerBattleInfo.TryDrawMainHandCards(1);
		player.PlayerBattleInfo.TryDrawSupHandCards(1);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int baseValue = 3;
		int num = RealDmg(player);
		int num2 = 0;
		Buff_Dodge buff_Dodge;
		if ((buff_Dodge = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Dodge) as Buff_Dodge) != null)
		{
			num2 = buff_Dodge.BuffRemainRound;
		}
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num, num2);
	}
}
