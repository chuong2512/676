public abstract class EquipCard_E_Head : EquipmentCard
{
	public EquipCard_E_Head(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.SetMemoryAmount(((E_Head_CardAttr)equipmentCardAttr).MemoryAmount);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
