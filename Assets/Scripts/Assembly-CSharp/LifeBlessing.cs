public class LifeBlessing : BaseGift
{
	public override GiftName Name => GiftName.LifeBlessing;

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家生命祝福生效：获得4层治愈buff");
		}
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Heal(Singleton<GameManager>.Instance.Player, 4));
	}
}
