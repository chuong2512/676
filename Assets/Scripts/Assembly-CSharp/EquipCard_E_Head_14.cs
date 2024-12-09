public class EquipCard_E_Head_14 : EquipCard_E_Head
{
	public EquipCard_E_Head_14(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_HolyProtect(Singleton<GameManager>.Instance.Player, 1));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
