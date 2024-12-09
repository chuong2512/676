using System;

[Serializable]
public class UsualCardAttr : BaseCardAttr
{
	public PlayerOccupation Occupation;

	public HandFlag HandFlag;

	public int ApCost;

	public string IllustrationName;

	public int MaxCardAmount;

	public string EffectConfigName;

	public bool IsComsumeableCard;
}
