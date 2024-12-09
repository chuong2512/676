using System;
using UnityEngine;

public class Enemy_25 : EnemyBase
{
	private class Enemy25_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy25_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_25)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：呢喃，给玩家施加1层沉默buff");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Silence(Singleton<GameManager>.Instance.Player, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy25_Action2 : AtkEnemyMean
	{
		private int atkTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy25_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			atkTime = UnityEngine.Random.Range(2, 5);
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), atkTime)
			});
		}

		public override void UpdateMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), atkTime)
			});
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_25)thisEnemy.EnemyCtrl).Action2Anim(atkTime, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，对玩家造{atkTime}次{realDmg}的非真实伤害({atkDes})");
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
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy25_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy25_Action3(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_25)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，对玩家造1次{realDmg}的非真实伤害({atkDes})");
			}
		}

		protected override int GetBaseDmg()
		{
			return 8;
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

	private EnemyCtrl_25 enemy25Ctrl;

	private Buff_DeadSilence _buffDeadSilence;

	private Func<EnemyMean> handler;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_25(EnemyAttr attr, EnemyCtrl_25 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy25Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy25_Action1(this),
			new Enemy25_Action2(this),
			new Enemy25_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		handler = LogicA;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffDeadSilence = new Buff_DeadSilence(this, int.MaxValue);
		GetBuff(_buffDeadSilence);
	}

	protected override EnemyMean GetNextAction()
	{
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
		handler = LogicD;
		if (!((double)UnityEngine.Random.value > 0.5))
		{
			return enemyActionArray[2];
		}
		return enemyActionArray[1];
	}

	private EnemyMean LogicD()
	{
		handler = LogicA;
		return enemyActionArray[0];
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffDeadSilence.TakeEffect(this);
		}
	}
}
