using System;
using UnityEngine;

public class SelfUpHandler : PointUpHandler
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
	}

	protected virtual void OnPointUp(Vector3 pointViewRect)
	{
	}
}
