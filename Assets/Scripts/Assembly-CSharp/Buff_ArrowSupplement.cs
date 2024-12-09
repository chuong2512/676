public class Buff_ArrowSupplement : BaseBuff
{
	private int arrowRecoveryAmount;

	public int ArrowRecoveryAmount => arrowRecoveryAmount;

	public override BuffType BuffType => BuffType.Buff_ArrowSupplement;

	public Buff_ArrowSupplement(EntityBase entityBase, int arrowRecoveryAmount)
		: base(entityBase, int.MaxValue)
	{
		this.arrowRecoveryAmount = arrowRecoveryAmount;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		buffIconCtrl.BuffEffectHint();
		Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(arrowRecoveryAmount);
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		arrowRecoveryAmount += ((Buff_ArrowSupplement)baseBuff).ArrowRecoveryAmount;
		buffIconCtrl.UpdateBuff();
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void OnPlayerRound(EventData data)
	{
		TakeEffect(entityBase);
	}

	public override string GetBuffHint()
	{
		return string.Format("{0}{1}</color>", "<color=#e9e9e9ff>", arrowRecoveryAmount);
	}

	public override int GetBuffHinAmount()
	{
		return arrowRecoveryAmount;
	}
}
