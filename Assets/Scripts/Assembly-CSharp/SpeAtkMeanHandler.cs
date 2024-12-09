using UnityEngine;

public class SpeAtkMeanHandler : MeanHandler
{
	private const string SpeAtkMean = "Mean_SpeAtk";

	private int singleAtkDmg;

	private int atkTime;

	private string specialDes;

	public SpeAtkMeanHandler(int singleAtkDmg, int atkTime, string specialDes)
	{
		this.singleAtkDmg = singleAtkDmg;
		this.atkTime = atkTime;
		this.specialDes = specialDes;
	}

	public override string GetSimpleDes0Str()
	{
		return singleAtkDmg.ToString();
	}

	public override string GetSimpleDes1Str()
	{
		return $"× {atkTime}";
	}

	public override string GetDetailDesStr()
	{
		if (specialDes.IsNullOrEmpty())
		{
			return string.Format("Mean_SpeAtk".LocalizeText(), singleAtkDmg, atkTime);
		}
		return string.Format("Mean_SpeAtk".LocalizeText(), singleAtkDmg, atkTime) + "\n" + specialDes;
	}

	public override string GetMeanTrigger()
	{
		return "SpeAtkTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_减益攻击", "Sprites/Mean");
	}
}
