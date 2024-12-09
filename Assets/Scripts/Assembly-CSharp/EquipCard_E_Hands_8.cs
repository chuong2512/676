public class EquipCard_E_Hands_8 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_8(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleOver);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleOver);
	}

	private void OnBattleOver(EventData data)
	{
		if (!Singleton<GameManager>.Instance.Player.IsDead)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryHealth(3);
		}
	}
}
