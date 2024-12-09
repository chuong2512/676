public class EquipCard_E_Armor_26 : EquipCard_E_Armor
{
	private EquipEffectIconCtrl _effectIconCtrl;

	private int round;

	public EquipCard_E_Armor_26(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void OnPlayerRound(EventData data)
	{
		round++;
		if (round == 3)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShadowEscape(Singleton<GameManager>.Instance.Player, 1));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			round = 0;
		}
		_effectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}

	private void OnBattleStart(EventData data)
	{
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_effectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_effectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return round.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), round);
	}
}
