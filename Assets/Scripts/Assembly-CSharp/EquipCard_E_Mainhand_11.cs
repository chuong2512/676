public class EquipCard_E_Mainhand_11 : EquipCard_E_Mainhand
{
	private class BattleEffect_E_Mainhand_11 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardApCostReduce;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_M_0")
			{
				IntData = -1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private BattleEffect_E_Mainhand_11 battleEffect;

	public EquipCard_E_Mainhand_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		battleEffect = new BattleEffect_E_Mainhand_11();
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerEffectContainer.AddBattleEffect(battleEffect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerEffectContainer.RemoveBattleEffect(battleEffect);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_OnShuffleAllCards, OnShuffleAllCards);
	}

	private void OnShuffleAllCards(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TurnAllMainHandSpecificCard("BC_M_1", "BC_M_0");
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
