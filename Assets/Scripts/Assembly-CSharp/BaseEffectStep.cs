using System;

[Serializable]
public abstract class BaseEffectStep : IComparable<BaseEffectStep>
{
	public float waitTime;

	public abstract EffectConfigType EffectConfigType { get; }

	public int CompareTo(BaseEffectStep other)
	{
		if (this == other)
		{
			return 0;
		}
		if (other == null)
		{
			return 1;
		}
		return waitTime.CompareTo(other.waitTime);
	}
}
