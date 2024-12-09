public class EquipCard_E_Mainhand_9 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_9(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.SetAtkDmg(7);
	}

	protected override void OnRelease(Player player)
	{
	}
}
