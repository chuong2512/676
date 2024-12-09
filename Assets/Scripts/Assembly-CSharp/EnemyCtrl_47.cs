using System;
using UnityEngine;

public class EnemyCtrl_47 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1_1ConfigName;

	private BaseEffectConfig action1_1Config;

	[SerializeField]
	private string Action1_2ConfigName;

	private BaseEffectConfig action1_2Config;

	[SerializeField]
	private string Action1_3ConfigName;

	private BaseEffectConfig action1_3Config;

	[SerializeField]
	private string Action2_1ConfigName;

	private BaseEffectConfig action2_1Config;

	[SerializeField]
	private string Action2_2ConfigName;

	private BaseEffectConfig action2_2Config;

	[SerializeField]
	private string Action2_3ConfigName;

	private BaseEffectConfig action2_3Config;

	public BaseEffectConfig Action1_1Config
	{
		get
		{
			if (action1_1Config.IsNull() && !Action1_1ConfigName.IsNullOrEmpty())
			{
				action1_1Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action1_1ConfigName);
			}
			return action1_1Config;
		}
	}

	public BaseEffectConfig Action1_2Config
	{
		get
		{
			if (action1_2Config.IsNull() && !Action1_2ConfigName.IsNullOrEmpty())
			{
				action1_2Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action1_2ConfigName);
			}
			return action1_2Config;
		}
	}

	public BaseEffectConfig Action1_3Config
	{
		get
		{
			if (action1_3Config.IsNull() && !Action1_3ConfigName.IsNullOrEmpty())
			{
				action1_3Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action1_3ConfigName);
			}
			return action1_3Config;
		}
	}

	public BaseEffectConfig Action2_1Config
	{
		get
		{
			if (action2_1Config.IsNull() && !Action2_1ConfigName.IsNullOrEmpty())
			{
				action2_1Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action2_1ConfigName);
			}
			return action2_1Config;
		}
	}

	public BaseEffectConfig Action2_2Config
	{
		get
		{
			if (action2_2Config.IsNull() && !Action2_2ConfigName.IsNullOrEmpty())
			{
				action2_2Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action2_2ConfigName);
			}
			return action2_2Config;
		}
	}

	public BaseEffectConfig Action2_3Config
	{
		get
		{
			if (action2_3Config.IsNull() && !Action2_3ConfigName.IsNullOrEmpty())
			{
				action2_3Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action2_3ConfigName);
			}
			return action2_3Config;
		}
	}

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_47"));
		enemyEntity = new Enemy_47(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public void Action1_1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1_1Config, targets, effect, actionOver);
	}

	public void Action1_2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1_2Config, targets, effect, actionOver);
	}

	public void Action1_3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1_3Config, targets, effect, actionOver);
	}

	public void Action2_1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action2_1Config, targets, effect, actionOver);
	}

	public void Action2_2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action2_2Config, targets, effect, actionOver);
	}

	public void Action2_3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action2_3Config, targets, effect, actionOver);
	}
}
