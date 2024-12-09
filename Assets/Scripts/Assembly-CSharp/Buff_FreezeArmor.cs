public class Buff_FreezeArmor : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_FreezeArmor;

	public Buff_FreezeArmor(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}
}
