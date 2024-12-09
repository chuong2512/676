public class EquipCard_E_Offhand_23 : EquipCard_E_Offhand
{
	private class EquipCard_E_Offhand_23_BattleEffect : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponArrowEffectTakeEffect;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null)
			{
				IntData = ((simpleEffectData.intData == 4) ? 1 : 0);
			}
			else
			{
				IntData = 0;
			}
		}
	}

	private EquipCard_E_Offhand_23_BattleEffect _battleEffect;

	public EquipCard_E_Offhand_23(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_battleEffect = new EquipCard_E_Offhand_23_BattleEffect();
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
