public class EquipCard_E_Mainhand_14 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseAUsualCard);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseAUsualCard);
	}

	private void OnShuffleAllCards(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TurnAllMainHandSpecificCard("BC_M_7", "BC_M_6");
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}

	private void OnPlayerUseAUsualCard(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "BC_M_6")
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
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
