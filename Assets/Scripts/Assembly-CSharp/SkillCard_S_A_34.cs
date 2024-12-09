using System;
using System.Collections;

public class SkillCard_S_A_34 : SkillCard_Archer
{
	private int buffAmount;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public SkillCard_S_A_34(SkillCardAttr skillCardAttr)
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
		buffAmount = player.PlayerBattleInfo.MainHandCardAmount;
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		int playerCurrentCardAmount = player.PlayerBattleInfo.PlayerCurrentCardAmount;
		player.PlayerBattleInfo.ClearMainHandCards(null);
		player.PlayerBattleInfo.ClearSupHandCards(null);
		player.GetBuff(new Buff_ShadowEscape(player, 1));
		player.GetBuff(new Buff_ShootStrengthen(player, playerCurrentCardAmount));
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return skillCardAttr.DesKeyOnBattle.LocalizeText();
	}
}
