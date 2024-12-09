public class EquipCard_E_Armor_14 : EquipCard_E_Armor
{
	public EquipCard_E_Armor_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DamageRestrik(Singleton<GameManager>.Instance.Player, 4));
		(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes).SetEffect();
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}

	private string GetEquipEffectHint()
	{
		return string.Empty;
	}

	private string GetEquipEffectDes()
	{
		return equipmentCardAttr.EquipEffectDesKey.LocalizeText();
	}

	private void OnPlayerRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.Player.PlayerAttr.IsWillTakeFlameDevourDmg)
		{
			EquipmentCard.EquipEffectAtkEntity(Singleton<GameManager>.Instance.Player, 2, isAbsDmg: true);
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
	}
}
