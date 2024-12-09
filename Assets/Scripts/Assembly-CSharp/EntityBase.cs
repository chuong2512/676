using System.Collections.Generic;
using UnityEngine;

public abstract class EntityBase
{
	protected bool isDead;

	protected List<BuffType> ImmuneBuffList = new List<BuffType>();

	public abstract bool IsActionOver { get; }

	public abstract Camp Camp { get; }

	public abstract EntityAttr EntityAttr { get; }

	public abstract float BuffHintScale { get; }

	public abstract Transform ArmorTrans { get; }

	public abstract string EntityName { get; }

	public abstract Transform EntityTransform { get; }

	public bool IsDead => isDead;

	public int PowerBuff => ((Buff_Power)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_Power))?.Power ?? 0;

	public void AddImmuneBuff(BuffType buffType)
	{
		if (!ImmuneBuffList.Contains(buffType))
		{
			ImmuneBuffList.Add(buffType);
		}
	}

	public void RemoveImmuneBuff(BuffType buffType)
	{
		ImmuneBuffList.Remove(buffType);
	}

	public bool IsImmuneBuff(BuffType buffType)
	{
		return ImmuneBuffList.Contains(buffType);
	}

	protected static void RemoveAllBuff(EntityBase entity)
	{
		BaseBuff[] allBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetAllBuff(entity);
		if (!allBuff.IsNull())
		{
			for (int i = 0; i < allBuff.Length; i++)
			{
				entity.RemoveBuff(allBuff[i]);
			}
		}
	}

	public static void RemoveAllDebuff(EntityBase entityBase)
	{
		List<BaseBuff> allDebuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetAllDebuff(entityBase);
		if (!allDebuff.IsNull())
		{
			for (int i = 0; i < allDebuff.Count; i++)
			{
				entityBase.RemoveBuff(allDebuff[i]);
			}
		}
	}

	public static void RemoveRandomDebuff(EntityBase entityBase)
	{
		List<BaseBuff> allDebuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetAllDebuff(entityBase);
		if (!allDebuff.IsNull() && allDebuff.Count > 0)
		{
			entityBase.RemoveBuff(allDebuff[Random.Range(0, allDebuff.Count)]);
		}
	}

	public virtual int TakeDamage(int dmg, EntityBase caster, bool isAbsDmg)
	{
		int finalDmg;
		return TakeDamageProcess(dmg, caster, isAbsDmg, out finalDmg);
	}

	public void TakeDirectoryArmorDmg(int dmg)
	{
		int num = Mathf.Min(dmg, EntityAttr.Armor);
		EntityAttr.ReduceArmor(num);
		OnEntityGetHurtOnBattle(0, num, isAbsDmg: false);
	}

	protected int TakeDamageProcess(int dmg, EntityBase caster, bool isAbsDmg, out int finalDmg)
	{
		if (isDead)
		{
			finalDmg = 0;
			return 0;
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		string empty = string.Empty;
		empty = string.Format("{0}受到伤害，初始伤害值为：{1}, 伤害源为：{2}", EntityName, dmg, caster.IsNull() ? "无" : caster.EntityName);
		BroadacastEntityBeAttackEvent(caster);
		finalDmg = dmg;
		finalDmg = CheckAllBuffInfluenceToDamage(finalDmg, isAbsDmg, caster, ref empty);
		empty += $",最终{EntityName}本次会受到{finalDmg}点伤害";
		int result = CalculateEntityHealthAndArmorReduce(finalDmg, isAbsDmg, ref empty);
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(empty);
		}
		TryBroadcastEntityGetHurtEvent(finalDmg, caster);
		return result;
	}

	protected void TryBroadcastEntityGetHurtEvent(int finalDmg, EntityBase caster)
	{
		if (finalDmg > 0)
		{
			EventManager.BroadcastEvent(this, EventEnum.E_EntityGetHurt, new SimpleEventData
			{
				objValue = caster,
				intValue = finalDmg
			});
		}
	}

	protected void BroadacastEntityBeAttackEvent(EntityBase caster)
	{
		EventManager.BroadcastEvent(this, EventEnum.E_EntityBeAttacked, new SimpleEventData
		{
			objValue = caster
		});
	}

	protected int CalculateEntityHealthAndArmorReduce(int finalDmg, bool isAbsDmg, ref string takeDmgDes)
	{
		int num = EntityAttr.ReduceArmor(finalDmg);
		OnEntityGetHurtOnBattle(num, finalDmg - num, isAbsDmg);
		if (num > 0)
		{
			EntityAttr.ReduceHealth(num);
		}
		takeDmgDes += $"，剩余生命值：{EntityAttr.Health}";
		return num;
	}

	protected abstract int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes);

	public static int CheckHoldPositionBuff(EntityBase entityBase, int finalDmg, ref string takeDmgDes)
	{
		if (finalDmg > 0)
		{
			BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_HoldPosition);
			if (!specificBuff.IsNull() && finalDmg > 1)
			{
				finalDmg = 1;
				specificBuff.TakeEffect(entityBase);
				takeDmgDes = takeDmgDes + "," + entityBase.EntityName + "拥有招加buff，本次伤害降低至1";
			}
		}
		return finalDmg;
	}

	public static int CheckShadowEscapeBuff(EntityBase entityBase, int finalDmg, ref string takeDmgDes)
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_ShadowEscape);
		if (finalDmg > 1 && !specificBuff.IsNull())
		{
			takeDmgDes = takeDmgDes + "," + entityBase.EntityName + "的影遁buff生效，伤害降低至1";
			finalDmg = 1;
			specificBuff.TakeEffect(entityBase);
		}
		return finalDmg;
	}

	public static int CheckHolyProtectBuff(EntityBase entityBase, int finalDmg, ref string takeDmgDes)
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(entityBase, BuffType.Buff_HolyProtect);
		if (finalDmg > 0 && !specificBuff.IsNull())
		{
			finalDmg = 0;
			takeDmgDes = takeDmgDes + "," + entityBase.EntityName + "拥有神圣庇护，最终伤害降低至0";
			specificBuff.TakeEffect(entityBase);
		}
		return finalDmg;
	}

	public static int CheckArmorBrokenBuff(EntityBase entityBase, int finalDmg, ref string takeDmgDes)
	{
		if (finalDmg > 0 && Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(entityBase, BuffType.Buff_BrokenArmor))
		{
			finalDmg = Mathf.FloorToInt(1.5f * (float)finalDmg);
			takeDmgDes += $",{entityBase.EntityName}拥有破甲buff，受到的伤害提升50%，此时伤害为{finalDmg}";
		}
		return finalDmg;
	}

	protected virtual void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
	}

	public virtual void EntityRecoveryHealthOnBattle(int value)
	{
		EntityAttr.RecoveryHealth(value);
	}

	public virtual void Dead()
	{
		isDead = true;
	}

	public virtual void GetBuff(BaseBuff buff)
	{
		if (!isDead && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			if (ImmuneBuffList.Contains(buff.BuffType))
			{
				OnImmueBuff(buff);
			}
			else
			{
				Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetBuff(this, buff);
			}
		}
	}

	protected abstract void OnImmueBuff(BaseBuff buff);

	public virtual void AddBuffIcon(BaseBuff buff)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(EntityName + "获得新的Buff:" + (buff.BuffType.ToString() + "_Name").LocalizeText());
		}
	}

	public virtual void RemoveBuff(BaseBuff buff)
	{
		Singleton<GameManager>.Instance.BattleSystem.BuffSystem.RemoveBuff(this, buff);
	}

	public virtual void RemoveBuffIcon(BaseBuff buff)
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(EntityName + "移除Buff:" + (buff.BuffType.ToString() + "_Name").LocalizeText());
		}
	}

	public virtual void UpdateBuffIcon(BaseBuff buff)
	{
	}

	public virtual float PowUpRate()
	{
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(this, BuffType.Buff_Shocked))
		{
			return 0f;
		}
		return -0.5f;
	}
}
