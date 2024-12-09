public class EquipCard_E_Armor_13 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_13(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
	}

	private void OnBattleStart(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(1);
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
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount != 0)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(1);
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
	}
}
