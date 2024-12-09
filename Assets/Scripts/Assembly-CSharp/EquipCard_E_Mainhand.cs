public abstract class EquipCard_E_Mainhand : EquipmentCard
{
	public EquipCard_E_Mainhand(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	public override void Equip(Player player)
	{
		player.PlayerAttr.SetAtkDmg(((E_Mainhand_CardAttr)equipmentCardAttr).AtkDmg);
		OnEquip(player);
	}

	protected abstract void OnEquip(Player player);

	public override void Release(Player player)
	{
		OnRelease(player);
	}

	protected abstract void OnRelease(Player player);
}
