public class EquipCard_E_Shoes_9 : EquipCard_E_Shoes
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private int usualCardAmount;

	public EquipCard_E_Shoes_9(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		usualCardAmount++;
		if (usualCardAmount == 10)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(1);
			usualCardAmount = 0;
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}

	private void OnBattleStart(EventData data)
	{
		usualCardAmount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return usualCardAmount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}
}
