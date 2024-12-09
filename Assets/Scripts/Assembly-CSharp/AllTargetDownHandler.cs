using UnityEngine;

public class AllTargetDownHandler : PointDownHandler
{
	private bool isPointOnTopScreen;

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
			CancelSelfHighLight();
		}
		else if (pointViewRect.y > 0.34f && !isPointOnTopScreen)
		{
			Debug.Log("enter");
			SetAllEnemyHighlight();
			SetSelfHighlight();
		}
	}

	public override void HandleShowCastHint(Transform target)
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).HighlightSelfCast();
	}

	public override void HandleEndCastHit()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).CancelHighlighSelfCast();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}

	private void SetAllEnemyHighlight()
	{
		isPointOnTopScreen = true;
		BattleEnvironmentManager.Instance.SetAllEnemyHintHighlight();
	}

	private void CancelAllEnemyHighlight()
	{
		isPointOnTopScreen = false;
		BattleEnvironmentManager.Instance.CancelAllEnemyHintHighlight();
	}

	private void SetSelfHighlight()
	{
		isPointOnTopScreen = true;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).ShowEnemyAreaHint();
	}

	private void CancelSelfHighLight()
	{
		isPointOnTopScreen = false;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}
}
