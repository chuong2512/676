using System;

[Serializable]
public struct Vec2Seri
{
	public int x;

	public int y;

	public Vec2Seri(int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public override string ToString()
	{
		return $"({x}, {y})";
	}
}
