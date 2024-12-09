using System;
using UnityEngine;

public class UsualCard_BC_O_1 : UsualCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public UsualCard_BC_O_1(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatisfySpecialStatus(Player player)
	{
		return Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Defence);
	}

	public override bool IsCanCast(Player player, bool isMain, out int finalApValue, out string failResult)
	{
		player.PlayerUseCardApReduce(this, out var amount);
		int num = Mathf.Max(usualCardAttr.ApCost - amount, 0);
		if (num > player.PlayerAttr.ApAmount)
		{
			failResult = "LackOfAp".LocalizeText();
			finalApValue = num;
			return false;
		}
		if (!IsSatisfySpecialStatus(player))
		{
			failResult = "LackOfDefenceBuff".LocalizeText();
			finalApValue = num;
			return false;
		}
		failResult = string.Empty;
		finalApValue = num;
		return true;
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
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(base.CardName + "效果触发：玩家获得1层格挡反击的buff");
		}
		player.GetBuff(new Buff_DefenceRestrik(player, 1));
		handler?.Invoke();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return usualCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
