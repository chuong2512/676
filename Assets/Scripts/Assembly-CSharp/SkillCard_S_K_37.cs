using System;
using UnityEngine;

public class SkillCard_S_K_37 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_37(SkillCardAttr skillCardAttr)
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
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
		{
			Effect(player, entityBase, handler);
		});
	}

	private void Effect(Player player, EnemyBase enemyBase, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
		}
		else
		{
			int num = 5;
			Buff_Shocked buff_Shocked = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(enemyBase, BuffType.Buff_Shocked) as Buff_Shocked;
			if (!buff_Shocked.IsNull())
			{
				num += 5 * buff_Shocked.BuffRemainRound;
			}
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次{num}点绝对伤害");
			}
			player.PlayerAtkEnemy(enemyBase, num, isTrueDmg: true);
		}
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
