public class EquipCard_E_Head_22 : EquipCard_E_Head
{
	private int arrowAddAmount;

	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Head_22(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_AddSpecialArrow, OnPlayerAddSpecialArrow);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_AddSpecialArrow, OnPlayerAddSpecialArrow);
	}

	private void OnBattleStart(EventData data)
	{
		arrowAddAmount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return arrowAddAmount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), arrowAddAmount);
	}

	private void OnPlayerAddSpecialArrow(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			arrowAddAmount += simpleEventData.intValue;
		}
		if (arrowAddAmount >= 3)
		{
			int num = arrowAddAmount / 3;
			arrowAddAmount -= 3 * num;
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DefenceToAttack(Singleton<GameManager>.Instance.Player, num));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}
}
