using System;

public class SpecialUsualCard_BC_P_37 : SpecialUsualCard
{
	private static int RoundTime;

	private static bool isEverCal;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_37(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), 3 * (1 + RoundTime));
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
		player.PlayerAttr.AddArmor(3 * (1 + RoundTime));
		handler?.Invoke();
	}

	protected override void OnEquiped()
	{
		base.OnEquiped();
		EventManager.RegisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	protected override void OnReleased()
	{
		base.OnReleased();
		EventManager.UnregisterEvent(EventEnum.E_OnBattleStart, OnBattleStart);
		EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void OnPlayerEndRound(EventData data)
	{
		if (!isEverCal)
		{
			PlayerBattleInfo playerBattleInfo = Singleton<GameManager>.Instance.Player.PlayerBattleInfo;
			if (playerBattleInfo.EnoughMainCards(base.CardCode, 1) || playerBattleInfo.EnoughSupCards(base.CardCode, 1))
			{
				isEverCal = true;
				RoundTime++;
				EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
			}
		}
	}

	private void OnBattleStart(EventData data)
	{
		RoundTime = 0;
		EventManager.BroadcastEvent(EventEnum.E_CardDescriptionUpdate, null);
	}

	private void OnPlayerRound(EventData data)
	{
		isEverCal = false;
	}
}
