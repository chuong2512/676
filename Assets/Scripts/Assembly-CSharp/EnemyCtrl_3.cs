using System;
using UnityEngine;

public class EnemyCtrl_3 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	private BaseEffectConfig action1Config;

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

	protected override void InitComponent()
	{
		base.InitComponent();
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_3"));
		enemyEntity = new Enemy_3(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1Config, targets, effect, actionOver);
	}
}
