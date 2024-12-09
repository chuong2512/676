using System;
using UnityEngine;

public class EnemyCtrl_37 : EnemyBaseCtrl
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

	[SerializeField]
	private string Action7ConfigName;

	[SerializeField]
	private string Action8ConfigName;

	[SerializeField]
	private string Action9ConfigName;

	private BaseEffectConfig action1Config;

	private BaseEffectConfig action2Config;

	private BaseEffectConfig action3Config;

	private BaseEffectConfig action4Config;

	private BaseEffectConfig action5Config;

	private BaseEffectConfig action6Config;

	private BaseEffectConfig action7Config;

	private BaseEffectConfig action8Config;

	private BaseEffectConfig action9Config;

	private Transform bubblePoint;

	public string[] Action2BubbleKeys;

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

	public BaseEffectConfig Action9Config
	{
		get
		{
			if (action9Config.IsNull() && !Action9ConfigName.IsNullOrEmpty())
			{
				action9Config = EnemyBaseCtrl.LoadEnemyEffectConfig(Action9ConfigName);
			}
			return action9Config;
		}
	}

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_37"));
		enemyEntity = new Enemy_37(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		bubblePoint = base.transform.Find("BubblePoint");
	}

	public void RebornAnim()
	{
		((Monster37AnimCtrl)enemyAnimCtrl).RebornAnim();
	}

	public void IdleAnim()
	{
		enemyAnimCtrl.IdleAnim();
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalkMRight(Action2BubbleKeys, bubblePoint);
		ActionAnim(Action2Config, targets, effect, actionOver);
	}

	public void Action3Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action3Config, targets, effect, actionOver);
	}

	public void Action4Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action4Config, time, targets, effect, actionOver);
	}

	public void Action5Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action5Config, targets, effect, actionOver);
	}

	public void Action6Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action6Config, time, targets, effect, actionOver);
	}

	public void Action7Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action7Config, targets, effect, actionOver);
	}

	public void Action8Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action8Config, time, targets, effect, actionOver);
	}

	public void Action9Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action9Config, targets, effect, actionOver);
	}
}
