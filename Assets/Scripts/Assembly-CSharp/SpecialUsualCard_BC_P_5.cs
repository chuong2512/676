using System;
using System.Collections;

public class SpecialUsualCard_BC_P_5 : SpecialUsualCard
{
	private int mainHandAmount;

	private int supHandAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_5(UsualCardAttr usualCardAttr)
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
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawingCard_IE(player, isMain, handler));
	}

	private IEnumerator WaitDrawingCard_IE(Player player, bool isMain, Action handler)
	{
		while (BattleUI.isDrawingSupCard || BattleUI.IsDrawingMainCard)
		{
			yield return null;
		}
		UsualCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, isMain, handler);
		});
	}

	private void Effect(Player player, bool isMain, Action handler)
	{
		mainHandAmount = player.PlayerBattleInfo.MainHandCardAmount;
		supHandAmount = player.PlayerBattleInfo.SupHandCardAmount;
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.PlayerBattleInfo.ClearSupHandCards(null);
		if (mainHandAmount > 0)
		{
			player.PlayerBattleInfo.TryDrawMainHandCards(mainHandAmount);
		}
		if (supHandAmount > 0)
		{
			player.PlayerBattleInfo.TryDrawSupHandCards(supHandAmount);
		}
		handler?.Invoke();
	}
}
