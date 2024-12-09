using System;
using UnityEngine;

public class EnemyCtrl_27 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	[SerializeField]
	private string Action2ConfigName;

	[SerializeField]
	private string Action3ConfigName;

	private BaseEffectConfig action1Config;

	private BaseEffectConfig action2Config;

	private BaseEffectConfig action3Config;

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

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_27"));
		enemyEntity = new Enemy_27(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action2Config, targets, effect, actionOver);
	}

	public void Action3Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action3Config, time, targets, effect, actionOver);
	}
}
