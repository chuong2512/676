public class EquipCard_E_Head_11 : EquipCard_E_Head
{
	public EquipCard_E_Head_11(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnEnemyDead(EventData data)
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(5);
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
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
