using UnityEngine;

public class SpecialMeanHandler : MeanHandler
{
	private const string SpecialMean = "Mean_Special";

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
		return "Mean_Special".LocalizeText();
	}

	public override string GetMeanTrigger()
	{
		return "SpecialTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_特殊", "Sprites/Mean");
	}
}
