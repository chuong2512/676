public abstract class EquipCard_E_Shoes : EquipmentCard
{
	public EquipCard_E_Shoes(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.AddBaseApAmount(((E_Shoes_CardAttr)equipmentCardAttr).ApRecoveryAmount);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		player.PlayerAttr.ReduceBaseApAmount(((E_Shoes_CardAttr)equipmentCardAttr).ApRecoveryAmount);
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
