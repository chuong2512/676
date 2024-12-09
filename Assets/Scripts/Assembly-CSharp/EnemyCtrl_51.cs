using System;
using System.Collections;
using UnityEngine;

public class EnemyCtrl_51 : EnemyBaseCtrl
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

	private DragonHandAnimCtrl _dragonHandAnimCtrl;

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

	protected override void InitComponent()
	{
		meanRoot = base.transform.Find("MeanRoot");
		boxCollider2D = GetComponent<BoxCollider2D>();
		healthBarCtrl = base.transform.Find("HealthBar").GetComponent<HealthBarCtrl>();
		Transform transform = base.transform.Find("EnemyAnim");
		ShadowTrans = base.transform.Find("Shadow");
		if (!transform.IsNull())
		{
			enemyAnimCtrl = (_dragonHandAnimCtrl = transform.GetComponent<DragonHandAnimCtrl>());
			enemyAnimCtrl.Init();
		}
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_51"));
		enemyEntity = new Enemy_51(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public void SetDragonHeadCtrl(Monster50AnimCtrl monster50AnimCtrl)
	{
		_dragonHandAnimCtrl.SetDragonHeadAnimCtrl(monster50AnimCtrl, isLeft: false);
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action2Config, targets, effect, actionOver);
	}

	public void Action3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action3Config, targets, effect, actionOver);
	}

	public void Action4Anim(Transform[] targets, int time, Action<int> effect, Action actionOver)
	{
		StartCoroutine(Action4Anim_IE(targets, time, effect, actionOver));
	}

	private IEnumerator Action4Anim_IE(Transform[] targets, int time, Action<int> effect, Action actionOver)
	{
		enemyAnimCtrl.PlaySkeletonAnim(new EnemySkeletonAnimEffectStep.EnemySkeletonEffect
		{
			skeletonAnimName = "youzhua_attack4",
			skeletonAnimTrack = 0,
			isLoop = false
		});
		yield return new WaitForSeconds(0.7f);
		ActionAnimForBoss(Action4Config, time, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForBoss(Action5Config, targets, effect, actionOver);
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

	protected override void OnEnemyDeadAnim(Action deadAction)
	{
		Singleton<EnemyController>.Instance.RemoveMonster(enemyEntity, isNeedReAdjust: false);
		_dragonHandAnimCtrl.Dead();
		deadAction?.Invoke();
		healthBarCtrl.gameObject.SetActive(value: false);
	}

	public override void FlashWhite()
	{
		if (base.gameObject.activeSelf && !enemyEntity.IsDead)
		{
			_dragonHandAnimCtrl.FlashWhite();
		}
	}

	protected override void OnEnableMeanRootAnim()
	{
	}
}
