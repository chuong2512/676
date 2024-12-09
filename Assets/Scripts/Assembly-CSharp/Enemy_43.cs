using System.Collections.Generic;
using UnityEngine;

public class Enemy_43 : EnemyBase
{
	private class Enemy43_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy43_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpecialMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_43)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			((Enemy_43)thisEnemy).SummorMonster43();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy43_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy43_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 2)
			});
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_43)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成2次{realDmg}的非真实伤害({atkDes})");
			}
		}

		private void Effect(int i)
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
			}
			if (i == 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private EnemyCtrl_43 _enemyCtrl43;

	private bool preAction1;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_43(EnemyAttr attr, EnemyCtrl_43 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl43 = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy43_Action1(this),
			new Enemy43_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		preAction1 = false;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (preAction1)
		{
			return enemyActionArray[1];
		}
		if (Random.value < 0.5f)
		{
			preAction1 = true;
			return enemyActionArray[0];
		}
		return enemyActionArray[1];
	}

	private void SummorMonster43()
	{
		if (Singleton<EnemyController>.Instance.AllEnemies.Count < 5)
		{
			Singleton<EnemyController>.Instance.SummorMonster(new List<string>(1) { "Monster_43" }, new List<bool>(1) { true }, null);
		}
	}
}
