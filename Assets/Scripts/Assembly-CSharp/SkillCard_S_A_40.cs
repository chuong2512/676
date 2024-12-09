using System;
using System.Collections;
using UnityEngine;

public class SkillCard_S_A_40 : SkillCard_Archer
{
	private int realDmg;

	private string atkDes;

	private int suphandCardAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_40(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler_ArcherComsume(3, isDrop: false);
		pointupHandler = new SingleEnemyUpHandler_ArrowComsume(3, isDrop: false);
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard_Archer.ComsumeSpecialAttrNotUpdate(player, 3);
		suphandCardAmount = player.PlayerBattleInfo.SupHandCardAmount;
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		Singleton<GameManager>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose, handler));
	}

	private IEnumerator Effect_IE(Player player, EnemyBase enemyBase, Action handler)
	{
		int i = 0;
		while (i < 3)
		{
			if (enemyBase.IsDead)
			{
				yield break;
			}
			SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, enemyBase);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		SkillCard_Archer.TryEffectShootRelatedEffect(player);
		handler?.Invoke();
	}

	private void Effect(Player player, EnemyBase enemyBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
			SkillCard_Archer.ComsumeSpecialAttr(player, null, 1, isDrop: false);
		}
		else
		{
			player.PlayerAtkEnemy(enemyBase, realDmg, isTrueDmg: false);
			SkillCard_Archer.ComsumeSpecialAttr(player, new EntityBase[1] { enemyBase }, 1, isDrop: false);
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"两倍武器伤害({player.PlayerAttr.AtkDmg * 2})", IntData, out var pwdBuff, out var rate);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg * 2, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseASkillCardPowUp(this, null, out var IntData);
		return SkillCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg * 2, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int baseValue = player.PlayerAttr.AtkDmg * 2;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.AtkDmg, BaseCard.GetValueColor(baseValue, num), num);
	}
}
