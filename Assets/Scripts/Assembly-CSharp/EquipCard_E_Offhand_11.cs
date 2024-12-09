public class EquipCard_E_Offhand_11 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerRoundEnd);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerRoundEnd);
	}

	private void OnPlayerRoundEnd(EventData data)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Defence))
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Baptism(Singleton<GameManager>.Instance.Player, 4));
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
