using System;
using System.Collections;
using UnityEngine;

public class EnemyCtrl_58 : EnemyBaseCtrl
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

	private Monster58AnimCtrl _monster58AnimCtrl;

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
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_58"));
		enemyEntity = new Enemy_58(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		_monster58AnimCtrl = (Monster58AnimCtrl)enemyAnimCtrl;
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy58(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy58(Action2Config, targets, effect, actionOver);
	}

	public void Action3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy58(Action3Config, targets, effect, actionOver);
	}

	public void Action4Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy58(Action4Config, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy58(Action5Config, targets, effect, actionOver);
	}

	private void ActionAnimForEnemy58(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		StartCoroutine(ActionAnimForEnemy58_IE(config, targets, effect, actionOver));
	}

	private IEnumerator ActionAnimForEnemy58_IE(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		HideEnemyMean();
		isEnemyActionAnim = true;
		yield return new WaitForSeconds(0.3f);
		EnemyBaseCtrl.HandleEffectConfig(config, base.transform, targets, effect, delegate
		{
			isEnemyActionAnim = false;
			actionOver?.Invoke();
		});
	}

	public void SetIdle1()
	{
		_monster58AnimCtrl.SetIdle1();
	}
}
