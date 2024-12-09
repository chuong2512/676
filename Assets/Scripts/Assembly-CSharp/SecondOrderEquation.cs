using UnityEngine;

public class SecondOrderEquation
{
	private float a;

	private float b;

	private float c;

	public SecondOrderEquation(float a, float b, float c)
	{
		this.a = a;
		this.b = b;
		this.c = c;
	}

	public float GetY(float x)
	{
		return a * Mathf.Pow(x, 2f) + b * x + c;
	}

	public Vector2 GetPos(float x)
	{
		return new Vector2(x, GetY(x));
	}

	public override string ToString()
	{
		return $"y = {a}x^2 + {b}x + {c}";
	}
}
