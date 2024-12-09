public class EquipCard_E_Offhand_13 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_13(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
	}

	private void OnPlayerGetNewBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(3);
		}
	}

	private void OnBattleStart(EventData data)
	{
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes).SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return string.Empty;
	}

	private string GetEquipEffectDes()
	{
		return equipmentCardAttr.EquipEffectDesKey.LocalizeText();
	}
}
