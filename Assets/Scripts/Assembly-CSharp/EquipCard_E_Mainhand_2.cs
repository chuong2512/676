public class EquipCard_E_Mainhand_2 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_2(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
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

	private void OnShuffleAllCards(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TurnAllMainHandSpecificCard("BC_M_1", "BC_M_0");
	}
}
