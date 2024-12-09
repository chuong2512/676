using System;

public class SpecialUsualCard_BC_P_50 : SpecialUsualCard
{
	private int armorAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_50(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return string.Format(specialUsualCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerBattleInfo.PlayerCurrentCardAmount);
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		armorAmount = player.PlayerBattleInfo.PlayerCurrentCardAmount + 1;
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.PlayerAttr.AddArmor(armorAmount);
		handler?.Invoke();
	}
}
