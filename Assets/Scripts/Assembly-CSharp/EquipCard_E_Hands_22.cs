public class EquipCard_E_Hands_22 : EquipCard_E_Hands
{
	private class EquipCard_E_Hands_22_BattleEffect_1 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCard;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_M_8")
			{
				IntData = 3;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private class EquipCard_E_Hands_22_BattleEffect_2 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardApCostReduce;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_M_8")
			{
				IntData = -1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private EquipCard_E_Hands_22_BattleEffect_1 _battleEffect1;

	private EquipCard_E_Hands_22_BattleEffect_2 _battleEffect2;

	public EquipCard_E_Hands_22(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_battleEffect1 = new EquipCard_E_Hands_22_BattleEffect_1();
		_battleEffect2 = new EquipCard_E_Hands_22_BattleEffect_2();
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		player.PlayerEffectContainer.AddBattleEffect(_battleEffect1);
		player.PlayerEffectContainer.AddBattleEffect(_battleEffect2);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		player.PlayerEffectContainer.RemoveBattleEffect(_battleEffect1);
		player.PlayerEffectContainer.RemoveBattleEffect(_battleEffect2);
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
