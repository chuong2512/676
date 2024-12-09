using System;
using UnityEngine;

public abstract class SkillCard : BaseCard
{
	protected SkillCardAttr skillCardAttr;

	public override CardType CardType => CardType.Skill;

	public SkillCardAttr SkillCardAttr => skillCardAttr;

	public int ApCost => skillCardAttr.ApCost;

	public int SpecialAttrCost
	{
		get
		{
			if (skillCardAttr.SpecialAttrCost < 0)
			{
				return Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr;
			}
			return skillCardAttr.SpecialAttrCost;
		}
	}

	public string SupHandCardCode => skillCardAttr.SupHandCardCode;

	public int SupHandCardConsumeAmount => skillCardAttr.SupHandCardConsumeAmount;

	public string MainHandCardCode => skillCardAttr.MainHandCardCode;

	public int MainHandCardConsumeAmount => skillCardAttr.MainHandCardConsumeAmount;

	protected BaseEffectConfig EffectConfig => GetEffectConfig();

	protected abstract PointDownHandler pointdownHandler { get; }

	protected abstract PointUpHandler pointupHandler { get; }

	public abstract bool IsWillBreakDefence { get; }

	public SkillCard(SkillCardAttr skillCardAttr)
		: base(skillCardAttr)
	{
		this.skillCardAttr = skillCardAttr;
	}

	protected virtual BaseEffectConfig GetEffectConfig()
	{
		if (!skillCardAttr.EffectConfigName.IsNullOrEmpty())
		{
			return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(skillCardAttr.EffectConfigName, "EffectConfigScriObj");
		}
		return null;
	}

	protected static void HandleEffect(BaseEffectConfig config, Transform[] targets, Action handler)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(config, null, targets, handler);
	}

	public void HandleShowCastHint(Transform target)
	{
		pointdownHandler.HandleShowCastHint(target);
	}

	public void HandleEndCastHint()
	{
		pointdownHandler.HandleEndCastHit();
	}

	public void HandlePointDown(Vector3 rect)
	{
		pointdownHandler.HandleDown(rect);
	}

	public void OnPointDown()
	{
		pointdownHandler.OnPointDown();
	}

	public void HandlePointUp(Vector3 rect, Action tryUseAction, Action tryCancelAction)
	{
		pointupHandler.PointUp(rect, tryUseAction, tryCancelAction);
	}

	protected abstract bool IsSatifySpecialStatus(Player player);

	public virtual bool IsCanCast(Player player, out string failResult)
	{
		bool flag = IsApAmountSatisfied(player, out failResult);
		if (!flag)
		{
			return flag;
		}
		flag = CheckSilenceBuff(out failResult);
		if (!flag)
		{
			return flag;
		}
		flag = IsHandCardSatisfied(out failResult);
		if (!flag)
		{
			return flag;
		}
		flag = IsSpecialAttrSatisfied(player, out failResult);
		if (!flag)
		{
			return flag;
		}
		failResult = string.Empty;
		return true;
	}

	private bool IsApAmountSatisfied(Player player, out string failResult)
	{
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Agile) && player.PlayerAttr.ApAmount < skillCardAttr.ApCost)
		{
			failResult = "LackOfAp".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	private bool CheckSilenceBuff(out string failResult)
	{
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Silence).IsNull())
		{
			failResult = "SilenceBuffKey".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	private bool IsHandCardSatisfied(out string failResult)
	{
		bool flag = true;
		if (skillCardAttr.MainHandCardConsumeAmount > 0)
		{
			flag = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EnoughMainCards(skillCardAttr.MainHandCardCode, skillCardAttr.MainHandCardConsumeAmount);
		}
		bool flag2 = true;
		if (skillCardAttr.SupHandCardConsumeAmount > 0)
		{
			flag2 = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EnoughSupCards(skillCardAttr.SupHandCardCode, skillCardAttr.SupHandCardConsumeAmount);
		}
		if (!flag || !flag2)
		{
			failResult = "LackOfBaseCard".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	protected virtual bool IsSpecialAttrSatisfied(Player player, out string failResult)
	{
		if (player.PlayerAttr.SpecialAttr < skillCardAttr.SpecialAttrCost)
		{
			failResult = "LackOfFaith".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	public abstract void SkillCardEffect(Player player, Action handler);

	public override string GetOnBattleDes(Player player, bool isMain)
	{
		return SkillOnBattleDes(player);
	}

	protected abstract string SkillOnBattleDes(Player player);
}
