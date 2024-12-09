public class EquipCard_E_Armor_7 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_7(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(6);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
