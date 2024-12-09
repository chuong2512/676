using System;

public class SkillCard_S_K_6 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_6(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
		return Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Defence);
	}

	public override bool IsCanCast(Player player, out string failResult)
	{
		bool flag = base.IsCanCast(player, out failResult);
		if (!flag)
		{
			return flag;
		}
		if (!IsSatifySpecialStatus(player))
		{
			failResult = "LackOfDefenceBuff".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		SkillCard.HandleEffect(base.EffectConfig, null, delegate
		{
			Effect(player, handler);
		});
	}

	private void Effect(Player player, Action handler)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：玩家获得当前格挡值{defenceAttr}的护甲");
		}
		player.PlayerAttr.AddArmor(defenceAttr);
		handler?.Invoke();
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), "<color=#ffffffff>", player.PlayerAttr.DefenceAttr);
	}
}
