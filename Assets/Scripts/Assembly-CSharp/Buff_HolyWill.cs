public class Buff_HolyWill : BaseBuff
{
	private int specialAttrComsume;

	public override BuffType BuffType => BuffType.Buff_HolyWill;

	public Buff_HolyWill(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
		base.entityBase.GetBuff(new Buff_Baptism(Singleton<GameManager>.Instance.Player, specialAttrComsume));
	}

	public override void UpdateRoundTurn()
	{
		buffRemainRound -= 0.5f;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeSpecialAttr);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnPlayerComsumeSpecialAttr);
	}

	private void OnPlayerComsumeSpecialAttr(EventData eventData)
	{
		SimpleEventData simpleEventData;
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle && (simpleEventData = eventData as SimpleEventData) != null)
		{
			specialAttrComsume = simpleEventData.intValue;
			TakeEffect(entityBase);
		}
	}
}
