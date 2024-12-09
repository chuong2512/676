using System;
using UnityEngine;

public class SkillCard_S_K_34 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_34(SkillCardAttr skillCardAttr)
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
		if (Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(entityBase, BuffType.Buff_Shocked))
		{
			SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { entityBase.EnemyCtrl.transform }, delegate
			{
				Effect(player, entityBase);
			});
		}
		handler?.Invoke();
	}

	private void Effect(Player player, EnemyBase enemyBase)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enemyBase, out var buff))
		{
			buff.TakeEffect(enemyBase);
			return;
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enemyBase.EntityName}造成1次非真实的{5}点绝对伤害");
		}
		player.PlayerAtkEnemy(enemyBase, 5, isTrueDmg: true);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
