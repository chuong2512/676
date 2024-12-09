using UnityEngine;

public static class SecondOrderFunc
{
	public static SecondOrderEquation GetSecondOrderEquation(Vector2 point1, Vector2 point2, float y)
	{
		Vector2 vector = point2 - point1;
		float num = y - point1.y;
		float num2 = -4f * num / (vector.x * vector.x);
		float num3 = (0f - num2) * vector.x;
		float b = -2f * num2 * point1.x + num3;
		float c = num2 * point1.x * point1.x - num3 * point1.x + point1.y;
		return new SecondOrderEquation(num2, b, c);
	}
}
