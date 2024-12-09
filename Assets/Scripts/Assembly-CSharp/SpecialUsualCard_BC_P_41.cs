using System;

public class SpecialUsualCard_BC_P_41 : SpecialUsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_41(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return (isMain ? specialUsualCardAttr.DesKeyOnBattle : specialUsualCardAttr.DesKeyOnBattleSupHand).LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		UsualCard.HandleEffect(isMain ? base.EffectConfig : base.SupEffectConfig, null, delegate
		{
			Effect(player, isMain, handler);
		});
	}

	private void Effect(Player player, bool isMain, Action handler)
	{
		if (isMain)
		{
			player.PlayerBattleInfo.TryDrawMainHandCards(8);
		}
		else
		{
			player.PlayerBattleInfo.TryDrawSupHandCards(8);
		}
		handler?.Invoke();
	}
}
