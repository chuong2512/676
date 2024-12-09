public class EquipCard_E_Offhand_24 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_24(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "BC_O_7")
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(1);
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
