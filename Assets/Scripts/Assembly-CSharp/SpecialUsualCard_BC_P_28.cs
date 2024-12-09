using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_28 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_28(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllTargetDownHandler();
		pointupHandler = new AllTargetUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		List<EnemyBase> allEnemies = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
		Transform[] array = new Transform[allEnemies.Count];
		for (int i = 0; i < allEnemies.Count; i++)
		{
			array[i] = allEnemies[i].EnemyCtrl.transform;
		}
		UsualCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, allEnemies, handler);
		});
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(player, out var buff))
		{
			buff.TakeEffect(player);
		}
		else
		{
			player.TakeDamage(10, null, isAbsDmg: true);
		}
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (BaseCard.IsEntityCanDodgeAttack(allEnemies[i], out var buff2))
			{
				buff2.TakeEffect(allEnemies[i]);
			}
			else
			{
				player.PlayerAtkEnemy(allEnemies[i], 10, isTrueDmg: true);
			}
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：对所有对象包括自身造成10点真实伤害");
		}
		handler?.Invoke();
	}
}
