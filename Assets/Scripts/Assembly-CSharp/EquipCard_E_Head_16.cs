public class EquipCard_E_Head_16 : EquipCard_E_Head
{
	public EquipCard_E_Head_16(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		player.AddImmuneBuff(BuffType.Buff_Shocked);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		player.RemoveImmuneBuff(BuffType.Buff_Shocked);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
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
