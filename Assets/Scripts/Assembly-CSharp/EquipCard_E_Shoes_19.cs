public class EquipCard_E_Shoes_19 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_19(EquipmentCardAttr equipmentCardAttr)
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
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		if ((float)playerAttr.Health / (float)playerAttr.MaxHealth >= 0.5f)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Dodge(Singleton<GameManager>.Instance.Player, 1));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
	}
}
