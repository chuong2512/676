public class EquipCard_E_Trinket_12 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_12(EquipmentCardAttr equipmentCardAttr)
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
		int supHandCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardAmount;
		if (supHandCardAmount > 0)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Cover(Singleton<GameManager>.Instance.Player, supHandCardAmount));
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
