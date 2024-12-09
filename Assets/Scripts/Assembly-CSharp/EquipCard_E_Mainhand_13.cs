public class EquipCard_E_Mainhand_13 : EquipCard_E_Mainhand
{
	private EquipEffectIconCtrl _effectIconCtrl;

	public EquipCard_E_Mainhand_13(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
	}

	private void OnPlayerRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddAtkDmgOnBattle(5);
			_effectIconCtrl.SetEffect();
		}
	}

	private void OnPlayerGetNewBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceAtkDmgOnBattle(5);
			_effectIconCtrl.SetNotEffect();
		}
	}

	private void OnBattleStart(EventData data)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddAtkDmgOnBattle(5);
		_effectIconCtrl.SetEffect();
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
