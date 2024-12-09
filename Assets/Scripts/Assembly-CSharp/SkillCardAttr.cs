using System;

[Serializable]
public class SkillCardAttr : BaseCardAttr
{
	public int ApCost;

	public int SpecialAttrCost;

	public string IllustrationName;

	public string SupHandCardCode;

	public int SupHandCardConsumeAmount;

	public string MainHandCardCode;

	public string EffectConfigName;

	public int MainHandCardConsumeAmount;

	public int LevelLimit;

	public int Price;

	public bool IsNeedPurchased;
}
