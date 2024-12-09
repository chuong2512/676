public class EquipCard_E_Hands_6 : EquipCard_E_Hands
{
	private class E_Hands_6 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardPowUp;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_M_1")
			{
				IntData = 3;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private BaseBattleEffect _effect;

	public EquipCard_E_Hands_6(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_effect = new E_Hands_6();
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerEffectContainer.AddBattleEffect(_effect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerEffectContainer.RemoveBattleEffect(_effect);
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
}
