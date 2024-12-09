public class EquipCard_E_Shoes_21 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_21(EquipmentCardAttr equipmentCardAttr)
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
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Agile(Singleton<GameManager>.Instance.Player, 2));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
