public class Buff_FlameArmor : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_FlameArmor;

	public Buff_FlameArmor(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
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

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		entityBase.AddImmuneBuff(BuffType.Buff_Freeze);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		entityBase.RemoveImmuneBuff(BuffType.Buff_Freeze);
	}
}
