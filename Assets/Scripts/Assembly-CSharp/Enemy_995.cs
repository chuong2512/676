using System.Collections.Generic;
using UnityEngine;

public class Enemy_995 : EnemyBase
{
	protected class Enemy995_Action1 : EnemyMean
	{
		private MeanHandler meanHandler;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy995_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			meanHandler = new BuffMeanHandler();
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1] { meanHandler });
		}

		public override void OnLogic()
		{
			((EnemyCtrl_995)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：进入防御状态");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Defence(thisEnemy, int.MaxValue));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy995_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy995_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 1)
			});
		}

		protected override int GetBaseDmg()
		{
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_995)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：造成1次{realDmg}的非真实伤害({atkDes})");
			}
		}

		private void Effect()
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy995_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy995_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_995)thisEnemy.EnemyCtrl).Action3Anim(2, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3，造成2次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_995 enemy995Ctrl;

	private List<EnemyMean> meanList;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_995(EnemyAttr attr, EnemyCtrl_995 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy995Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy995_Action1(this),
			new Enemy995_Action2(this),
			new Enemy995_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		enemy995Ctrl.IdleAnim();
		meanList = new List<EnemyMean>(3)
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[2]
		};
		int index = Random.Range(0, meanList.Count);
		currentEnemyMean = meanList[index];
		meanList.RemoveAt(index);
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		int index = Random.Range(0, meanList.Count);
		EnemyMean result = meanList[index];
		meanList.RemoveAt(index);
		meanList.Add(currentEnemyMean);
		return result;
	}
}
