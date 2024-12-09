public class Buff_BrokenArmor : BaseBuff
{
	public override BuffType BuffType => BuffType.Buff_BrokenArmor;

	public Buff_BrokenArmor(EntityBase entityBase, int round)
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
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的破甲buff层数获得提升，当前的buff层数剩余：" + base.BuffRemainRound);
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
