public class Buff_ShootStrengthen : BaseBuff
{
	private int strengthenAmount;

	public int StrengthenAmount => strengthenAmount;

	public override BuffType BuffType => BuffType.Buff_ShootStrengthen;

	public Buff_ShootStrengthen(EntityBase entityBase, int strengthenAmount)
		: base(entityBase, int.MaxValue)
	{
		this.strengthenAmount = strengthenAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		entityBase.RemoveBuff(this);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddAtkDmgOnBattle(strengthenAmount);
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		int num = ((Buff_ShootStrengthen)baseBuff).StrengthenAmount;
		strengthenAmount += num;
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddAtkDmgOnBattle(num);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		Singleton<GameManager>.Instance.Player.PlayerAttr.ReduceAtkDmgOnBattle(strengthenAmount);
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + strengthenAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return strengthenAmount;
	}
}
