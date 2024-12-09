public abstract class EquipCard_E_Offhand : EquipmentCard
{
	public EquipCard_E_Offhand(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.AddBaseDefenceAttr(((E_Offhand_CardAttr)equipmentCardAttr).DefenceAttrAmount);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		player.PlayerAttr.ReduceBaseDefenceAttra(((E_Offhand_CardAttr)equipmentCardAttr).DefenceAttrAmount);
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
