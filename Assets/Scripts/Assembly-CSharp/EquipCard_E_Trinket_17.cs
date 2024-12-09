public class EquipCard_E_Trinket_17 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_17(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
	}

	private void OnPlayerStoringForce(EventData data)
	{
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		if (playerAttr.SpecialAttr == 0)
		{
			playerAttr.AddSpecialAttr(playerAttr.DefenceAttr);
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
