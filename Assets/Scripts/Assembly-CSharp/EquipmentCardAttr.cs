using System;

[Serializable]
public class EquipmentCardAttr : BaseCardAttr
{
	public string EquipEffectDesKey;

	public EquipmentType EquipmentType;

	public PlayerOccupation Occupation;

	public SuitType SuitType;

	public string ImageName;

	public int Price;

	public int StageLimit;

	public int SourceLimit;

	public string EquipSoundName;

	public int ShopLimit;

	public bool IsNeedPurchased;
}
