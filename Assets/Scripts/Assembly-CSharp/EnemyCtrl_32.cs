using System;
using UnityEngine;

public class EnemyCtrl_32 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	[SerializeField]
	private string Action2ConfigName;

	private BaseEffectConfig action1Config;

	private BaseEffectConfig action2Config;

	private Transform bubblePoint;

	public string[] Action1BubbleKeys;

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

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_32"));
		enemyEntity = new Enemy_32(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		bubblePoint = base.transform.Find("BubblePoint");
	}

	public void EnemyAwake()
	{
		((Monster32AnimCtrl)enemyAnimCtrl).AwakeAnim();
	}

	public void EnemySleep()
	{
		((Monster32AnimCtrl)enemyAnimCtrl).IdleAnim();
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action1BubbleKeys, bubblePoint);
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action2Config, time, targets, effect, actionOver);
	}
}
