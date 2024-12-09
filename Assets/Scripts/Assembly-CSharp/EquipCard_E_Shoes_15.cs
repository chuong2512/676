public class EquipCard_E_Shoes_15 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_15(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayRound);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayRound);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.Player.PlayerAttr.IsWillTakeFlameDevourDmg)
		{
			EquipmentCard.EquipEffectAtkEntity(Singleton<GameManager>.Instance.Player, 2, isAbsDmg: true);
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
