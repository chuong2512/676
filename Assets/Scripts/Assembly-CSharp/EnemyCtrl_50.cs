using System;
using System.Collections;
using UnityEngine;

public class EnemyCtrl_50 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	private BaseEffectConfig action1Config;

	[SerializeField]
	private string Action2ConfigName;

	private BaseEffectConfig action2Config;

	[SerializeField]
	private string Action3ConfigName;

	private BaseEffectConfig action3Config;

	[SerializeField]
	private string Action4ConfigName;

	private BaseEffectConfig action4Config;

	[SerializeField]
	private string Action5ConfigName;

	private BaseEffectConfig action5Config;

	[SerializeField]
	private string Action6ConfigName;

	private BaseEffectConfig action6Config;

	[SerializeField]
	private string Action7ConfigName;

	private BaseEffectConfig action7Config;

	[SerializeField]
	private string Action8ConfigName;

	private BaseEffectConfig action8Config;

	[SerializeField]
	private ParticleSystem firePartical;

	private Enemy_50 _enemy50;

	private BossUI bossUI;

	public BaseEffectConfig Action1Config
	{
		get
		{
			if (action1Config.IsNull() && !Action1ConfigName.IsNullOrEmpty())
			{
				action1Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action1ConfigName);
			}
			return action1Config;
		}
	}

	public BaseEffectConfig Action2Config
	{
		get
		{
			if (action2Config.IsNull() && !Action2ConfigName.IsNullOrEmpty())
			{
				action2Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action2ConfigName);
			}
			return action2Config;
		}
	}

	public BaseEffectConfig Action3Config
	{
		get
		{
			if (action3Config.IsNull() && !Action3ConfigName.IsNullOrEmpty())
			{
				action3Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action3ConfigName);
			}
			return action3Config;
		}
	}

	public BaseEffectConfig Action4Config
	{
		get
		{
			if (action4Config.IsNull() && !Action4ConfigName.IsNullOrEmpty())
			{
				action4Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action4ConfigName);
			}
			return action4Config;
		}
	}

	public BaseEffectConfig Action5Config
	{
		get
		{
			if (action5Config.IsNull() && !Action5ConfigName.IsNullOrEmpty())
			{
				action5Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action5ConfigName);
			}
			return action5Config;
		}
	}

	public BaseEffectConfig Action6Config
	{
		get
		{
			if (action6Config.IsNull() && !Action6ConfigName.IsNullOrEmpty())
			{
				action6Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action6ConfigName);
			}
			return action6Config;
		}
	}

	public BaseEffectConfig Action7Config
	{
		get
		{
			if (action7Config.IsNull() && !Action7ConfigName.IsNullOrEmpty())
			{
				action7Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action7ConfigName);
			}
			return action7Config;
		}
	}

	public BaseEffectConfig Action8Config
	{
		get
		{
			if (action8Config.IsNull() && !Action8ConfigName.IsNullOrEmpty())
			{
				action8Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action8ConfigName);
			}
			return action8Config;
		}
	}

	public Transform RightHandPoint { get; private set; }

	public Transform LeftHandPoint { get; private set; }

	public Monster50AnimCtrl DragonHeadAnimCtrl { get; private set; }

	public override Transform HealthBarTransform => bossUI.HealthBarTrans;

	public Transform ArmorTrans => bossUI.ArmorTrans;

	protected override void InitComponent()
	{
		boxCollider2D = GetComponent<BoxCollider2D>();
		Transform transform = base.transform.Find("EnemyAnim");
		if (!transform.IsNull())
		{
			Monster50AnimCtrl monster50AnimCtrl = (Monster50AnimCtrl)(enemyAnimCtrl = (DragonHeadAnimCtrl = transform.GetComponent<Monster50AnimCtrl>()));
			enemyAnimCtrl.Init();
			StartCoroutine(InitAnimCtrl_IE());
		}
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_50"));
		enemyEntity = (_enemy50 = new Enemy_50(enemyAttr, this));
		enemyAttr.SetEnemyBase(enemyEntity);
		DragonHeadAnimCtrl.SetEnemyBase(_enemy50);
		DragonHeadAnimCtrl.SetNormalSkin();
		RightHandPoint = base.transform.Find("RightHandPoint");
		LeftHandPoint = base.transform.Find("LeftHandPoint");
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, int time, Action<int> effect, Action actionOver)
	{
		StartCoroutine(Action2Anim_IE(targets, time, effect, actionOver));
	}

	private IEnumerator Action2Anim_IE(Transform[] targets, int time, Action<int> effect, Action actionOver)
	{
		firePartical.Play();
		enemyAnimCtrl.PlaySkeletonAnim(new EnemySkeletonAnimEffectStep.EnemySkeletonEffect
		{
			skeletonAnimName = "longtou_attack2",
			skeletonAnimTrack = 0,
			isLoop = false
		});
		yield return new WaitForSeconds(0.7f);
		ActionAnimForBoss(Action2Config, time, targets, effect, actionOver);
	}

	public void Action3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action3Config, targets, effect, actionOver);
	}

	public void Action4Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action4Config, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action5Config, targets, effect, actionOver);
	}

	public void Action6Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action6Config, targets, effect, actionOver);
	}

	public void Action7Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action7Config, targets, effect, actionOver);
	}

	public void Action8Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action8Config, targets, effect, actionOver);
	}

	protected void ActionAnimForBoss(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		StartCoroutine(ActionAnim_IE(config, targets, effect, actionOver));
	}

	private IEnumerator ActionAnim_IE(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		HideEnemyMean();
		isEnemyActionAnim = true;
		yield return new WaitForSeconds(0.1f);
		EnemyBaseCtrl.HandleEffectConfig(config, base.transform, targets, effect, delegate
		{
			actionOver?.Invoke();
			isEnemyActionAnim = false;
		});
	}

	protected void ActionAnimForBoss(BaseEffectConfig config, int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		StartCoroutine(ActionAnim_IE(config, time, targets, effect, actionOver));
	}

	private IEnumerator ActionAnim_IE(BaseEffectConfig config, int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		HideEnemyMean();
		isEnemyActionAnim = true;
		yield return new WaitForSeconds(0.1f);
		if (time == 0)
		{
			EnemyBaseCtrl.HandleEffectConfig(config, base.transform, targets, delegate
			{
				effect(0);
			}, delegate
			{
				actionOver?.Invoke();
				isEnemyActionAnim = false;
			});
			yield break;
		}
		bool isMove = false;
		int i = 0;
		while (i < time)
		{
			isMove = true;
			int tempI = i;
			EnemyBaseCtrl.HandleEffectConfig(config, base.transform, targets, delegate
			{
				effect(tempI);
			}, delegate
			{
				isMove = false;
			});
			while (isMove)
			{
				yield return null;
			}
			if (enemyEntity.IsDead)
			{
				break;
			}
			int num = i + 1;
			i = num;
		}
		actionOver?.Invoke();
		isEnemyActionAnim = false;
	}

	protected override void OnEnableMeanRootAnim()
	{
	}

	public override void AddMean(MeanHandler[] meanHandlers)
	{
		bossUI.AddMean(meanHandlers);
	}

	protected override void HideEnemyMean()
	{
		bossUI.HideEnemyMean();
	}

	public override void ShowEnemyMean()
	{
		bossUI.ShowEnemyMean();
	}

	protected override void InitHealthBar()
	{
		EnemyData enemyAttr = DataManager.Instance.GetEnemyAttr("Monster_50");
		bossUI = SingletonDontDestroy<UIManager>.Instance.ShowView("BossUI", enemyAttr.NameKey.LocalizeText(), enemyAttr.MaxHealth, enemyAttr.BaseArmor) as BossUI;
	}

	public override void UpdateHealth(int currentHealth, int maxHealth)
	{
		bossUI.UpdateHealth(currentHealth, maxHealth);
	}

	public override void UpdateArmor(int armor)
	{
		bossUI.UpdateArmor(armor);
	}

	protected override void ClearAllBuff()
	{
		bossUI.ClearHealthBarBuff();
	}

	public override void AddBuffIcon(BaseBuff buff)
	{
		bossUI.AddBuff(buff);
	}

	public override void RemoveBuffIcon(BaseBuff buff)
	{
		bossUI.RemoveBuff(buff);
	}

	public override void UpdateBuffIcon(BaseBuff buff)
	{
		bossUI.UpdateBuff(buff);
	}

	protected override void RecycleMeanIcon()
	{
		bossUI.RecycleMeanIcon();
	}

	public override void FlashWhite()
	{
		if (base.gameObject.activeSelf && !enemyEntity.IsDead)
		{
			DragonHeadAnimCtrl.FlashWhite();
		}
	}

	protected override void OnEnemyDeadAnim(Action DeadAction)
	{
		Singleton<EnemyController>.Instance.RemoveMonster(enemyEntity, isNeedReAdjust: false);
		DeadAction?.Invoke();
	}
}
