using System;
using System.Collections;

public class SkillCard_S_K_18 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_18(SkillCardAttr skillCardAttr)
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
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawMainCard_IE(player, handler));
	}

	private IEnumerator WaitDrawMainCard_IE(Player player, Action handler)
	{
		while (BattleUI.IsDrawingMainCard)
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
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：扔掉所有的主手牌，副手抽丢掉的牌数张牌， 并进入防御状态");
		}
		int mainHandCardAmount = player.PlayerBattleInfo.MainHandCardAmount;
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.PlayerBattleInfo.TryDrawSupHandCards(mainHandCardAmount);
		player.GetBuff(new Buff_Defence(player, 1));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
