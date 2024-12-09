using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_23 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_23(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new AllEnemyDownHandler();
		pointupHandler = new AllEnemyUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		Transform[] array = new Transform[allEnemies.Count];
		for (int i = 0; i < array.Length; i++)
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
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].GetBuff(new Buff_DeadPoison(allEnemies[i], 3));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：所有的敌人上3层致命毒药");
		}
		handler?.Invoke();
	}
}
