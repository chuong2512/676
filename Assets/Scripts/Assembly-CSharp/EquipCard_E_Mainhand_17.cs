public class EquipCard_E_Mainhand_17 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_17(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnComsumeSpecialAttr(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			int intValue = simpleEventData.intValue;
			for (int i = 0; i < intValue; i++)
			{
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropMainHandCard();
			}
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
