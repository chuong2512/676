using System;
using System.Collections;

public class SpecialUsualCard_BC_P_19 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_19(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), AllPlayerHandCardAmount() - 1);
	}

	private int AllPlayerHandCardAmount()
	{
		return Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PlayerCurrentCardAmount;
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawCard_IE(player, isMain, handler));
	}

	private IEnumerator WaitDrawCard_IE(Player player, bool isMain, Action handler)
	{
		while (BattleUI.IsDrawingMainCard || BattleUI.isDrawingSupCard)
		{
			yield return null;
		}
		int amount = AllPlayerHandCardAmount();
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, amount, handler);
		});
	}

	private void Effect(Player player, int amount, Action handler)
	{
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.PlayerBattleInfo.ClearSupHandCards(null);
		player.GetBuff(new Buff_HoldPosition(player, amount));
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：弃掉所有的手牌并且获得{amount}层的招架buff");
		}
		handler?.Invoke();
	}
}
