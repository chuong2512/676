using System;
using System.Collections;

public class SkillCard_S_K_31 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_31(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawSupCard_IE(player, handler));
	}

	private IEnumerator WaitDrawSupCard_IE(Player player, Action handler)
	{
		while (BattleUI.isDrawingSupCard)
		{
			yield return null;
		}
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：丢弃所有副手牌，主手抽丢该的牌数张，并获得1点力量");
		}
		int supHandCardAmount = player.PlayerBattleInfo.SupHandCardAmount;
		player.PlayerBattleInfo.ClearSupHandCards(null);
		player.PlayerBattleInfo.TryDrawMainHandCards(supHandCardAmount);
		player.GetBuff(new Buff_Power(player, 1));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
