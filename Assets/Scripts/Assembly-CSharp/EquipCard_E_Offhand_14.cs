public class EquipCard_E_Offhand_14 : EquipCard_E_Offhand
{
	private class EquipCard_E_Offhand_14_BattleEffect : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_SpecialArrowEffect, OnArrowEffect);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_SpecialArrowEffect, OnArrowEffect);
		}

		private void OnArrowEffect(EventData data)
		{
			SimpleEventData simpleEventData;
			EntityBase[] array;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue == 1 && (array = simpleEventData.objValue as EntityBase[]) != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].GetBuff(new Buff_Power(array[i], -1));
				}
			}
		}
	}

	private EquipCard_E_Offhand_14_BattleEffect _battleEffect;

	public EquipCard_E_Offhand_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		_battleEffect = new EquipCard_E_Offhand_14_BattleEffect();
	}

	protected override void OnEquip(Player player)
	{
		player.PlayerEffectContainer.AddBattleEffect(_battleEffect);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		player.PlayerEffectContainer.RemoveBattleEffect(_battleEffect);
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
