using UnityEngine;

public class SingleEnemyDownHandler : PointDownHandler
{
	private EnemyBase enemyBase;

	public override void OnPointDown()
	{
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			BattleEnvironmentManager.Instance.ShowEnemyHint(Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl, Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.hintOffset);
		}
	}

	public override void HandleDown(Vector3 pointViewRect)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose != null && (enemyBase == null || enemyBase != Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose))
		{
			if (enemyBase != null)
			{
				CancelSelectEnemy();
			}
			SetSelectEnemy(Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose);
		}
		else if (Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose == null && enemyBase != null)
		{
			CancelSelectEnemy();
		}
		OnHandleDown(pointViewRect);
	}

	protected virtual void OnHandleDown(Vector3 pointViewRect)
	{
	}

	public override void HandleShowCastHint(Transform target)
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CastHintUI") as CastHintUI).StartShowCastHint(target);
	}

	public override void HandleEndCastHit()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CastHintUI") as CastHintUI).EndShowCastHint();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}

	private void SetSelectEnemy(EnemyBase enemyBase)
	{
		this.enemyBase = enemyBase;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).ShowEnemyAreaHint();
		BattleEnvironmentManager.Instance.SetEnemyHintHighlight(enemyBase.EnemyCtrl);
	}

	private void CancelSelectEnemy()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
		BattleEnvironmentManager.Instance.CancelEnemyHintHighlight(enemyBase.EnemyCtrl);
		enemyBase = null;
	}
}
