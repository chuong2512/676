public class EquipCard_E_Armor_5 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_5(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerAttr.AddBaseDefenceAttr(1);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerAttr.ReduceBaseDefenceAttra(1);
	}
}
