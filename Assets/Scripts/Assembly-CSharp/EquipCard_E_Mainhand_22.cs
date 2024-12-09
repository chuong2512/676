public class EquipCard_E_Mainhand_22 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_22(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void OnPlayerRound(EventData data)
	{
		int mainHandCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount;
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShootStrengthen(Singleton<GameManager>.Instance.Player, mainHandCardAmount));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}

	private void OnComsumeSpecialAttr(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropMainHandCard();
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
