public class EquipCard_E_Hands_12 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_12(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(5);
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(5);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
