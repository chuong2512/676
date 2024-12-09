public class EquipCard_E_Head_8 : EquipCard_E_Head
{
	public EquipCard_E_Head_8(EquipmentCardAttr equipmentCardAttr)
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
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(4);
		}
	}
}
