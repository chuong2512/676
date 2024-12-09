public class Buff_JianShou : BaseBuff
{
	private int blockAddAmount;

	public override BuffType BuffType => BuffType.Buff_JianShou;

	public Buff_JianShou(EntityBase entityBase, int round)
		: base(entityBase, round)
	{
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddExtraBlock(baseBuff.BuffRemainRound);
		blockAddAmount += baseBuff.BuffRemainRound;
		base.HandleSameBuffAdd(baseBuff);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的坚守buff的效果获得提升，当前获得的格挡值的提升为：" + blockAddAmount);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#27dd34ff>" + blockAddAmount + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return blockAddAmount;
	}

	public override void HandleNewBuffAdd()
	{
		blockAddAmount = base.BuffRemainRound;
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddExtraBlock(blockAddAmount);
		base.HandleNewBuffAdd();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的坚守buff效果触发，获得额外的格挡值：" + blockAddAmount);
		}
	}

	public override void HandleBuffRemove()
	{
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).ReduceExtraBlock(blockAddAmount);
		base.HandleBuffRemove();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的坚守buff失效，移除额外的格挡值：" + blockAddAmount);
		}
	}
}
