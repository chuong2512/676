public class EquipCard_E_Hands_2 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_2(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.AddBaseArmor(2);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerAttr.ReduceBaseArmor(2);
	}
}
