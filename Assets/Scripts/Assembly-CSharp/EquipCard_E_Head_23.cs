public class EquipCard_E_Head_23 : EquipCard_E_Head
{
	public EquipCard_E_Head_23(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseASkill);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseASkill);
	}

	private void OnPlayerUseASkill(EventData data)
	{
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Cover(Singleton<GameManager>.Instance.Player, 1));
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
