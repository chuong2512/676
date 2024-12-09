public class EquipCard_E_Offhand_20 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_20(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnEndPlayerRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnEndPlayerRound);
	}

	private void OnEndPlayerRound(EventData data)
	{
		int amount = ((E_Offhand_CardAttr)equipmentCardAttr).DefenceAttrAmount - Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr;
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(amount);
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
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
