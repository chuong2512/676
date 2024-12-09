public class AgileBlessing : BaseGift
{
	public override GiftName Name => GiftName.AgileBlessing;

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家迅捷祝福生效：获得2层迅捷buff");
		}
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Agile(Singleton<GameManager>.Instance.Player, 2));
	}
}
