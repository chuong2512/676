public class EquipCard_E_Head_10 : EquipCard_E_Head
{
	public EquipCard_E_Head_10(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(1);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
