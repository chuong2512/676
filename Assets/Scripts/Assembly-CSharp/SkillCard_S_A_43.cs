using System;
using System.Collections;

public class SkillCard_S_A_43 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_43(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		Singleton<GameManager>.Instance.StartCoroutine(WaitDrawCard_IE(player, handler));
	}

	private IEnumerator WaitDrawCard_IE(Player player, Action handler)
	{
		while (BattleUI.isDrawingSupCard || BattleUI.IsDrawingMainCard)
		{
			yield return null;
		}
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		int mainHandCardAmount = player.PlayerBattleInfo.MainHandCardAmount;
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.GetBuff(new Buff_Cover(player, mainHandCardAmount));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerBattleInfo.MainHandCardAmount);
	}
}
