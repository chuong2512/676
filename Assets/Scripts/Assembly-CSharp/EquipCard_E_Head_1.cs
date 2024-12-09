public class EquipCard_E_Head_1 : EquipCard_E_Head
{
	public EquipCard_E_Head_1(EquipmentCardAttr equipmentCardAttr)
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
