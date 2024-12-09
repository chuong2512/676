using System;
using UnityEngine;

public class Enemy_15 : EnemyBase
{
	private class Enemy15_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy15_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new DeBuffMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_15)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：狼嚎，为玩家施加2层震荡buff");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy15_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy15_Action2(EnemyBase enemyBase)
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
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_15)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：撕碎，造1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy15_Action3 : AtkEnemyMean
	{
		private int atkTime = 3;

		private int useCounter;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy15_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), atkTime)
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			atkTime = 3;
			useCounter = 0;
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_15)thisEnemy.EnemyCtrl).Action3Anim(atkTime, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：疯狂撕咬，造{atkTime}次{realDmg}的非真实伤害({atkDes})");
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
			if (i == atkTime - 1)
			{
				useCounter++;
				if (useCounter == 2)
				{
					atkTime++;
					useCounter = 0;
				}
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy15_Action7 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy15_Action7(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new BuffMeanHandler(),
				new DeBuffMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_15)thisEnemy.EnemyCtrl).Action7Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为7：刺耳嚎叫，自身+3力量，并对玩家施加2层破甲buff和2层震荡buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 3));
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_15 enemy15Ctrl;

	private bool isEnemy16DeadLastRound;

	private bool isEnemy16Dead;

	private Func<EnemyMean> handler;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_15(EnemyAttr attr, EnemyCtrl_15 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy15Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy15_Action1(this),
			new Enemy15_Action2(this),
			new Enemy15_Action3(this),
			new Enemy15_Action7(this)
		};
	}

	protected override void OnStartBattle()
	{
		handler = LogicB;
		currentEnemyMean = enemyActionArray[1];
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.objValue is Enemy_16)
		{
			isEnemy16DeadLastRound = true;
			isEnemy16Dead = true;
		}
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_WolfRelationship(this, int.MaxValue));
	}

	protected override EnemyMean GetNextAction()
	{
		if (isEnemy16DeadLastRound)
		{
			isEnemy16DeadLastRound = false;
			return enemyActionArray[3];
		}
		if (isEnemy16Dead)
		{
			return enemyActionArray[2];
		}
		return handler();
	}

	private EnemyMean LogicA()
	{
		handler = LogicB;
		return enemyActionArray[1];
	}

	private EnemyMean LogicB()
	{
		handler = LogicC;
		return enemyActionArray[2];
	}

	private EnemyMean LogicC()
	{
		handler = LogicA;
		return enemyActionArray[0];
	}
}
