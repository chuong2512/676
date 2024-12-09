public class EquipCard_E_Armor_11 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	private void OnBattleEnd(EventData data)
	{
		if (!Singleton<GameManager>.Instance.Player.IsDead)
		{
			int value = Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth - Singleton<GameManager>.Instance.Player.PlayerAttr.Health;
			Singleton<GameManager>.Instance.Player.EntityAttr.RecoveryHealth(value);
		}
	}
}
