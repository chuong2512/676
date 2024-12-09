using UnityEngine;

public class AttackMeanHandler : MeanHandler
{
	private const string AttackMean = "Mean_Attack";

	private int singleAtkDmg;

	private int atkTime;

	public AttackMeanHandler(int dmg, int time)
	{
		singleAtkDmg = dmg;
		atkTime = time;
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
		return string.Format("Mean_Attack".LocalizeText(), singleAtkDmg, atkTime);
	}

	public override string GetMeanTrigger()
	{
		return "AtkTrigger";
	}

	public override Sprite GetMeanIcon()
	{
		return SingletonDontDestroy<ResourceManager>.Instance.LoadSprite("意图_攻击", "Sprites/Mean");
	}
}
