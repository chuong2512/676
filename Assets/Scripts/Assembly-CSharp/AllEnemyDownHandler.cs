using UnityEngine;

public class AllEnemyDownHandler : PointDownHandler
{
	protected bool isPointOnTopScreen;

	public override void OnPointDown()
	{
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			BattleEnvironmentManager.Instance.ShowEnemyHint(Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl, Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.hintOffset);
		}
	}

	public override void HandleDown(Vector3 pointViewRect)
	{
		if (pointViewRect.y <= 0.34f && isPointOnTopScreen)
		{
			CancelAllEnemyHighlight();
		}
		else if (pointViewRect.y > 0.34f && !isPointOnTopScreen)
		{
			SetAllEnemyHighlight();
		}
		OnHandleDown(pointViewRect);
	}

	protected virtual void OnHandleDown(Vector3 pointViewRect)
	{
	}

	public override void HandleShowCastHint(Transform target)
	{
	}

	public override void HandleEndCastHit()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}

	private void SetAllEnemyHighlight()
	{
		isPointOnTopScreen = true;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).ShowEnemyAreaHint();
		BattleEnvironmentManager.Instance.SetAllEnemyHintHighlight();
	}

	private void CancelAllEnemyHighlight()
	{
		isPointOnTopScreen = false;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
		BattleEnvironmentManager.Instance.CancelAllEnemyHintHighlight();
	}
}
