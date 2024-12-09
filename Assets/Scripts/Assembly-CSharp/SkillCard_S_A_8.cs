using System;
using System.Collections;
using System.Collections.Generic;

public class SkillCard_S_A_8 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_8(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawSupCard_IE(player, handler));
	}

	private IEnumerator WaitDrawSupCard_IE(Player player, Action handler)
	{
		while (BattleUI.isDrawingSupCard)
		{
			yield return null;
		}
		FinalEffect(player, handler);
	}

	private void FinalEffect(Player player, Action handler)
	{
		if (player.PlayerBattleInfo.SupHandCardAmount <= 0)
		{
			EffectHint(player, handler);
			return;
		}
		(SingletonDontDestroy<UIManager>.Instance.ShowView("ChooseCardUI") as ChooseCardUI).ShowChooseCard("CHOOSECARDTODROP".LocalizeText(), player.PlayerBattleInfo.SupHandCards, 1, isMustEqualLimit: true, delegate(List<string> cardsChoose)
		{
			OnConfirm(cardsChoose);
			EffectHint(player, handler);
		});
	}

	private void OnConfirm(List<string> cardsChoose)
	{
		Singleton<GameManager>.Instance.Player.PlayerBattleInfo.DropSuphandCard(cardsChoose[0]);
	}

	private void EffectHint(Player player, Action handler)
	{
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		player.GetBuff(new Buff_Dodge(player, 1));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
