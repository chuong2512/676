public class EquipCard_E_Mainhand_19 : EquipCard_E_Mainhand
{
	private int magicPow;

	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Mainhand_19(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeSpecialAttr);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnEndPlayerRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeSpecialAttr);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnEndPlayerRound);
	}

	private void OnPlayerComsumeSpecialAttr(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			magicPow += simpleEventData.intValue;
			_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
		}
	}

	private void OnEndPlayerRound(EventData data)
	{
		int power = magicPow / 3;
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, power));
		magicPow = 0;
		_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}

	private void OnBattleStart(EventData data)
	{
		magicPow = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return magicPow.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), magicPow);
	}
}
