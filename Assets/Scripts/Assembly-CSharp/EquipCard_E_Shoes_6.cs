public class EquipCard_E_Shoes_6 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_6(EquipmentCardAttr equipmentCardAttr)
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
		Player player = Singleton<GameManager>.Instance.Player;
		player.GetBuff(new Buff_Power(player, 2));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
