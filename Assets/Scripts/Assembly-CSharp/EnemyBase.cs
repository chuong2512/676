using UnityEngine;

public abstract class EnemyBase : EntityBase
{
	public abstract class EnemyMean
	{
		protected EnemyBase thisEnemy;

		public abstract bool ItWillBreakDefence { get; }

		public abstract bool IsNeedUpdateByBuffChange { get; }

		public EnemyMean(EnemyBase enemyBase)
		{
			thisEnemy = enemyBase;
		}

		public virtual void UpdateMean()
		{
			OnSetMean();
		}

		public abstract void OnSetMean();

		public abstract void OnLogic();

		public virtual void ResetMean()
		{
		}
	}

	public abstract class AtkEnemyMean : EnemyMean
	{
		protected int realDmg;

		protected AtkEnemyMean(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		protected abstract int GetBaseDmg();

		protected int RealDmg()
		{
			return GetRealDamage(thisEnemy, GetBaseDmg());
		}

		protected int RealDmg(out string atkDes)
		{
			atkDes = thisEnemy.GetEnemyAtkDes(GetBaseDmg().ToString(), out var pwdBuff, out var rate);
			return GetRealDamage(thisEnemy, GetBaseDmg(), pwdBuff, rate);
		}

		protected static int GetRealDamage(EnemyBase enemy, int baseDmg)
		{
			int num = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + enemy.PowerBuff) * (1f + enemy.PowUpRate())), 0, int.MaxValue);
			if (num <= 1)
			{
				return num;
			}
			if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(enemy, BuffType.Buff_Freeze))
			{
				return num;
			}
			return 1;
		}

		protected static int GetRealDamage(EnemyBase enemy, int baseDmg, int pwdBuff, float rate)
		{
			int num = Mathf.Clamp(Mathf.FloorToInt((float)(baseDmg + pwdBuff) * (1f + rate)), 0, int.MaxValue);
			if (num <= 1)
			{
				return num;
			}
			if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(enemy, BuffType.Buff_Freeze))
			{
				return num;
			}
			return 1;
		}
	}

	public abstract class EnemyState
	{
		public abstract void OnEnter(EnemyBase enemyBase);

		public abstract void OnExit(EnemyBase enemyBase);
	}

	public class IdleState : EnemyState
	{
		public override void OnEnter(EnemyBase enemyBase)
		{
		}

		public override void OnExit(EnemyBase enemyBase)
		{
		}
	}

	public class OnBattleState : EnemyState
	{
		public override void OnEnter(EnemyBase enemyBase)
		{
		}

		public override void OnExit(EnemyBase enemyBase)
		{
		}
	}

	public class DeadState : EnemyState
	{
		public override void OnEnter(EnemyBase enemyBase)
		{
		}

		public override void OnExit(EnemyBase enemyBase)
		{
		}
	}

	protected static IdleState EnemyIdleState = new IdleState();

	protected static OnBattleState EnemyOnBattleState = new OnBattleState();

	protected static DeadState EnemyDeadState = new DeadState();

	protected EnemyAttr enemyAttr;

	protected EnemyBaseCtrl enemyCtrl;

	protected EnemyMean currentEnemyMean;

	protected EnemyState currentEnemyState;

	private const string EnemyAtkDesFormat = "基础伤害：{0}，额外的力量值{1}，额外的伤害提升率{2}";

	public override bool IsActionOver => currentEnemyState == EnemyIdleState;

	public override float BuffHintScale => 0.01f;

	public override Transform ArmorTrans => enemyCtrl.HealthBarCtrl.ArmorTrans;

	public string EnemyCode => enemyAttr.EnemyCode;

	public override string EntityName => enemyAttr.NameKey.LocalizeText();

	public override Camp Camp => Camp.Enemy;

	public override EntityAttr EntityAttr => enemyAttr;

	public EnemyBaseCtrl EnemyCtrl => enemyCtrl;

	public override Transform EntityTransform => enemyCtrl.transform;

	protected abstract EnemyMean[] enemyActionArray { get; }

	public EnemyBase(EnemyAttr attr, EnemyBaseCtrl enemyCtrl)
	{
		enemyAttr = attr;
		this.enemyCtrl = enemyCtrl;
	}

	public void StartBattle()
	{
		isDead = false;
		enemyAttr.StartBattle();
		enemyCtrl.StartBattle();
		enemyCtrl.ShowEnemyMean();
		SetEnemyState(EnemyIdleState);
		ResetAllMeans();
		OnStartBattle();
	}

	private void ResetAllMeans()
	{
		if (enemyActionArray != null)
		{
			for (int i = 0; i < enemyActionArray.Length; i++)
			{
				enemyActionArray[i].ResetMean();
			}
		}
	}

	protected abstract void OnStartBattle();

	public virtual void OnBattleReady()
	{
		enemyCtrl.OnEnemyBorn();
	}

	public virtual void StartBattleAction()
	{
		SetEnemyState(EnemyOnBattleState);
		currentEnemyMean.OnLogic();
		if (currentEnemyMean.ItWillBreakDefence)
		{
			TryRemoveDefence();
		}
	}

	public override void Dead()
	{
		base.Dead();
		string soundName = "VoiceAct/" + EnemyCode + "_Dead";
		SingletonDontDestroy<AudioManager>.Instance.PlaySound(soundName);
		EventManager.BroadcastEvent(EventEnum.E_EnemyDead, new SimpleEventData
		{
			objValue = this
		});
		Singleton<GameManager>.Instance.BattleSystem.BuffSystem.RemoveBuff(this);
		enemyCtrl.OnEnemyDead(OnEnemyDeadEffect);
	}

	protected virtual void OnEnemyDeadEffect()
	{
	}

	protected static int EnemyAttackPlayer(int dmg, EnemyBase caster, bool isTrue)
	{
		EventManager.BroadcastEvent(caster, EventEnum.E_EnemyAtkPlayer, null);
		return Singleton<GameManager>.Instance.Player.TakeDamage(dmg, caster, isTrue);
	}

	protected static bool IsPlayerCanDodgeAttack(out BaseBuff buff)
	{
		buff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Dodge);
		return !buff.IsNull();
	}

	protected override int CheckAllBuffInfluenceToDamage(int finalDmg, bool isAbsDmg, EntityBase caster, ref string takeDmgDes)
	{
		finalDmg = EntityBase.CheckHolyProtectBuff(this, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckArmorBrokenBuff(this, finalDmg, ref takeDmgDes);
		finalDmg = CheckDefenceBuff(finalDmg, caster, ref takeDmgDes);
		finalDmg = CheckCoverBuff(finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckShadowEscapeBuff(this, finalDmg, ref takeDmgDes);
		finalDmg = EntityBase.CheckHoldPositionBuff(this, finalDmg, ref takeDmgDes);
		return finalDmg;
	}

	protected int CheckCoverBuff(int finalDmg, ref string takeDmgDes)
	{
		Buff_Cover buff_Cover;
		if ((buff_Cover = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_Cover) as Buff_Cover) != null)
		{
			finalDmg -= buff_Cover.DmgReduceAmount;
			finalDmg = Mathf.Clamp(finalDmg, 0, int.MaxValue);
			buff_Cover.TakeEffect(this);
			takeDmgDes += $",{EntityName}因为掩护buff，伤害减少，剩余伤害为{finalDmg}";
		}
		return finalDmg;
	}

	protected int CheckDefenceBuff(int finalDmg, EntityBase caster, ref string takeDmgDes)
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(this, BuffType.Buff_Defence))
		{
			finalDmg = Mathf.Max(0, finalDmg - ((EnemyAttr)EntityAttr).Block);
			takeDmgDes += $",{EntityName}当前处于防御状态，伤害受到格挡，剩余伤害为{finalDmg}";
			OnBlockDmg(caster);
		}
		return finalDmg;
	}

	protected static void OnBlockDmg(EntityBase caster)
	{
		EventManager.BroadcastEvent(EventEnum.E_EnemyBlockDmg, new SimpleEventData
		{
			objValue = caster
		});
	}

	protected override void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
		base.OnEntityGetHurtOnBattle(healthDmg, armorDmg, isAbsDmg);
		enemyCtrl.FlashWhite();
		if (healthDmg > 0)
		{
			Singleton<GameHintManager>.Instance.ShowDamageFlowingText(EnemyCtrl.transform, isSetParent: true, Vector3.zero, Vector2.one * 0.5f, healthDmg, 0.005f, isAbsDmg);
		}
		if (armorDmg > 0)
		{
			Singleton<GameHintManager>.Instance.ShowArmorDamageFlowingText(enemyCtrl.HealthBarCtrl.ArmorTrans, isSetParent: false, Vector3.zero, armorDmg, 0.005f, isAbsDmg);
		}
	}

	public override void EntityRecoveryHealthOnBattle(int value)
	{
		base.EntityRecoveryHealthOnBattle(value);
		Singleton<GameHintManager>.Instance.ShowHealingFlowingText(enemyCtrl.HealthBarTransform, isSetParent: true, Vector3.zero, Vector2.zero, value, 0.005f);
	}

	protected override void OnImmueBuff(BaseBuff buff)
	{
		Singleton<GameHintManager>.Instance.AddFlowingTextForImmueBuffHing(buff.GetBuffImmueHint(), 0.01f, EntityTransform);
	}

	protected void TryRemoveDefence()
	{
		BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_MagicProtect);
		if (specificBuff.IsNull())
		{
			BaseBuff specificBuff2 = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_Defence);
			if (specificBuff2 != null)
			{
				RemoveBuff(specificBuff2);
			}
		}
		else
		{
			specificBuff.TakeEffect(this);
		}
	}

	public virtual void NextAction()
	{
		enemyCtrl.ShowEnemyMean();
		currentEnemyMean = ((SingletonDontDestroy<Game>.Instance.isTest && enemyCtrl.actionIndex > 0) ? enemyActionArray[enemyCtrl.actionIndex - 1] : GetNextAction());
		currentEnemyMean.OnSetMean();
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}

	protected abstract EnemyMean GetNextAction();

	public string GetEnemyAtkDes(string baseDmgStr, out int pwdBuff, out float rate)
	{
		pwdBuff = base.PowerBuff;
		rate = PowUpRate();
		if (!Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(this, BuffType.Buff_Freeze))
		{
			return $"基础伤害：{baseDmgStr}，额外的力量值{pwdBuff}，额外的伤害提升率{rate}";
		}
		return EntityName + "因为冻结buff伤害被降低至1";
	}

	public void SetEnemyState(EnemyState state)
	{
		if (!currentEnemyState.IsNull())
		{
			currentEnemyState.OnExit(this);
		}
		currentEnemyState = state;
		currentEnemyState.OnEnter(this);
	}

	public override void AddBuffIcon(BaseBuff buff)
	{
		base.AddBuffIcon(buff);
		if (DataManager.Instance.GetBuffDataByBuffType(buff.BuffType).IsNeedShow)
		{
			enemyCtrl.AddBuffIcon(buff);
		}
		if (!currentEnemyMean.IsNull() && currentEnemyMean.IsNeedUpdateByBuffChange)
		{
			currentEnemyMean.UpdateMean();
		}
	}

	public override void RemoveBuffIcon(BaseBuff buff)
	{
		base.RemoveBuffIcon(buff);
		enemyCtrl.RemoveBuffIcon(buff);
		if (!currentEnemyMean.IsNull() && currentEnemyMean.IsNeedUpdateByBuffChange)
		{
			currentEnemyMean.UpdateMean();
		}
	}

	public override void UpdateBuffIcon(BaseBuff buff)
	{
		base.UpdateBuffIcon(buff);
		enemyCtrl.UpdateBuffIcon(buff);
		if (!currentEnemyMean.IsNull() && currentEnemyMean.IsNeedUpdateByBuffChange)
		{
			currentEnemyMean.UpdateMean();
		}
	}
}
