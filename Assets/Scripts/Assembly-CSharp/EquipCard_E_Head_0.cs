public class EquipCard_E_Head_0 : EquipCard_E_Head
{
	public EquipCard_E_Head_0(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.SetMemoryAmount(3);
	}

	protected override void OnRelease(Player player)
	{
	}
}
