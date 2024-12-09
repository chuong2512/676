public class Buff_FancyOrReal : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_FancyOrReal;

	public Buff_FancyOrReal(EntityBase entityBase, int round)
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
