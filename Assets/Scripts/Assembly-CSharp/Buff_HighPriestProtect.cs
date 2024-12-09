using System.Collections.Generic;
using UnityEngine;

public class Buff_HighPriestProtect : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_HighPriestProtect;

	public Buff_HighPriestProtect(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("大祭司被动触发，为所有己方目标添加一回合的神圣庇护");
		}
		buffIconCtrl.BuffEffectHint();
		Transform[] array = new Transform[Singleton<EnemyController>.Instance.AllEnemies.Count];
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			array[i] = Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.transform;
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, array, BuffEffect);
	}

	private void BuffEffect()
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].GetBuff(new Buff_HolyProtect(allEnemies[i], 1));
		}
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
