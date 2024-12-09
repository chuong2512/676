public class EquipCard_E_Offhand_16 : EquipCard_E_Offhand
{
	private class EquipCard_E_Offhand_16_BattleEffect : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardApCostReduce;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_O_7")
			{
				IntData = 1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private EquipCard_E_Offhand_16_BattleEffect _battleEffect;

	public EquipCard_E_Offhand_16(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_battleEffect = new EquipCard_E_Offhand_16_BattleEffect();
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		player.PlayerEffectContainer.AddBattleEffect(_battleEffect);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		player.PlayerEffectContainer.RemoveBattleEffect(_battleEffect);
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
