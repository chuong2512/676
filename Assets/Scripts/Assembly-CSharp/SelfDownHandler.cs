using UnityEngine;

public class SelfDownHandler : PointDownHandler
{
	private bool isPointOnTopScreen = true;

	public override void OnPointDown()
	{
	}

	public override void HandleDown(Vector3 pointViewRect)
	{
		if (pointViewRect.y <= 0.34f && isPointOnTopScreen)
		{
			CancelSelfHighLight();
		}
		else if (pointViewRect.y > 0.34f && !isPointOnTopScreen)
		{
			SetSelfHighlight();
		}
		OnHandleDown(pointViewRect);
	}

	protected virtual void OnHandleDown(Vector3 pointViewRect)
	{
	}

	public override void HandleShowCastHint(Transform target)
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).HighlightSelfCast();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).ShowEnemyAreaHint();
	}

	public override void HandleEndCastHit()
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).CancelHighlighSelfCast();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ScreenEffectDefaultLayerUI") as ScreenEffectDefaultLayerUI).HideEnemyAreaHint();
	}

	private void SetSelfHighlight()
	{
		isPointOnTopScreen = false;
	}

	private void CancelSelfHighLight()
	{
		isPointOnTopScreen = true;
	}
}
