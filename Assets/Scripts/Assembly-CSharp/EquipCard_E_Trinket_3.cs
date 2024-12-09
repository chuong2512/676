public class EquipCard_E_Trinket_3 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_3(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayerStoringForce(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr);
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
