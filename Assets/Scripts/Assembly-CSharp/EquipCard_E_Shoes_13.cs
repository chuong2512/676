public class EquipCard_E_Shoes_13 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_13(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.AddBaseArmor(4);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerAttr.ReduceArmor(4);
	}
}
