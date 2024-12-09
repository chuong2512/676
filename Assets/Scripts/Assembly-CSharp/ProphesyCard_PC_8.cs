public class ProphesyCard_PC_8 : ProphesyCard
{
	public override string ProphesyCode => "PC_8";

	public override void Active(bool isLoad)
	{
		if (!isLoad)
		{
			EquipmentCard equipmentCard = FactoryManager.GetEquipmentCard("E_Mainhand_2");
			Singleton<GameManager>.Instance.Player.PlayerEquipment.EquipMainHandWeapon(equipmentCard);
		}
	}
}
