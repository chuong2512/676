using System.Collections.Generic;
using UnityEngine;

public class Enemy_58 : EnemyBase
{
	private abstract class LogicHandler
	{
		public abstract EnemyMean GetNextAction();

		public abstract void InitHandler(Enemy_58 enemy58);
	}

	private class LogicHandlerA : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_58 enemy58;

		public LogicHandlerA(Enemy_58 enemy58)
		{
			this.enemy58 = enemy58;
		}

		public override void InitHandler(Enemy_58 enemy58)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy58.enemyActionArray[0]);
			allMeans.Enqueue(enemy58.enemyActionArray[1]);
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy58.SetNewHandler(this);
			}
			return result;
		}
	}

	private class LogicHandlerB : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_58 enemy58;

		public LogicHandlerB(Enemy_58 enemy58)
		{
			this.enemy58 = enemy58;
		}

		public override void InitHandler(Enemy_58 enemy58)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy58.enemyActionArray[4]);
			allMeans.Enqueue(enemy58.enemyActionArray[1]);
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy58.SetNewHandler(this);
			}
			return result;
		}
	}

	private class LogicHandlerC : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_58 enemy58;

		public LogicHandlerC(Enemy_58 enemy58)
		{
			this.enemy58 = enemy58;
		}

		public override void InitHandler(Enemy_58 enemy58)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy58.enemyActionArray[2]);
			allMeans.Enqueue(enemy58.enemyActionArray[1]);
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy58.SetNewHandler(this);
			}
			return result;
		}
	}

	private class Enemy58_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy58_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_58)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：");
			}
		}

		private void Effect()
		{
			Enemy_57 knightEnemy = ((Enemy_58)thisEnemy).knightEnemy;
			knightEnemy.EntityAttr.AddArmor(20);
			knightEnemy.GetBuff(new Buff_DefenceRestrik(knightEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy58_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy58_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_58)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：");
			}
		}

		private void Effect()
		{
			Enemy_57 knightEnemy = ((Enemy_58)thisEnemy).knightEnemy;
			knightEnemy.GetBuff(new Buff_KnightSpirit(knightEnemy, 1));
			thisEnemy.EntityAttr.RecoveryHealth(10);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy58_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy58_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_58)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：");
			}
		}

		private void Effect()
		{
			((Enemy_58)thisEnemy).knightEnemy.EntityAttr.RecoveryHealth(50);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy58_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy58_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_58)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			((Enemy_58)thisEnemy).ResetStat();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy58_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy58_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_58)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：");
			}
		}

		private void Effect()
		{
			Enemy_57 knightEnemy = ((Enemy_58)thisEnemy).knightEnemy;
			knightEnemy.GetBuff(new Buff_Sharp(knightEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_58 enemy58Ctrl;

	private Enemy_57 knightEnemy;

	private bool isEverGetHurt;

	private List<LogicHandler> allHandler;

	private LogicHandler currentHandler;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_58(EnemyAttr attr, EnemyCtrl_58 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy58Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[5]
		{
			new Enemy58_Action1(this),
			new Enemy58_Action2(this),
			new Enemy58_Action3(this),
			new Enemy58_Action4(this),
			new Enemy58_Action5(this)
		};
	}

	protected override void OnStartBattle()
	{
		isEverGetHurt = false;
		GetBuff(new Buff_Cowardly(this, int.MaxValue));
		allHandler = new List<LogicHandler>(3)
		{
			new LogicHandlerA(this),
			new LogicHandlerB(this),
			new LogicHandlerC(this)
		};
	}

	public void SetKnightEnemy(Enemy_57 knightEnemy)
	{
		this.knightEnemy = knightEnemy;
		int logicIndex = knightEnemy.LogicIndex;
		currentHandler = allHandler[logicIndex];
		allHandler.RemoveAt(logicIndex);
		currentHandler.InitHandler(this);
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		return currentHandler.GetNextAction();
	}

	public void ResetStat()
	{
		isEverGetHurt = false;
	}

	public void ForceToAction4()
	{
		if (!isEverGetHurt)
		{
			isEverGetHurt = true;
			currentEnemyMean = enemyActionArray[3];
			currentEnemyMean.OnSetMean();
			enemy58Ctrl.SetIdle1();
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("Buff_懦弱触发");
		}
	}

	private void SetNewHandler(LogicHandler handler)
	{
		int logicIndex = knightEnemy.LogicIndex;
		currentHandler = allHandler[logicIndex];
		allHandler.RemoveAt(logicIndex);
		currentHandler.InitHandler(this);
		allHandler.Add(handler);
	}
}
