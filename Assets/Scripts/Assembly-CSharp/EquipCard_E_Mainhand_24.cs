public class EquipCard_E_Mainhand_24 : EquipCard_E_Mainhand
{
	private EquipEffectIconCtrl _effectIconCtrl;

	private int shootArrowAmount;

	public EquipCard_E_Mainhand_24(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeAttr);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeAttr);
	}

	private void OnShuffleAllCards(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TurnAllMainHandSpecificCard("BC_M_7", "BC_M_6");
	}

	private void OnPlayerComsumeAttr(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			shootArrowAmount += simpleEventData.intValue;
			if (shootArrowAmount >= 4)
			{
				int num = shootArrowAmount / 4;
				shootArrowAmount -= 4 * num;
				Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(num);
				EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			}
			_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
		}
	}

	private void OnBattleStart(EventData data)
	{
		shootArrowAmount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return shootArrowAmount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), shootArrowAmount);
	}
}
