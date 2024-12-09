public class EquipCard_E_Head_25 : EquipCard_E_Head
{
	private int arrowAmount;

	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Head_25(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnComsumeSpecialAttr(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			arrowAmount += simpleEventData.intValue;
		}
		if (arrowAmount >= 4)
		{
			int num = arrowAmount / 4;
			arrowAmount -= 4 * num;
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Dodge(Singleton<GameManager>.Instance.Player, num));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}

	private void OnBattleStart(EventData data)
	{
		arrowAmount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return arrowAmount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), arrowAmount);
	}
}
