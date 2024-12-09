using System.Collections.Generic;
using UnityEngine;

public class BattleBlessing : BaseGift
{
	private EnemyBase targetEnemy;

	public override GiftName Name => GiftName.BattleBlessing;

	public override void OnBattleStart()
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		targetEnemy = allEnemies[Random.Range(0, allEnemies.Count)];
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(Name);
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseGift.LoadConfigByName(giftDataByGiftName.EffectConfigName), null, new Transform[1] { targetEnemy.EntityTransform }, Effect);
	}

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家战斗祝福生效：对随机的敌人造成20点绝对伤害");
		}
		targetEnemy.TakeDamage(20, null, isAbsDmg: true);
	}
}
