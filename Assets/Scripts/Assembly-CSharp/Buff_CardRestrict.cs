public class Buff_CardRestrict : BaseBuff
{
	private int cardUsed;

	public override BuffType BuffType => BuffType.Buff_CardRestrict;

	public Buff_CardRestrict(EntityBase entityBase)
		: base(entityBase, int.MaxValue)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(BaseBuff.LoadEffectConfig(isPlayer: false, BuffType), buffIconCtrl.transform, null, BuffEffect);
		buffIconCtrl.BuffEffectHint();
	}

	private void BuffEffect()
	{
		BaseBuff.AtkEntity(Singleton<GameManager>.Instance.Player, 5, isAbsDmg: true);
	}

	public override void UpdateRoundTurn()
	{
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
		cardUsed++;
		if (cardUsed == 5)
		{
			TakeEffect(entityBase);
			cardUsed = 0;
		}
		buffIconCtrl.UpdateBuff();
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + cardUsed + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return cardUsed;
	}
}
