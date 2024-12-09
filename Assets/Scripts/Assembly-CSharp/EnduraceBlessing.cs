public class EnduraceBlessing : BaseGift
{
	public override GiftName Name => GiftName.EnduranceBlessing;

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家耐力祝福生效：获得额外的3点体力");
		}
		Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(3);
	}
}
