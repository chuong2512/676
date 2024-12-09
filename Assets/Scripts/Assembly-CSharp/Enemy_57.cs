using System.Collections.Generic;
using UnityEngine;

public class Enemy_57 : EnemyBase
{
	private abstract class LogicHandler
	{
		public abstract EnemyMean GetNextAction();

		public abstract void InitHandler(Enemy_57 enemy57, bool isChildDead);
	}

	private class LogicHandlerA : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_57 enemy57;

		public LogicHandlerA(Enemy_57 enemy57)
		{
			this.enemy57 = enemy57;
		}

		public override void InitHandler(Enemy_57 enemy57, bool isChildDead)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy57.enemyActionArray[1]);
			allMeans.Enqueue(enemy57.enemyActionArray[2]);
			if (isChildDead)
			{
				allMeans.Enqueue(enemy57.enemyActionArray[5]);
			}
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy57.SetNewHandler(this);
			}
			return result;
		}
	}

	private class LogicHandlerB : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_57 enemy57;

		public LogicHandlerB(Enemy_57 enemy57)
		{
			this.enemy57 = enemy57;
		}

		public override void InitHandler(Enemy_57 enemy57, bool isChildDead)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy57.enemyActionArray[3]);
			allMeans.Enqueue(enemy57.enemyActionArray[4]);
			if (isChildDead)
			{
				allMeans.Enqueue(enemy57.enemyActionArray[5]);
			}
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy57.SetNewHandler(this);
			}
			return result;
		}
	}

	private class LogicHandlerC : LogicHandler
	{
		private Queue<EnemyMean> allMeans;

		private Enemy_57 enemy57;

		public LogicHandlerC(Enemy_57 enemy57)
		{
			this.enemy57 = enemy57;
		}

		public override void InitHandler(Enemy_57 enemy57, bool isChildDead)
		{
			allMeans = new Queue<EnemyMean>();
			allMeans.Enqueue(enemy57.enemyActionArray[0]);
			allMeans.Enqueue(enemy57.enemyActionArray[0]);
			if (isChildDead)
			{
				allMeans.Enqueue(enemy57.enemyActionArray[5]);
			}
		}

		public override EnemyMean GetNextAction()
		{
			EnemyMean result = allMeans.Dequeue();
			if (allMeans.Count == 0)
			{
				enemy57.SetNewHandler(this);
			}
			return result;
		}
	}

	private class Enemy57_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy57_Action1(EnemyBase enemyBase)
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
			return 15;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy57_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy57_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：");
			}
		}

		private void Effect()
		{
			((EnemyAttr_57)thisEnemy.EntityAttr).AddBlock(1);
			thisEnemy.GetBuff(new Buff_Defence(thisEnemy, 1));
			((Enemy_57)thisEnemy).AddArmorTOEnemy58();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy57_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy57_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 1, string.Empty)
			});
		}

		protected override int GetBaseDmg()
		{
			return ((EnemyAttr)thisEnemy.EntityAttr).Block;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成1次{realDmg}的非真实伤害({atkDes})");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy57_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy57_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 2));
			thisEnemy.GetBuff(new Buff_KnightSpirit(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy57_Action5 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy57_Action5(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 4)
			});
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action5Anim(null, 4, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成?次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy57_Action6 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy57_Action6(EnemyBase enemyBase)
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
			return 15;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_57)thisEnemy.EnemyCtrl).Action6Anim(null, 2, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成2次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_57 enemy57Ctrl;

	private Enemy_58 _enemy58;

	private List<LogicHandler> allHandler;

	private LogicHandler currentHandler;

	public int LogicIndex { get; private set; }

	protected override EnemyMean[] enemyActionArray { get; }

	private bool isChildDead => _enemy58.IsDead;

	public Enemy_57(EnemyAttr attr, EnemyCtrl_57 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy57Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[6]
		{
			new Enemy57_Action1(this),
			new Enemy57_Action2(this),
			new Enemy57_Action3(this),
			new Enemy57_Action4(this),
			new Enemy57_Action5(this),
			new Enemy57_Action6(this)
		};
	}

	protected override void OnStartBattle()
	{
		GetBuff(new Buff_KnightSpirit(this, 4));
		allHandler = new List<LogicHandler>(3)
		{
			new LogicHandlerA(this),
			new LogicHandlerB(this),
			new LogicHandlerC(this)
		};
		LogicIndex = Random.Range(0, allHandler.Count);
		currentHandler = allHandler[LogicIndex];
		allHandler.RemoveAt(LogicIndex);
		currentHandler.InitHandler(this, isChildDead: false);
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		_enemy58 = (Enemy_58)Singleton<EnemyController>.Instance.AddMonster("Monster_58", actionFlag: false);
		_enemy58.SetKnightEnemy(this);
	}

	protected override EnemyMean GetNextAction()
	{
		return currentHandler.GetNextAction();
	}

	public void AddArmorTOEnemy58()
	{
		if (!isChildDead)
		{
			_enemy58.EntityAttr.AddArmor(20);
		}
	}

	public override void Dead()
	{
		base.Dead();
		if (!_enemy58.IsDead)
		{
			_enemy58.Dead();
		}
	}

	private void SetNewHandler(LogicHandler handler)
	{
		LogicIndex = Random.Range(0, allHandler.Count);
		currentHandler = allHandler[LogicIndex];
		allHandler.RemoveAt(LogicIndex);
		currentHandler.InitHandler(this, isChildDead);
		allHandler.Add(handler);
	}
}
