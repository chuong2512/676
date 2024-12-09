public class EquipCard_E_Armor_15 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_15(EquipmentCardAttr equipmentCardAttr)
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

	private void OnPlayerStoringForce(EventData data)
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Agile(Singleton<GameManager>.Instance.Player, 1));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
