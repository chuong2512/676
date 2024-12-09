using System;

public class UsualCard_BC_M_12 : UsualCard_Archer
{
	private int armorAddAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public UsualCard_BC_M_12(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler_ArrowComsume(5, isDrop: true);
		pointupHandler = new SelfUpHandler_ArrowComsume(5, isDrop: true);
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return usualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		armorAddAmount = player.PlayerAttr.SpecialAttr;
		UsualCard_Archer.ComsumeSpecialAttrNotUpdate(player, armorAddAmount);
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.PlayerAttr.AddArmor(armorAddAmount);
		UsualCard_Archer.ComsumeSpecialArrow(player, null, armorAddAmount, isDrop: true);
		handler?.Invoke();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return true;
	}
}
