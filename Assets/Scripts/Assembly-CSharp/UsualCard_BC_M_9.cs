using System;
using System.Collections;
using UnityEngine;

public class UsualCard_BC_M_9 : UsualCard_Archer
{
	private int shootTime;

	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public UsualCard_BC_M_9(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler_ArcherComsume(3, isDrop: false);
		pointupHandler = new SingleEnemyUpHandler_ArrowComsume(3, isDrop: false);
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int atkDmg = player.PlayerAttr.AtkDmg;
		int num = RealDmg(player);
		return string.Format(usualCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.AtkDmg, BaseCard.GetValueColor(atkDmg, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		shootTime = ((player.PlayerAttr.SpecialAttr >= 3) ? 3 : player.PlayerAttr.SpecialAttr);
		UsualCard_Archer.ComsumeSpecialAttrNotUpdate(player, shootTime);
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, EnemyBase entityBase)
	{
		int i = 0;
		while (i < shootTime)
		{
			if (entityBase.IsDead)
			{
				yield break;
			}
			int tmp = i;
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, entityBase, tmp);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
		UsualCard_Archer.TryEffectShootRelatedEffect(player);
	}

	private void Effect(Player player, EnemyBase entityBase, int i)
	{
		if (BaseCard.IsEntityCanDodgeAttack(entityBase, out var buff))
		{
			buff.TakeEffect(entityBase);
		}
		else
		{
			player.PlayerAtkEnemy(entityBase, realDmg, isTrueDmg: false);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成第{i + 1}次非真实的{realDmg}点伤害({atkDes})");
			}
		}
		UsualCard_Archer.ComsumeSpecialArrow(player, new EntityBase[1] { entityBase }, 1, isDrop: false);
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.AtkDmg})", IntData, out var pwdBuff, out var rate);
		return UsualCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerUseAUsualCardPowUp(this, out var IntData);
		return UsualCard_Archer.GetRealDamage_Arrow(player, player.PlayerAttr.AtkDmg, IntData);
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}

	protected override bool IsSatisfiedSpecialAttr(Player player, out string failResult)
	{
		return UsualCard_Archer.CheckSpecialAttr(player, 1, out failResult);
	}
}
