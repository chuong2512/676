using System;
using System.Collections;
using UnityEngine;

public class SkillCard_S_K_3 : SkillCard
{
	private int realDmg;

	private string atkDes;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_3(SkillCardAttr skillCardAttr)
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
		EnemyBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		SingletonDontDestroy<Game>.Instance.StartCoroutine(Effect_IE(player, enemyPlayerChoose));
		handler?.Invoke();
	}

	private IEnumerator Effect_IE(Player player, EnemyBase entityBase)
	{
		int i = 0;
		while (i < 3)
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

	private void Effect(Player player, EnemyBase entityBase, int i)
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
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{entityBase.EntityName}造成第{i + 1}次非真实的{realDmg}点伤害({atkDes})");
		}
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.AtkDmg})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.AtkDmg, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.AtkDmg, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int atkDmg = player.PlayerAttr.AtkDmg;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.AtkDmg, BaseCard.GetValueColor(atkDmg, num), num);
	}
}
