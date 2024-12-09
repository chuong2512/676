public class EquipCard_E_Hands_11 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.SetDrawCardAmount(5);
	}

	protected override void OnRelease(Player player)
	{
	}
}
