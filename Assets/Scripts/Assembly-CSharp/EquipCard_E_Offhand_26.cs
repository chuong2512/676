public class EquipCard_E_Offhand_26 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_26(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_SpecialArrowEffect, OnSpecialArrowEffect);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_SpecialArrowEffect, OnSpecialArrowEffect);
	}

	private void OnSpecialArrowEffect(EventData data)
	{
		SimpleEventData simpleEventData;
		EntityBase[] array;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.intValue == 5 && (array = simpleEventData.objValue as EntityBase[]) != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetBuff(new Buff_BrokenArmor(array[i], 1));
			}
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
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
