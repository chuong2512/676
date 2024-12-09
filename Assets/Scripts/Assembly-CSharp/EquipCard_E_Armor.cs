public abstract class EquipCard_E_Armor : EquipmentCard
{
	protected EquipCard_E_Armor(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.AddBaseArmor(((E_Armor_CardAttr)equipmentCardAttr).BaseArmorAmount);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		player.PlayerAttr.ReduceBaseArmor(((E_Armor_CardAttr)equipmentCardAttr).BaseArmorAmount);
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
