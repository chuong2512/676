using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCard
{
	protected const string LackOfApKey = "LackOfAp";

	protected const string SilenceBuffKey = "SilenceBuffKey";

	protected const string LackOfBaseCardKey = "LackOfBaseCard";

	protected const string LackOfFaithKey = "LackOfFaith";

	protected const string LackOfArrowKey = "LackOfArrow";

	protected const string LackOfDefenceBuffKey = "LackOfDefenceBuff";

	protected const string PlayerImmueDefenceBuffKey = "PlayerImmueDefenceBuff";

	protected const string NotLastHandCardKey = "NotLastHandCardKey";

	protected const string NormalColor = "<color=#ffffffff>";

	protected const string ReinforcedColor = "<color=#27dd34ff>";

	protected const string WeakenedColor = "<color=#ed2024ff>";

	protected const string ClearColor = "<color=#ffffff00>";

	protected BaseCardAttr baseCardAttr;

	private const string RealDamageAtkDesFormat = "基础伤害：{0}, 额外的战斗效果带来的伤害提升值：{1}, 额外的力量buff带来的伤害提升值{2},伤害力量的提升率：{3}";

	public abstract CardType CardType { get; }

	public string CardCode => baseCardAttr.CardCode;

	public string CardName => baseCardAttr.NameKey.LocalizeText();

	public string CardNormalDes => baseCardAttr.DesKeyNormal.LocalizeText();

	public abstract string GetOnBattleDes(Player player, bool isMain);

	public List<KeyValuePair> GetKeyDescription()
	{
		return baseCardAttr.AllKeys;
	}

	public BaseCard(BaseCardAttr baseCardAttr)
	{
		this.baseCardAttr = baseCardAttr;
	}

	protected static string GetValueColor(int baseValue, int realValue)
	{
		if (realValue <= baseValue)
		{
			if (realValue >= baseValue)
			{
				return "<color=#ffffffff>";
			}
			return "<color=#ed2024ff>";
		}
		return "<color=#27dd34ff>";
	}

	protected static string GetRealDamageAtkDes(Player player, string baseDmgStr, int extraPwdUp, out int pwdBuff, out float rate)
	{
		pwdBuff = player.PowerBuff;
		rate = player.PowUpRate();
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(player, BuffType.Buff_Freeze))
		{
			return $"基础伤害：{baseDmgStr}, 额外的战斗效果带来的伤害提升值：{extraPwdUp}, 额外的力量buff带来的伤害提升值{pwdBuff},伤害力量的提升率：{rate}";
		}
		return player.EntityName + "因为冻结buff伤害被降低至1";
	}

	protected static int GetRealDamage(Player player, int baseDmg, int extraPwdUp, int pwdBuff, float rate)
	{
		int num = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + extraPwdUp + pwdBuff) * (1f + rate)), 0, int.MaxValue);
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

	protected static int GetRealDamage(Player player, int baseDmg, int extraPwdUp)
	{
		int powerBuff = player.PowerBuff;
		float num = player.PowUpRate();
		int num2 = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + extraPwdUp + powerBuff) * (1f + num)), 0, int.MaxValue);
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

	protected static bool IsEntityCanDodgeAttack(EntityBase en, out BaseBuff buff)
	{
		buff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(en, BuffType.Buff_Dodge);
		return !buff.IsNull();
	}
}
