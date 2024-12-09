using System;
using UnityEngine;

public abstract class UsualCard : BaseCard
{
	protected UsualCardAttr usualCardAttr;

	public override CardType CardType => CardType.Card;

	public bool IsComsumeable => usualCardAttr.IsComsumeableCard;

	public virtual int ApCost => usualCardAttr.ApCost;

	public string IllustrationName => usualCardAttr.IllustrationName;

	protected BaseEffectConfig EffectConfig
	{
		get
		{
			if (!usualCardAttr.EffectConfigName.IsNullOrEmpty())
			{
				return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(usualCardAttr.EffectConfigName, "EffectConfigScriObj");
			}
			return null;
		}
	}

	protected abstract PointDownHandler pointdownHandler { get; }

	protected abstract PointUpHandler pointupHandler { get; }

	public abstract bool IsWillBreakDefence { get; }

	public UsualCard(UsualCardAttr usualCardAttr)
		: base(usualCardAttr)
	{
		this.usualCardAttr = usualCardAttr;
	}

	public abstract void UsualCardEffect(Player player, bool isMain, Action handler);

	protected abstract bool IsSatisfySpecialStatus(Player player);

	public virtual bool IsCanCast(Player player, bool isMain, out int finalApValue, out string failResult)
	{
		return IsSatisfiedAp(player, out finalApValue, out failResult);
	}

	public bool IsSatisfiedAp(Player player, out int finalValue, out string failResult)
	{
		player.PlayerUseCardApReduce(this, out var amount);
		int num = Mathf.Max(ApCost - amount, 0);
		if (num > player.PlayerAttr.ApAmount)
		{
			failResult = "LackOfAp".LocalizeText();
			finalValue = num;
			return false;
		}
		failResult = string.Empty;
		finalValue = num;
		return true;
	}

	protected static void HandleEffect(BaseEffectConfig config, Transform[] targets, Action handler)
	{
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(config, null, targets, handler);
	}

	public void OnPointDown()
	{
		pointdownHandler.OnPointDown();
	}

	public void HandlePointDown(Vector3 pointViewRect)
	{
		pointdownHandler.HandleDown(pointViewRect);
	}

	public void HandPointUp(Vector3 pointViewRect, Action tryUseAction, Action tryCancelAction)
	{
		pointupHandler.PointUp(pointViewRect, tryUseAction, tryCancelAction);
	}

	public void HandleShowCastHint(Transform target)
	{
		pointdownHandler.HandleShowCastHint(target);
	}

	public void HandleEndCastHint()
	{
		pointdownHandler.HandleEndCastHit();
	}

	public void EquipCard()
	{
		OnEquiped();
	}

	protected virtual void OnEquiped()
	{
	}

	public void ReleaseCard()
	{
		OnReleased();
	}

	protected virtual void OnReleased()
	{
	}
}
