public class EquipCard_E_Shoes_5 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_5(EquipmentCardAttr equipmentCardAttr)
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
