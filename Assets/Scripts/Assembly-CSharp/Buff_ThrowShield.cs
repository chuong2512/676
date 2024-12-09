public class Buff_ThrowShield : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_ThrowShield;

	public Buff_ThrowShield(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else if (buffRemainRound == (float)base.BuffRemainRound)
		{
			buffIconCtrl.UpdateBuff();
		}
	}

	public override string GetBuffHint()
	{
		return string.Empty;
	}

	public override int GetBuffHinAmount()
	{
		return 0;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		entityBase.AddImmuneBuff(BuffType.Buff_Defence);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		entityBase.RemoveImmuneBuff(BuffType.Buff_Defence);
	}
}
