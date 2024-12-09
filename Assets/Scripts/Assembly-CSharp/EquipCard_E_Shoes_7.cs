public class EquipCard_E_Shoes_7 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_7(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(4);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
