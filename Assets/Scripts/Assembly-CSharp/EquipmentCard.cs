public abstract class EquipmentCard : BaseCard
{
	protected EquipmentCardAttr equipmentCardAttr;

	public override CardType CardType => CardType.Equipment;

	public EquipmentCardAttr EquipmentCardAttr => equipmentCardAttr;

	public SuitType SuitType => equipmentCardAttr.SuitType;

	public string ImageName => equipmentCardAttr.ImageName;

	public EquipmentCard(EquipmentCardAttr equipmentCardAttr)
		: base(equipmentCardAttr)
	{
		this.equipmentCardAttr = equipmentCardAttr;
	}

	public abstract void Equip(Player player);

	public abstract void Release(Player player);

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Empty;
	}

	protected static void ShowPlayerEquipEffectHint(string cardCode)
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("PlayerEquipEffectHintUI") as PlayerEquipEffectHintUI).AddHint(cardCode);
	}

	protected static void EquipEffectAtkEntity(EntityBase entityBase, int amount, bool isAbsDmg)
	{
		Buff_Dodge buff_Dodge;
		if ((buff_Dodge = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_Dodge) as Buff_Dodge) != null)
		{
			buff_Dodge.TakeEffect(entityBase);
		}
		else
		{
			entityBase.TakeDamage(amount, null, isAbsDmg);
		}
	}
}
