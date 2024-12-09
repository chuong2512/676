using System.Collections.Generic;
using UnityEngine;

public class Enemy_22 : EnemyBase
{
	private class Enemy22_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy22_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new AttackMeanHandler(RealDmg(), 1),
				new BuffMeanHandler()
			});
		}

		protected override int GetBaseDmg()
		{
			return 12;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_22)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				realDmg = RealDmg(out var atkDes);
				int num = EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
				if (num > 0)
				{
					GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：吸血，造1次{realDmg}的非真实伤害({atkDes}), 伤害未被抵挡，为自身恢复{num}生命值");
					}
					thisEnemy.EntityAttr.RecoveryHealth(num);
				}
				else
				{
					GameReportUI gameReportUI2 = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
					if (gameReportUI2 != null)
					{
						gameReportUI2.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：吸血，造1次{realDmg}的非真实伤害({atkDes})");
					}
				}
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy22_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy22_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 2, string.Empty)
			});
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			realDmg = RealDmg();
			((EnemyCtrl_22)thisEnemy.EnemyCtrl).Action2Anim(2, null, Effect, thisEnemy.NextAction);
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, 1));
			}
			if (i == 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy22_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy22_Action3(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_22)thisEnemy.EnemyCtrl).Action3Anim(null, 3, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造3次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_22 enemy22Ctrl;

	private List<EnemyMean> allActions;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_22(EnemyAttr attr, EnemyCtrl_22 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy22Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy22_Action1(this),
			new Enemy22_Action2(this),
			new Enemy22_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		allActions = new List<EnemyMean>
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[2]
		};
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		EnemyMean enemyMean = currentEnemyMean;
		int index = Random.Range(0, allActions.Count);
		EnemyMean result = allActions[index];
		allActions.RemoveAt(index);
		if (!enemyMean.IsNull())
		{
			allActions.Add(enemyMean);
		}
		return result;
	}
}
