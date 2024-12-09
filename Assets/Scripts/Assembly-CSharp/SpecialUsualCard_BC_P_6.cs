using System;
using System.Collections;
using System.Collections.Generic;

public class SpecialUsualCard_BC_P_6 : SpecialUsualCard
{
	private bool isMainHand;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SpecialUsualCard_BC_P_6(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		if (!isMain)
		{
			return specialUsualCardAttr.DesKeyOnBattleSupHand.LocalizeText();
		}
		return specialUsualCardAttr.DesKeyOnBattle.LocalizeText();
	}

	public override void UsualCardEffect(Player player, bool isMain, Action handler)
	{
		Effect(player, isMain, handler);
	}

	private void Effect(Player player, bool isMain, Action handler)
	{
		isMainHand = isMain;
		if (isMain)
		{
			if (BattleUI.IsDrawingMainCard)
			{
				Singleton<GameManager>.Instance.StartCoroutine(WaitMainHandDrawCard_IE(player, isMain, handler));
			}
			else
			{
				FinalEffect(player, isMain, handler);
			}
		}
		else if (BattleUI.isDrawingSupCard)
		{
			Singleton<GameManager>.Instance.StartCoroutine(WaitSupHandDrawCard_IE(player, isMain, handler));
		}
		else
		{
			FinalEffect(player, isMain, handler);
		}
	}

	private IEnumerator WaitMainHandDrawCard_IE(Player player, bool isMain, Action handler)
	{
		while (BattleUI.IsDrawingMainCard)
		{
			yield return null;
		}
		FinalEffect(player, isMain, handler);
	}

	private IEnumerator WaitSupHandDrawCard_IE(Player player, bool isMain, Action handler)
	{
		while (BattleUI.isDrawingSupCard)
		{
			yield return null;
		}
		FinalEffect(player, isMain, handler);
	}

	private void FinalEffect(Player player, bool isMain, Action handler)
	{
		handler?.Invoke();
		if (isMain && player.PlayerBattleInfo.MainHandCardAmount > 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseCardUI") as ChooseCardUI).ShowChooseCard("chooseCardToDrop".LocalizeText(), player.PlayerBattleInfo.MainHandCards, 1, isMustEqualLimit: true, OnComfirm);
		}
		else if (!isMain && player.PlayerBattleInfo.SupHandCardAmount > 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseCardUI") as ChooseCardUI).ShowChooseCard("chooseCardToDrop".LocalizeText(), player.PlayerBattleInfo.SupHandCards, 1, isMustEqualLimit: true, OnComfirm);
		}
	}

	private void OnComfirm(List<string> cards)
	{
		if (!cards.IsNull() && cards.Count > 0)
		{
			UsualCard card = FactoryManager.GetUsualCard(cards[0]);
			if (isMainHand)
			{
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ComsumeMainHandCards(card.CardCode, isDrop: true);
			}
			else
			{
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ComsumeSupHandCards(card.CardCode, isDrop: true);
			}
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).PlayerDropACard(card.CardCode, isMainHand);
			UsualCard.HandleEffect(isMainHand ? base.EffectConfig : base.SupEffectConfig, null, delegate
			{
				GetActionPoint(card.ApCost);
			});
		}
	}

	private void GetActionPoint(int amount)
	{
		if (amount > 0)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(amount);
		}
	}
}
