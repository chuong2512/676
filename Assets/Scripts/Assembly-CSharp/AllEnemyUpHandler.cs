using System;
using UnityEngine;

public class AllEnemyUpHandler : PointUpHandler
{
	public override void PointUp(Vector3 pointViewRect, Action tryUseAction, Action tryCancelAction)
	{
		OnPointUp(pointViewRect);
		if (pointViewRect.y <= 0.34f)
		{
			tryCancelAction?.Invoke();
		}
		else
		{
			tryUseAction?.Invoke();
		}
		BattleEnvironmentManager.Instance.HideAllEnemyHint();
	}

	protected virtual void OnPointUp(Vector3 pointViewRect)
	{
	}
}
