public class Buff_Stable : BaseBuff
{
	private int blockAdd;

	public int BlockAdd => blockAdd;

	public override BuffType BuffType => BuffType.Buff_Stable;

	public Buff_Stable(EntityBase entityBase, int blockAdd)
		: base(entityBase, 1)
	{
		this.blockAdd = blockAdd;
	}

	public override void TakeEffect(EntityBase entityBase)
	{
	}

	public override void UpdateRoundTurn()
	{
	}

	public override void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		int num = ((Buff_Stable)baseBuff).BlockAdd;
		blockAdd += num;
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddBlock(num);
		base.HandleSameBuffAdd(baseBuff);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的稳固buff效果提升，当前回合额外的格挡值为：" + blockAdd);
		}
	}

	public override string GetBuffHint()
	{
		return "<color=#e9e9e9ff>" + blockAdd + "</color>";
	}

	public override int GetBuffHinAmount()
	{
		return blockAdd;
	}

	public override void HandleNewBuffAdd()
	{
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddBlock(blockAdd);
		base.HandleNewBuffAdd();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的稳固buff效果触发，获得额外的格挡值：" + blockAdd);
		}
	}

	public override void HandleBuffRemove()
	{
		((KnightPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).ReduceBlock(blockAdd);
		base.HandleBuffRemove();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(entityBase.EntityName + "的稳固buff失效，移除额外的格挡值：" + blockAdd);
		}
	}
}
