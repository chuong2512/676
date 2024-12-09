using System;
using UnityEngine;

public class SkillCard_S_K_16 : SkillCard
{
	private const string ArmorBrokenEffectName = "S_K_16_Armor_EffectConfig";

	private int realDmg;

	private string atkDes;

	private bool isTargetArmorBroken;

	protected override PointDownHandler pointdownHandler { get; }

	protected override PointUpHandler pointupHandler { get; }

	public override bool IsWillBreakDefence => false;

	public SkillCard_S_K_16(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		pointdownHandler = new SingleEnemyDownHandler();
		pointupHandler = new SingleEnemyUpHandler();
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

	protected override BaseEffectConfig GetEffectConfig()
	{
		if (!skillCardAttr.EffectConfigName.IsNullOrEmpty())
		{
			return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(isTargetArmorBroken ? "S_K_16_Armor_EffectConfig" : skillCardAttr.EffectConfigName, "EffectConfigScriObj");
		}
		return null;
	}

	public override void SkillCardEffect(Player player, Action handler)
	{
		EnemyBase enTarget = Singleton<GameManager>.Instance.BattleSystem.EnemyPlayerChoose;
		realDmg = RealDmg(player, out atkDes);
		isTargetArmorBroken = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(enTarget, BuffType.Buff_BrokenArmor);
		SkillCard.HandleEffect(base.EffectConfig, new Transform[1] { enTarget.EnemyCtrl.transform }, delegate
		{
			Effect(player, enTarget, handler);
		});
	}

	private void Effect(Player player, EnemyBase enTarget, Action handler)
	{
		if (BaseCard.IsEntityCanDodgeAttack(enTarget, out var buff))
		{
			buff.TakeEffect(enTarget);
		}
		else
		{
			bool num = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(enTarget, BuffType.Buff_BrokenArmor);
			player.PlayerAtkEnemy(enTarget, realDmg, isTrueDmg: false);
			if (num)
			{
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes}),目标拥有破甲buff获得3点体力");
				}
				player.PlayerAttr.RecoveryApAmount(2);
			}
			else
			{
				GameReportUI gameReportUI2 = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI2 != null)
				{
					gameReportUI2.AddGameReportContent($"{base.CardName}效果触发：对{enTarget.EntityName}造成1次非真实的{realDmg}点伤害({atkDes})");
				}
			}
		}
		handler?.Invoke();
	}

	private int RealDmg(Player player, out string atkDes)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		atkDes = BaseCard.GetRealDamageAtkDes(player, $"当前格挡值({defenceAttr})", IntData, out var pwdBuff, out var rate);
		return BaseCard.GetRealDamage(player, defenceAttr, IntData, pwdBuff, rate);
	}

	private int RealDmg(Player player)
	{
		player.PlayerEffectContainer.TakeEffect(BattleEffectType.UponUsingSkillCardPowUp, (BattleEffectData)new SimpleEffectData
		{
			strData = base.CardCode
		}, out int IntData);
		return BaseCard.GetRealDamage(player, player.PlayerAttr.DefenceAttr, IntData);
	}

	protected override string SkillOnBattleDes(Player player)
	{
		int defenceAttr = player.PlayerAttr.DefenceAttr;
		int baseValue = defenceAttr;
		int num = RealDmg(player);
		return string.Format(skillCardAttr.DesKeyOnBattle.LocalizeText(), defenceAttr, BaseCard.GetValueColor(baseValue, num), num);
	}
}
