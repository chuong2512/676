public class ProphesyCard_PC_9 : ProphesyCard
{
	public override string ProphesyCode => "PC_9";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard("E_Offhand_2");
			Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipSupHandWeapon(equipmentCard);
		}
	}
}
