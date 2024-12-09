public class EquipCard_E_Offhand_27 : EquipCard_E_Offhand
{
	private class EquipCard_E_Offhand_27_BattleEffect : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponArrowEffectTakeEffect;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.intData == 3)
			{
				IntData = 1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private EquipCard_E_Offhand_27_BattleEffect _battleEffect;

	public EquipCard_E_Offhand_27(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_battleEffect = new EquipCard_E_Offhand_27_BattleEffect();
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.AddBattleEffect(_battleEffect);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		Singleton<GameManager>.Instance.Player.PlayerEffectContainer.RemoveBattleEffect(_battleEffect);
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
