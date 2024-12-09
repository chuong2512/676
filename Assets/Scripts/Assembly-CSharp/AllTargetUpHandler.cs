using System;
using UnityEngine;

public class AllTargetUpHandler : PointUpHandler
{
	public override void PointUp(Vector3 pointViewRect, Action tryUseAction, Action tryCancelAction)
	{
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
}
