using System;

public class SpecialUsualCard_BC_P_12 : SpecialUsualCard
{
	private static int CardUseTimeInBattle;

	public override int ApCost => specialUsualCardAttr.ApCost + CardUseTimeInBattle;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_12(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), CardUseTimeInBattle);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.PlayerAttr.AddArmor(6);
		handler?.Invoke();
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：自身获得6点护甲");
		}
		CardUseTimeInBattle++;
		EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	private void OnBattleStart(EventData data)
	{
		CardUseTimeInBattle = 0;
		EventManager.BroadcastEvent(EventEnum.E_UpdateApAmount, null);
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void OnBattleEnd(EventData data)
	{
		CardUseTimeInBattle = 0;
	}
}
