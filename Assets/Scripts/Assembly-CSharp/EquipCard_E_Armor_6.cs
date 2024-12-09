public class EquipCard_E_Armor_6 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_6(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DamageRestrik(Singleton<GameManager>.Instance.Player, 2));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
