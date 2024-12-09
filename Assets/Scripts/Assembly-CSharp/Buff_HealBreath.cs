using System.Collections.Generic;
using UnityEngine;

public class Buff_HealBreath : BaseBuff
{
	private List<EnemyBase> allTargets;

	public override BuffType BuffType => BuffType.Buff_HealBreath;

	public Buff_HealBreath(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "死亡被动触发：所有己方目标恢复12点生命值");
		}
		allTargets = new List<EnemyBase>();
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			EnemyBase enemyBase = Singleton<EnemyController>.Instance.AllEnemies[i];
			if (enemyBase != entityBase)
			{
				allTargets.Add(enemyBase);
			}
		}
		Transform[] array = new Transform[allTargets.Count];
		for (int j = 0; j < allTargets.Count; j++)
		{
			array[j] = allTargets[j].EnemyCtrl.transform;
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, array, BuffEffect);
	}

	private void BuffEffect()
	{
		for (int i = 0; i < allTargets.Count; i++)
		{
			allTargets[i].EntityAttr.RecoveryHealth(12);
		}
		allTargets.Clear();
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}
}
