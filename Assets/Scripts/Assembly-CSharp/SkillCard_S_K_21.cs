using System;

public class SkillCard_S_K_21 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_21(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SelfDownHandler();
		pointupHandler = new SelfUpHandler();
	}

	protected override bool IsSatifySpecialStatus(Player player)
	{
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
		int num = RealArmor(player);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：消耗所有的信仰({num}),转化称为护甲({num})");
		}
		player.PlayerAttr.AddArmor(num);
		handler?.Invoke();
	}

	private int RealArmor(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return player.PlayerAttr.SpecialAttr + IntData;
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int specialAttr = player.PlayerAttr.SpecialAttr;
		int num = RealArmor(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), player.PlayerAttr.SpecialAttr, BaseCard.GetValueColor(specialAttr, num), num);
	}
}
