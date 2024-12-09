public class ProtectBlessing : BaseGift
{
	public override GiftName Name => GiftName.ProtectBlessing;

	protected override void Effect()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent("玩家守护祝福生效：获得2层神圣庇护");
		}
		Singleton<GameManager>.Instance.Player.GetBuff(new Buff_HolyProtect(Singleton<GameManager>.Instance.Player, 2));
	}
}
