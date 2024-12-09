public class EquipCard_E_Mainhand_10 : EquipCard_E_Mainhand
{
	public EquipCard_E_Mainhand_10(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && (simpleEventData.stringValue == "BC_M_0" || simpleEventData.stringValue == "BC_M_1"))
		{
			EntityBase enemyPlayerChoose = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
			if (!enemyPlayerChoose.IsNull())
			{
				enemyPlayerChoose.GetBuff(new Buff_Bleeding(enemyPlayerChoose, 1));
				EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			}
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
