public class PowerBlessing : BaseGift
{
	public override GiftName Name => GiftName.PowerBlessing;

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家力量祝福生效：获得3点力量");
		}
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, 3));
	}
}
