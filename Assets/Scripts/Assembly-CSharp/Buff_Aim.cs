public class Buff_Aim : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Aim;

	public Buff_Aim(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public void ReduceAimBuffAmount(int amount)
	{
		buffRemainRound -= amount;
		if (buffRemainRound <= 0f)
		{
			entityBase.RemoveBuff(this);
		}
		else
		{
			buffIconCtrl.UpdateBuff();
		}
		EventManager.BroadcastEvent(EventEnum.E_ShootArrowWhenAim, new SimpleEventData
		{
			intValue = amount
		});
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		BroadcastAddEvent(baseBuff);
	}

	public override void HandleNewBuffAdd()
	{
		base.HandleNewBuffAdd();
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		((ArcherArrowCtrl)battleUI.PlayerDefenceAttrCtrl).HighlightAnimHint();
		if (entityBase is Player)
		{
			battleUI.SetPlayerSpecialPossHeadPortrait();
		}
		BroadcastAddEvent(this);
	}

	public override void HandleBuffRemove()
	{
		base.HandleBuffRemove();
		BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
		((ArcherArrowCtrl)battleUI.PlayerDefenceAttrCtrl).CancelHighlightAnimHint();
		if (entityBase is Player)
		{
			battleUI.SetPlayerNormalHeapPortrait();
		}
	}

	public override string GetBuffHint()
	{
		return string.Format("{0}{1}</color>", "<color=#e9e9e9ff>", base.BuffRemainRound);
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}

	private static void BroadcastAddEvent(BaseBuff buff)
	{
		EventManager.BroadcastEvent(EventEnum.E_GetAimBuff, new SimpleEventData
		{
			intValue = buff.BuffRemainRound
		});
	}
}
