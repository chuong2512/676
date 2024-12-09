using System;
using UnityEngine;

public class EnemyCtrl_10 : EnemyBaseCtrl
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
		bubblePoint = base.transform.Find("BubblePoint");
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_10"));
		enemyEntity = new Enemy_10(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action1BubbleKeys, bubblePoint);
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action2Config, targets, effect, actionOver);
	}
}
