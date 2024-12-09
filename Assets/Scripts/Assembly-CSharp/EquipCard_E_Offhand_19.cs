public class EquipCard_E_Offhand_19 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_19(EquipmentCardAttr equipmentCardAttr)
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
		((ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddRandomSpecialArrows(5, isNeedReplacePreEffect: false);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
