using System;
using UnityEngine;

public class SpecialUsualCard_BC_P_39 : SpecialUsualCard
{
	private int armorAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_39(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		float value = UnityEngine.Random.value;
		armorAmount = Mathf.FloorToInt(4f * ((value < 0.33f) ? 1f : ((value < 0.66f) ? 0.5f : 2f)));
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
