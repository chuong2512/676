public abstract class EquipCard_E_Trinket : EquipmentCard
{
	protected EquipCard_E_Trinket(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
