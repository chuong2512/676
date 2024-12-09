public abstract class BaseBattleEffect
{
	public abstract BattleEffectType BattleEffectType { get; }

	public virtual void OnAddEffect()
	{
	}

	public virtual void OnRemoveEffect()
	{
	}

	public virtual void TakeEffect(BattleEffectData data)
	{
	}

	public virtual void TakeEffect(BattleEffectData data, out int IntData)
	{
		IntData = 0;
	}

	public virtual void TakeEffect(BattleEffectData data, out float FloatData)
	{
		FloatData = 0f;
	}

	public virtual void TakeEffect(BattleEffectData data, out int IntData, out string source)
	{
		IntData = 0;
		source = string.Empty;
	}

	protected static void BattleEffectAtkEntity(EntityBase entityBase, int amount, bool isAbsDmg)
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
