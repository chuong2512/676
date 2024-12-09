using System.Collections.Generic;
using UnityEngine;

public class Enemy_5 : EnemyBase
{
	private class Enemy5_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy5_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 3)
			});
		}

		protected override int GetBaseDmg()
		{
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_5)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：连续撕裂，造成3次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 2)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy5_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy5_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new BuffMeanHandler()
			});
		}

		public override void OnLogic()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			List<EnemyBase> tmpEnemies = new List<EnemyBase>(allEnemies.Count - 1);
			List<Transform> list = new List<Transform>(allEnemies.Count - 1);
			for (int i = 0; i < allEnemies.Count; i++)
			{
				if (allEnemies[i] is Enemy_3)
				{
					tmpEnemies.Add(allEnemies[i]);
					list.Add(allEnemies[i].EnemyCtrl.transform);
				}
			}
			((EnemyCtrl_5)thisEnemy.EnemyCtrl).Action2Anim(list.ToArray(), delegate
			{
				Effect(tmpEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：疗伤，所有的恐狼恢复8点生命值");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityRecoveryHealthOnBattle(8);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_5 enemy5Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_5(EnemyAttr attr, EnemyCtrl_5 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy5Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy5_Action1(this),
			new Enemy5_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_WolfRelationship(this, int.MaxValue));
	}

	protected override EnemyMean GetNextAction()
	{
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		bool flag = false;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i].EnemyCode == "Monster_3" && allEnemies[i].EntityAttr.Health < 10)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (Random.value > 0.5f)
			{
				return enemyActionArray[0];
			}
			return enemyActionArray[1];
		}
		return enemyActionArray[0];
	}
}
