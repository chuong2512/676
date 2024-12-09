using UnityEngine;

public class StunMeanHandler : MeanHandler
{
	private const string StunMean = "Mean_Stun";

	public override string GetSimpleDes0Str()
	{
		return string.Empty;
	}

	public override string GetSimpleDes1Str()
	{
		return string.Empty;
	}

	public override string GetDetailDesStr()
	{
		return "Mean_Stun".LocalizeText();
	}

	public override string GetMeanTrigger()
	{
		return "StunTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_昏迷", "Sprites/Mean");
	}
}
