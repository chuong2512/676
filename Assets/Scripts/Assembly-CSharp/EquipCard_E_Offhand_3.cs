public class EquipCard_E_Offhand_3 : EquipCard_E_Offhand
{
	private int blockCount;

	private EquipEffectIconCtrl _equipEffectIconCtrl;

	public EquipCard_E_Offhand_3(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerBlockDmg, OnPlayerBlockDmg);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnPlayerBlockDmg(EventData data)
	{
		blockCount++;
		if (blockCount == 5)
		{
			Player player = Singleton<GameManager>.Instance.Player;
			player.GetBuff(new Buff_Power(player, 1));
			blockCount = 0;
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}

	private void OnBattleStart(EventData data)
	{
		blockCount = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return blockCount.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}
}
