public class EquipCard_E_Hands_1 : EquipCard_E_Hands
{
	private int cardAmount;

	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Hands_1(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		cardAmount = 0;
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
		cardAmount++;
		if (cardAmount == 5)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, 1));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			cardAmount = 0;
		}
		_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}

	private void OnBattleStart(EventData data)
	{
		cardAmount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return cardAmount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), cardAmount);
	}
}
