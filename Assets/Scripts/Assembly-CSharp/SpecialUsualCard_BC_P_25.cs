using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_25 : SpecialUsualCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_25(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyRandomDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		int baseValue = 2;
		int num = RealDmg(player);
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), BaseCard.GetValueColor(baseValue, num), num);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		realDmg = RealDmg(player, out atkDes);
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, Singleton<EnemyController>.Instance.AllEnemies));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, List<EnemyBase> allEnemies)
	{
		int i = 0;
		while (i < 5)
		{
			if (allEnemies.Count == 0)
			{
				break;
			}
			EnemyBase enemyBase = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
			int tmp = i;
			UsualCard.HandleEffect(base.EffectConfig, new Transform[1] { enemyBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, enemyBase, tmp);
			});
			yield return new WaitForSeconds(0.2f);
			int num = i + 1;
			i = num;
		}
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
			entityBase.GetBuff(new Buff_Bleeding(entityBase, 1));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成第{i + 1}次非真实的{realDmg}点伤害({atkDes}), 并上一层流血buff");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"基础伤害({2})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, 2, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, 2, IntData);
	}
}
