public class EquipCard_E_Shoes_2 : EquipCard_E_Shoes
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private int usualCardCount;

	public EquipCard_E_Shoes_2(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	private void OnBattleStart(EventData data)
	{
		usualCardCount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return usualCardCount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}

	private void OnPlayerRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount != 0)
		{
			usualCardCount = 0;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
		}
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		usualCardCount++;
		if (usualCardCount == 3)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(1);
			usualCardCount = 0;
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}
}
