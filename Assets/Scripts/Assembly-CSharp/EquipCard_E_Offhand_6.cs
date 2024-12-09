public class EquipCard_E_Offhand_6 : EquipCard_E_Offhand
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private bool isInDefence;

	public EquipCard_E_Offhand_6(EquipmentCardAttr equipmentCardAttr)
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

	private void OnBattleStart(EventData data)
	{
		isInDefence = false;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetNotEffect();
	}

	private string GetEquipEffectHint()
	{
		if (!isInDefence)
		{
			return "0";
		}
		return "3";
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}

	private void OnPlayerGetNewBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DamageRestrik(Singleton<GameManager>.Instance.Player, 3));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			isInDefence = true;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
			_equipEffectIconCtrl.SetEffect();
		}
	}

	private void OnPlayerRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DamageRestrik(Singleton<GameManager>.Instance.Player, -3));
			isInDefence = false;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
			_equipEffectIconCtrl.SetNotEffect();
		}
	}
}
