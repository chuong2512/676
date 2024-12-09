using UnityEngine;

public class DeBuffMeanHandler : MeanHandler
{
	private const string DebuffMean = "Mean_DeBuff";

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
		return "Mean_DeBuff".LocalizeText();
	}

	public override string GetMeanTrigger()
	{
		return "DebuffTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_减益", "Sprites/Mean");
	}
}
