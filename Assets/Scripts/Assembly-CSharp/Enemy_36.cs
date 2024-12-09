using UnityEngine;

public class Enemy_36 : EnemyBase
{
	private class Enemy36_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy36_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_36)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1，立即让玩家丢弃所有的牌");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearMainHandCards(null);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearSupHandCards(null);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy36_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy36_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_36)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2，给玩家施加2层致命毒药");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy36_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy36_Action3(EnemyBase enemyBase)
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

		protected override int GetBaseDmg()
		{
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_36)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3，给玩家造成1次{realDmg}非真实伤害({atkDes})");
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

	private EnemyCtrl_36 enemy36Ctrl;

	private Buff_TimePunishment _buffTimePunishment;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_36(EnemyAttr attr, EnemyCtrl_36 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy36Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy36_Action1(this),
			new Enemy36_Action2(this),
			new Enemy36_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffTimePunishment = new Buff_TimePunishment(this, int.MaxValue);
		GetBuff(_buffTimePunishment);
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffTimePunishment.TakeEffect(this);
			_buffTimePunishment = null;
		}
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Random.Range(0, 3);
		return enemyActionArray[num];
	}
}
