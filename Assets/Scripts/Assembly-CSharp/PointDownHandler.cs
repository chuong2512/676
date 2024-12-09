using UnityEngine;

public abstract class PointDownHandler
{
	public abstract void OnPointDown();

	public abstract void HandleDown(Vector3 pointViewRect);

	public abstract void HandleShowCastHint(Transform target);

	public abstract void HandleEndCastHit();
}
