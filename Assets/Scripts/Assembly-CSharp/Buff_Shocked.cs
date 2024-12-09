public class Buff_Shocked : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_Shocked;

	public Buff_Shocked(EntityBase entityBase, int round)
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

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		base.HandleSameBuffAdd(baseBuff);
		buffRemainRound += baseBuff.ExactlyBuffRemainRound;
		buffIconCtrl.UpdateBuff();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的震荡buff层数提升，当前的buff的层数为：" + base.BuffRemainRound);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + base.BuffRemainRound + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return base.BuffRemainRound;
	}
}
