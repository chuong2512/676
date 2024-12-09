public class EquipCard_E_Hands_24 : EquipCard_E_Hands
{
	public EquipCard_E_Hands_24(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, GetNewBuff);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_GetSameBuff, GetSameBuff);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, GetNewBuff);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_GetSameBuff, GetSameBuff);
	}

	private void GetNewBuff(EventData data)
	{
		AddHuntAbility(data);
	}

	private void GetSameBuff(EventData data)
	{
		AddHuntAbility(data);
	}

	private void AddHuntAbility(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_ShadowEscape)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_HuntAbility(Singleton<GameManager>.Instance.Player, buffEventData.intValue));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
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
