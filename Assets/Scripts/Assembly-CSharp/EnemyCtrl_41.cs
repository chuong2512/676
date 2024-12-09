using System;
using UnityEngine;

public class EnemyCtrl_41 : EnemyBaseCtrl
{
	[SerializeField]
	private string Action1ConfigName;

	[SerializeField]
	private string Action2ConfigName;

	private BaseEffectConfig action1Config;

	private BaseEffectConfig action2Config;

	private Action getshieldAction;

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
		EnemyAttr enemyAttr = new EnemyAttr(DataManager.Instance.GetEnemyAttr("Monster_41"));
		enemyEntity = new Enemy_41(enemyAttr, this);
		enemyAttr.SetEnemyBase(enemyEntity);
	}

	public override void UpdateArmor(int armor)
	{
		base.UpdateArmor(armor);
		if (armor == 0)
		{
			((Enemy_41)enemyEntity).OnArmorBroken();
		}
	}

	public void GetShiled(Action completeAction)
	{
		((Monster41AnimCtrl)enemyAnimCtrl).GetShieldAnim();
		getshieldAction = completeAction;
	}

	public void GetShieldAction()
	{
		getshieldAction?.Invoke();
		getshieldAction = null;
	}

	public void LoseShield()
	{
		((Monster41AnimCtrl)enemyAnimCtrl).LoseShieldAnim();
	}

	public void SetIdleAnim()
	{
		enemyAnimCtrl.IdleAnim();
	}

	public void Action1Anim(Transform[] targets, Action effect, Action actionOver)
	{
		ActionAnim(Action1Config, targets, effect, actionOver);
	}

	public void Action2Anim(int time, Transform[] targets, Action<int> effect, Action actionOver)
	{
		ActionAnim(Action2Config, time, targets, effect, actionOver);
	}
}
