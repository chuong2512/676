using UnityEngine;

public class Buff_OceanSprite : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_OceanSprite;

	public Buff_OceanSprite(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "被动生效，所有友方+3生命");
		}
		Transform[] array = new Transform[Singleton<EnemyController>.Instance.AllEnemies.Count];
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			array[i] = Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.transform;
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), ((EnemyBase)entityBase).EnemyCtrl.transform, array, BuffEffect);
	}

	private void BuffEffect()
	{
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			Singleton<EnemyController>.Instance.AllEnemies[i].EntityAttr.RecoveryHealth(3);
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
