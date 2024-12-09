public class EquipCard_E_Head_17 : EquipCard_E_Head
{
	private bool isPlayerUseSkillThisRound;

	public EquipCard_E_Head_17(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerRoundEnd);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerRoundEnd);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkill);
	}

	private void OnPlayerRoundEnd(EventData data)
	{
		if (!isPlayerUseSkillThisRound)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Agile(Singleton<GameManager>.Instance.Player, 1));
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
			isPlayerUseSkillThisRound = false;
		}
	}

	private void OnPlayerUseSkill(EventData data)
	{
		isPlayerUseSkillThisRound = true;
	}

	private void OnBattleStart(EventData data)
	{
		isPlayerUseSkillThisRound = false;
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
