public class EquipCard_E_Trinket_13 : EquipCard_E_Trinket
{
	public EquipCard_E_Trinket_13(EquipmentCardAttr equipmentCardAttr)
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
		int mainHandCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount;
		if (mainHandCardAmount > 0)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShootStrengthen(Singleton<GameManager>.Instance.Player, mainHandCardAmount));
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
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), Singleton<GameManager>.Instance.Player.PlayerAttr.AtkDmg);
	}
}
