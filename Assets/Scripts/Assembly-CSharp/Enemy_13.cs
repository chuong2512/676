using System;
using UnityEngine;

public class Enemy_13 : EnemyBase
{
	private class Enemy13_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy13_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new AttackMeanHandler(RealDmg(), 1),
				new DeBuffMeanHandler()
			});
		}

		protected override int GetBaseDmg()
		{
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_13)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：破甲，造1次{realDmg}的非真实伤害({atkDes}),每次攻击附带1层破甲buff");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy13_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy13_Action2(EnemyBase enemyBase)
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
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_13)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：乱舞，造1次{realDmg}的非真实伤害({atkDes}),为自己施加1层躲闪buff");
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
			thisEnemy.GetBuff(new Buff_Dodge(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy13_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy13_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_13)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：致命毒药，为自身+8点力量，恢复10点生命值，为自身施加5层致命毒药buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 8));
			thisEnemy.EntityRecoveryHealthOnBattle(10);
			thisEnemy.GetBuff(new Buff_DeadPoison(thisEnemy, 5));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy13_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy13_Action4(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_13)thisEnemy.EnemyCtrl).Action4Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：乱舞，造3次{realDmg}的非真实伤害({atkDes})");
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

		protected override int GetBaseDmg()
		{
			return 3;
		}
	}

	private EnemyCtrl_13 enemy13Ctrl;

	private Func<EnemyMean> logicFunc;

	private bool isAction1;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_13(EnemyAttr attr, EnemyCtrl_13 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy13Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy13_Action1(this),
			new Enemy13_Action2(this),
			new Enemy13_Action3(this),
			new Enemy13_Action4(this)
		};
	}

	protected override void OnStartBattle()
	{
		isAction1 = true;
		logicFunc = NormalLogic;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		return logicFunc();
	}

	private EnemyMean NormalLogic()
	{
		if ((double)((float)enemyAttr.Health / (float)enemyAttr.MaxHealth) <= 0.3)
		{
			logicFunc = LowHealthLogic;
			return enemyActionArray[2];
		}
		if (isAction1)
		{
			isAction1 = false;
			return enemyActionArray[(!(UnityEngine.Random.value > 0.5f)) ? 1 : 0];
		}
		isAction1 = true;
		return enemyActionArray[3];
	}

	private EnemyMean LowHealthLogic()
	{
		return enemyActionArray[0];
	}
}
