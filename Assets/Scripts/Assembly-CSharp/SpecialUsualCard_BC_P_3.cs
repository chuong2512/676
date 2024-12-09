using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecialUsualCard_BC_P_3 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_3(UsualCardAttr usualCardAttr)
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
		Transform[] array = new Transform[Singleton<EnemyController>.Instance.AllEnemies.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.transform;
		}
		UsualCard.HandleEffect(base.EffectConfig, array, delegate
		{
			Effect(player, Singleton<EnemyController>.Instance.AllEnemies, handler);
		});
	}

	private void Effect(Player player, List<EnemyBase> allEnemies, Action handler)
	{
		player.GetBuff(new Buff_Freeze(player, 1));
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].GetBuff(new Buff_Freeze(allEnemies[i], 1));
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：包括自身内的所有目标获得1回合的冻结");
		}
		handler?.Invoke();
	}
}
