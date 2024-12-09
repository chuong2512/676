public class EquipCard_E_Mainhand_25 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_25(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayerUseSkill(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "S_A_45")
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(2);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(2);
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
