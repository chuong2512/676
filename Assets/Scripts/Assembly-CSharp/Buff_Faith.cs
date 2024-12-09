public class Buff_Faith : BaseBuff
{
	private int faithAmount;

	public int FaithAmount => faithAmount;

	public override BuffType BuffType => BuffType.Buff_Faith;

	public Buff_Faith(EntityBase entityBase)
		: base(entityBase, 1)
	{
		faithAmount = 0;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public void ClearFaith()
	{
		faithAmount = 0;
		buffIconCtrl.UpdateBuff();
	}

	public void AddFaith(int value)
	{
		faithAmount += value;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
	}

	private void OnPlayerUseUsualCard(EventData data)
	{
		faithAmount++;
		buffIconCtrl.UpdateBuff();
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + faithAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return faithAmount;
	}
}
