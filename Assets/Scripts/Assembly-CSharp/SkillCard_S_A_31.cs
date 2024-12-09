using System;
using System.Collections;

public class SkillCard_S_A_31 : SkillCard_Archer
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_31(SkillCardAttr skillCardAttr)
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
		player.PlayerBattleInfo.ClearSupHandCards(null);
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.GetBuff(new Buff_ShadowEscape(player, 2));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
