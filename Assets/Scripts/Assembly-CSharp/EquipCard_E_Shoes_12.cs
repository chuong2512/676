public class EquipCard_E_Shoes_12 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_12(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
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

	private void OnPlayerRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount > 0)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropMainHandCard();
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
	}
}
