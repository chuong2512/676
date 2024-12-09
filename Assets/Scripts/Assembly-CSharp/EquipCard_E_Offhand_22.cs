public class EquipCard_E_Offhand_22 : EquipCard_E_Offhand
{
	public EquipCard_E_Offhand_22(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_EnemyEndRound, OnEnemyEndRound);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_EnemyEndRound, OnEnemyEndRound);
	}

	private void OnEnemyEndRound(EventData data)
	{
		int specialAttr = Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr;
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShootStrengthen(Singleton<GameManager>.Instance.Player, specialAttr));
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
