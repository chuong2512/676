public abstract class EquipCard_E_Hands : EquipmentCard
{
	public EquipCard_E_Hands(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.SetDrawCardAmount(((E_Hands_CardAttr)equipmentCardAttr).DrawCardAmount);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
