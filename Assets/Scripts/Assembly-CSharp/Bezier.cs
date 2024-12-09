using UnityEngine;

public static class Bezier
{
	public static Vector2 GetV2Point(Vector2 start, Vector2 end, float t)
	{
		return start + (end - start) * t;
	}

	public static Vector2 GetV3Point(Vector2 point1, Vector2 point2, Vector2 point3, float t)
	{
		return Mathf.Pow(1f - t, 2f) * point1 + 2f * t * (1f - t) * point2 + Mathf.Pow(t, 2f) * point3;
	}

	public static float GetV3PointAngle(Vector2 point1, Vector2 point2, Vector2 point3, float t)
	{
		Vector2 v2Point = GetV2Point(point1, point2, t);
		Vector2 to = GetV2Point(point2, point3, t) - v2Point;
		return Vector2.SignedAngle(Vector2.up, to);
	}
}
