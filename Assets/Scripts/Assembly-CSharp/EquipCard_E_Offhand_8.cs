public class EquipCard_E_Offhand_8 : EquipCard_E_Offhand
{
	private class BattleEffect_E_Offhand_8 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardApCostReduce;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "BC_O_0")
			{
				IntData = -1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private BaseBattleEffect battleEffect;

	public EquipCard_E_Offhand_8(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		battleEffect = new BattleEffect_E_Offhand_8();
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerEffectContainer.AddBattleEffect(battleEffect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerEffectContainer.RemoveBattleEffect(battleEffect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
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
