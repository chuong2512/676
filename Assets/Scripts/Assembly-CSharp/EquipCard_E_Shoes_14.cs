public class EquipCard_E_Shoes_14 : EquipCard_E_Shoes
{
	public EquipCard_E_Shoes_14(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
	}

	protected override void OnEquip(Player player)
	{
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	protected override void OnRelease(Player player)
	{
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
	}

	private void OnBattleStart(EventData data)
	{
		for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
		{
			EnemyBase enemyBase = Singleton<EnemyController>.Instance.AllEnemies[i];
			enemyBase.GetBuff(new Buff_DeadPoison(enemyBase, 1));
		}
		EquipmentCard.ShowPlayerEquipEffectHint(base.CardCode);
	}
}
