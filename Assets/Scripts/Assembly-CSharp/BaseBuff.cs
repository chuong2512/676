using UnityEngine;

public abstract class BaseBuff
{
	protected const string BuffColorStr = "<color=#27dd34ff>";

	protected const string DeBuffColorStr = "<color=#ec2125ff>";

	protected const string NormalColorStr = "<color=#e9e9e9ff>";

	protected static readonly Color BuffColor = "27DD34FF".HexColorToColor();

	protected static readonly Color DeBuffColor = "EC2125FF".HexColorToColor();

	protected float buffRemainRound;

	protected EntityBase entityBase;

	protected BuffIconCtrl buffIconCtrl;

	protected bool isPlayer;

	private const string EffectAssetPath = "EffectConfigScriObj/Buff";

	public abstract BuffType BuffType { get; }

	public float ExactlyBuffRemainRound => buffRemainRound;

	public int BuffRemainRound => Mathf.CeilToInt(buffRemainRound);

	public virtual bool IsDebuff => DataManager.Instance.GetBuffDataByBuffType(BuffType).IsDebuff;

	public BaseBuff(EntityBase entityBase, int round)
	{
		this.entityBase = entityBase;
		buffRemainRound = round;
		isPlayer = entityBase is Player;
	}

	public void SetUpdateBuffAction(BuffIconCtrl buffIconCtrl)
	{
		this.buffIconCtrl = buffIconCtrl;
	}

	public virtual string GetBuffImmueHint()
	{
		return (IsDebuff ? "<color=#27dd34ff>" : "<color=#ec2125ff>") + "Immune".LocalizeText() + (BuffType.ToString() + "_Name").LocalizeText() + "</color>";
	}

	public virtual void HandleNewBuffAdd()
	{
		if (DataManager.Instance.GetBuffDataByBuffType(BuffType).IsNeedShow)
		{
			ShowBuffHint(entityBase.EntityTransform, !isPlayer, entityBase.BuffHintScale, BuffType, IsDebuff);
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadAddConfig(isPlayer, BuffType), entityBase.EntityTransform, null, null);
		BroadcastNewBuffAddEvent();
	}

	protected virtual void BroadcastNewBuffAddEvent()
	{
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetNewBuff, new BuffEventData
		{
			buffType = BuffType,
			intValue = BuffRemainRound
		});
	}

	protected static void AtkEntity(EntityBase entityBase, int amount, bool isAbsDmg)
	{
		Buff_Dodge buff_Dodge;
		if ((buff_Dodge = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_Dodge) as Buff_Dodge) != null)
		{
			buff_Dodge.TakeEffect(entityBase);
		}
		else
		{
			entityBase.TakeDamage(amount, null, isAbsDmg);
		}
	}

	public virtual void HandleBuffRemove()
	{
		BroadcastBuffRemoveEvent();
	}

	protected virtual void BroadcastBuffRemoveEvent()
	{
		EventManager.BroadcastEvent(entityBase, EventEnum.E_RemoveBuff, new BuffEventData
		{
			buffType = BuffType
		});
	}

	public virtual void HandleSameBuffAdd(BaseBuff baseBuff)
	{
		if (DataManager.Instance.GetBuffDataByBuffType(BuffType).IsNeedShow)
		{
			ShowBuffHint(entityBase.EntityTransform, !isPlayer, entityBase.BuffHintScale, BuffType, IsDebuff);
		}
		Singleton<BattleEffectManager>.Instance.HandleEffectConfig(LoadAddConfig(isPlayer, BuffType), entityBase.EntityTransform, null, null);
		BroadcastSameBuffAddEvent(baseBuff);
	}

	protected virtual void BroadcastSameBuffAddEvent(BaseBuff baseBuff)
	{
		EventManager.BroadcastEvent(entityBase, EventEnum.E_GetSameBuff, new BuffEventData
		{
			buffType = BuffType,
			intValue = baseBuff.BuffRemainRound
		});
	}

	public abstract void TakeEffect(EntityBase entityBase);

	public abstract void UpdateRoundTurn();

	public abstract string GetBuffHint();

	public abstract int GetBuffHinAmount();

	protected static void ShowBuffHint(Transform targetTrans, bool isSetParent, float scale, BuffType buffType, bool isDebuff)
	{
		Singleton<GameHintManager>.Instance.ShowBuffHint(targetTrans, isSetParent, scale, buffType, isDebuff ? DeBuffColor : BuffColor);
	}

	protected static BaseEffectConfig LoadEffectConfig(bool isPlayer, BuffType buffType)
	{
		BuffData buffDataByBuffType = DataManager.Instance.GetBuffDataByBuffType(buffType);
		return LoadConfigByName(isPlayer ? buffDataByBuffType.BuffEffectConfigForPlayer : buffDataByBuffType.BuffEffectConfigForEnemy);
	}

	protected static BaseEffectConfig LoadAddConfig(bool isPlayer, BuffType buffType)
	{
		BuffData buffDataByBuffType = DataManager.Instance.GetBuffDataByBuffType(buffType);
		return LoadConfigByName(isPlayer ? buffDataByBuffType.BuffAddEffectForPlayer : buffDataByBuffType.BuffAddEffectForEnemy);
	}

	protected static BaseEffectConfig LoadConfigByName(string configName)
	{
		if (configName.IsNullOrEmpty())
		{
			return null;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadScriptObject<BaseEffectConfig>(configName, "EffectConfigScriObj/Buff");
	}
}
