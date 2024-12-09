using UnityEngine;

public class AllEnemyRandomDownHandler : AllEnemyDownHandler
{
	public override void OnPointDown()
	{
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			BattleEnvironmentManager.Instance.ShowEnemyRandomHint(Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl, Singleton<EnemyController>.Instance.AllEnemies[i].EnemyCtrl.hintOffset);
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

	private void CancelAllEnemyHighlight()
	{
		isPointOnTopScreen = false;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}

	private void SetAllEnemyHighlight()
	{
		isPointOnTopScreen = true;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).ShowEnemyAreaHint();
	}
}
