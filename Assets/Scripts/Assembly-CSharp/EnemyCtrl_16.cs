using System;
using UnityEngine;

public class EnemyCtrl_16 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action4ConfigName;

	[SerializeField]
	private string Action5ConfigName;

	[SerializeField]
	private string Action6ConfigName;

	[SerializeField]
	private string Action7ConfigName;

	private BaseEffectConfig action4Config;

	private BaseEffectConfig action5Config;

	private BaseEffectConfig action6Config;

	private BaseEffectConfig action7Config;

	private Transform bubblePoint;

	public string[] Action4BubbleKeys;

	public string[] Action7BubbleKeys;

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

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_16"));
		enemyEntity = new Enemy_16(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		bubblePoint = base.transform.Find("BubblePoint");
	}

	public void Action4Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action4BubbleKeys, bubblePoint);
		ActionAnim(Action4Config, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action5Config, targets, effect, actionOver);
	}

	public void Action6Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action6Config, targets, effect, actionOver);
	}

	public void Action7Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action7BubbleKeys, bubblePoint);
		ActionAnim(Action7Config, targets, effect, actionOver);
	}
}
