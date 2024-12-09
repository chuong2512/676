public class Buff_Wound : BaseBuff
{
	private int woundAmount;

	public int WoundAmount => woundAmount;

	public override BuffType BuffType => BuffType.Buff_Wound;

	public Buff_Wound(EntityBase entityBase, int woundAmount)
		: base(entityBase, int.MaxValue)
	{
		this.woundAmount = woundAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		entityBase.TakeDamage(woundAmount, null, isAbsDmg: true);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		woundAmount += ((Buff_Wound)baseBuff).WoundAmount;
		buffIconCtrl.UpdateBuff();
	}

	public override string GetBuffHint()
	{
		return "<color=#ec2125ff>" + WoundAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return woundAmount;
	}
}
