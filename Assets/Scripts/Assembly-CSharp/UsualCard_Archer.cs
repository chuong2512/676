using UnityEngine;

public abstract class UsualCard_Archer : UsualCard
{
	public override bool IsWillBreakDefence => false;

	public UsualCard_Archer(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
	}

	public override bool IsCanCast(Player player, bool isMain, out int finalApValue, out string failResult)
	{
		bool flag = IsSatisfiedAp(player, out finalApValue, out failResult);
		if (!flag)
		{
			return flag;
		}
		return IsSatisfiedSpecialAttr(player, out failResult);
	}

	protected virtual bool IsSatisfiedSpecialAttr(Player player, out string failResult)
	{
		failResult = string.Empty;
		return true;
	}

	protected static bool CheckSpecialAttr(Player player, int needAmount, out string failResult)
	{
		if (player.PlayerAttr.SpecialAttr < needAmount)
		{
			failResult = "LackOfArrow".LocalizeText();
			return false;
		}
		failResult = string.Empty;
		return true;
	}

	protected static void ComsumeSpecialArrow(Player player, EntityBase[] targets, int comsumeAmount, bool isDrop)
	{
		ArcherPlayerAttr archerPlayerAttr = (ArcherPlayerAttr)player.PlayerAttr;
		for (int i = 0; i < comsumeAmount; i++)
		{
			archerPlayerAttr.ActiveSpecialArrow(targets);
		}
		archerPlayerAttr.ComsumeSpecialAttr(targets, comsumeAmount, isDrop);
	}

	protected static void ComsumeSpecialAttrNotUpdate(Player player, int comsumeAmount)
	{
		((ArcherPlayerAttr)player.PlayerAttr).ComsumeSpecialAttrNotUpdate(comsumeAmount);
	}

	protected static void ActiveArrowEffect(Player player, EntityBase[] targets)
	{
		((ArcherPlayerAttr)player.PlayerAttr).ActiveSpecialArrow(targets);
	}

	protected static int GetRealDamage_Arrow(Player player, int baseDmg, int extraPwdUp, int pwdBuff, float rate)
	{
		int num = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + extraPwdUp + pwdBuff) * (1f + rate)), 0, int.MaxValue);
		num *= ((!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Aim)) ? 1 : 2);
		if (num <= 1)
		{
			return num;
		}
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Freeze))
		{
			return num;
		}
		return 1;
	}

	protected static int GetRealDamage_Arrow(Player player, int baseDmg, int extraPwdUp)
	{
		int powerBuff = player.PowerBuff;
		float num = player.PowUpRate();
		int num2 = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + extraPwdUp + powerBuff) * (1f + num)), 0, int.MaxValue);
		num2 *= ((!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Aim)) ? 1 : 2);
		if (num2 <= 1)
		{
			return num2;
		}
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Freeze))
		{
			return num2;
		}
		return 1;
	}

	protected static void TryEffectShootRelatedEffect(Player player)
	{
		Buff_ShootStrengthen buff_ShootStrengthen;
		if ((buff_ShootStrengthen = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_ShootStrengthen) as Buff_ShootStrengthen) != null)
		{
			buff_ShootStrengthen.TakeEffect(player);
		}
		(Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(player, BuffType.Buff_Aim) as Buff_Aim)?.ReduceAimBuffAmount(1);
	}
}
