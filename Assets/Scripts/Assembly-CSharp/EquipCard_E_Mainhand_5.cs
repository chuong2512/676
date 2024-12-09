public class EquipCard_E_Mainhand_5 : EquipCard_E_Mainhand
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private bool isPlayerDefence;

	public EquipCard_E_Mainhand_5(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetBuff);
		EventManager.RegisterObjRelatedEvent(player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_GetNewBuff, OnPlayerGetBuff);
		EventManager.UnregisterObjRelatedEvent(player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
	}

	private void OnBattleStart(EventData data)
	{
		isPlayerDefence = false;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetNotEffect();
	}

	private string GetEquipEffectHint()
	{
		if (!isPlayerDefence)
		{
			return "0";
		}
		return "2";
	}

	private void OnPlayerGetBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, 2));
			isPlayerDefence = true;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
			_equipEffectIconCtrl.SetEffect();
		}
	}

	private void OnPlayerRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, -2));
			isPlayerDefence = false;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
			_equipEffectIconCtrl.SetNotEffect();
		}
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}
}
