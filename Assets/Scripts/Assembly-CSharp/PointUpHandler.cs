using System;
using UnityEngine;

public abstract class PointUpHandler
{
	public abstract void PointUp(Vector3 pointViewRect, Action tryUseAction, Action tryCancelAction);
}
