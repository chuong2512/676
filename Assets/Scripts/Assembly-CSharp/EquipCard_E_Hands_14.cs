public class EquipCard_E_Hands_14 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.AddBaseArmor(10);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerAttr.ReduceBaseArmor(10);
	}
}
