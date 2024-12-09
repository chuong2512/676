using System;
using System.Collections;
using UnityEngine;

public class EnemyCtrl_20 : EnemyBaseCtrl
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
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_20"));
		enemyEntity = new Enemy_20(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
		bubblePoint = base.transform.Find("BubblePoint");
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		EnemyBaseCtrl.BubbleTalk(Action1BubbleKeys, bubblePoint);
		ActionAnimForEnemy20(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnimForEnemy20(Action2Config, targets, effect, actionOver);
	}

	private void ActionAnimForEnemy20(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
	{
		StartCoroutine(ActionAnimForEnemy20_IE(config, targets, effect, actionOver));
	}

	private IEnumerator ActionAnimForEnemy20_IE(BaseEffectConfig config, Transform[] targets, Action effect, Action actionOver)
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
}
