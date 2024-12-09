public class EquipCard_E_Shoes_8 : EquipCard_E_Shoes
{
	private EquipEffectIconCtrl _equipEffectIconCtrl;

	private int skillCastTime;

	public EquipCard_E_Shoes_8(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkillCard);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseSkillCard, OnPlayerUseSkillCard);
	}

	private void OnBattleStart(EventData data)
	{
		skillCastTime = 0;
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		_equipEffectIconCtrl = battleUI.AddEquipEffect(this, GetEquipEffectHint(), GetEquipEffectDes);
		_equipEffectIconCtrl.SetEffect();
	}

	private string GetEquipEffectHint()
	{
		return skillCastTime.ToString();
	}

	private string GetEquipEffectDes()
	{
		return string.Format(equipmentCardAttr.EquipEffectDesKey.LocalizeText(), GetEquipEffectHint());
	}

	private void OnPlayerRound(EventData data)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount != 0)
		{
			skillCastTime = 0;
			_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
		}
	}

	private void OnPlayerUseSkillCard(EventData data)
	{
		skillCastTime++;
		if (skillCastTime == 3)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(1);
			skillCastTime = 0;
			EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
		}
		_equipEffectIconCtrl.UpdateEquipHint(GetEquipEffectHint());
	}
}
