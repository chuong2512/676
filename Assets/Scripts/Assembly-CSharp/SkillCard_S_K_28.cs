using System;
using UnityEngine;

public class SkillCard_S_K_28 : SkillCard
{
	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => true;

	public SkillCard_S_K_28(SkillCardAttr skillCardAttr)
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
		string atkDes;
		int num = RealDmg(player, out atkDes);
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent($"{base.CardName}效果触发：玩家获得1回合鞭策buff，鞭策能对攻击者造成{num}的伤害({atkDes})");
		}
		player.GetBuff(new Buff_BianCe(player, 1, num));
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"单倍武器伤害({player.PlayerAttr.Armor})", IntData, out var pwdBuff, out var rate);
		return Mathf.FloorToInt((float)(player.PlayerAttr.AtkDmg + IntData + pwdBuff) * (1f + rate));
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return Mathf.FloorToInt((float)(player.PlayerAttr.AtkDmg + IntData + player.PowerBuff) * (1f + player.PowUpRate()));
	}

	protected override string SkillOnBattleDes(Player player)
	{
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), RealDmg(player));
	}
}
