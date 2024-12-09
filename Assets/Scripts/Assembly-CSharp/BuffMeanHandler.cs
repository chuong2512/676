using UnityEngine;

public class BuffMeanHandler : MeanHandler
{
	private const string BuffMean = "Mean_Buff";

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
		return "Mean_Buff".LocalizeText();
	}

	public override string GetMeanTrigger()
	{
		return "BuffTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_增益", "Sprites/Mean");
	}
}
