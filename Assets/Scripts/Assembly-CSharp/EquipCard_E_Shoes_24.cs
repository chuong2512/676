public class EquipCard_E_Shoes_24 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_24(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_OnDodgeBuffEffect, OnDodgeBuffEffect);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_OnDodgeBuffEffect, OnDodgeBuffEffect);
	}

	private void OnDodgeBuffEffect(EventData data)
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DefenceToAttack(Singleton<GameManager>.Instance.Player, 1));
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
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
