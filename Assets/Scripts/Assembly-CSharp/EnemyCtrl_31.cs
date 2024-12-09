using System;
using UnityEngine;

public class EnemyCtrl_31 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	[SerializeField]
	private string Action2ConfigName;

	[SerializeField]
	private string Action3ConfigName;

	[SerializeField]
	private string Action4ConfigName;

	[SerializeField]
	private string Action5ConfigName;

	[SerializeField]
	private string Action6ConfigName;

	private BaseEffectConfig action1Config;

	private BaseEffectConfig action2Config;

	private BaseEffectConfig action3Config;

	private BaseEffectConfig action4Config;

	private BaseEffectConfig action5Config;

	private BaseEffectConfig action6Config;

	private Transform bubblePoint;

	public string[] Action1BubbleKeys;

	public string[] Action2BubbleKeys;

	public string[] Action3BubbleKeys;

	public string[] Action4BubbleKeys;

	public string[] Action5BubbleKeys;

	public string[] Action6BubbleKeys;

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

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_31"));
		enemyEntity = new Enemy_31(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		bubblePoint = base.transform.Find("BubblePoint");
	}

	public void SetIdleAnim()
	{
		enemyAnimCtrl.IdleAnim();
	}

	public void SetSlot(int index, Monster31AnimCtrl.SkinType type)
	{
		((Monster31AnimCtrl)enemyAnimCtrl).SetSlot(index, type);
	}

	public void ClearAllSlots()
	{
		((Monster31AnimCtrl)enemyAnimCtrl).ClearAllSlots();
	}

	public void SetSkin(Monster31AnimCtrl.SkinType type)
	{
		((Monster31AnimCtrl)enemyAnimCtrl).SetSkin(type);
	}

	public void ChangeSkin(Action onChangeSkinAction, Action onCompleteAction)
	{
		((Monster31AnimCtrl)enemyAnimCtrl).ChangeSkinAnim(onChangeSkinAction, onCompleteAction);
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action1BubbleKeys, bubblePoint);
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action2BubbleKeys, bubblePoint);
		ActionAnim(Action2Config, targets, effect, actionOver);
	}

	public void Action3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action3BubbleKeys, bubblePoint);
		ActionAnim(Action3Config, targets, effect, actionOver);
	}

	public void Action4Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action4BubbleKeys, bubblePoint);
		ActionAnim(Action4Config, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action5BubbleKeys, bubblePoint);
		ActionAnim(Action5Config, targets, effect, actionOver);
	}

	public void Action6Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action6BubbleKeys, bubblePoint);
		ClearAllSlots();
		ActionAnim(Action6Config, time, targets, effect, actionOver);
	}
}
