using UnityEngine;

public class Enemy_42 : EnemyBase
{
	private class Enemy42_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy42_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new StunMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_42)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy42_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy42_Action2(EnemyBase enemyBase)
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
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_42)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy42_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy42_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_42)thisEnemy.EnemyCtrl).Action3Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_42 enemy42Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_42(EnemyAttr attr, EnemyCtrl_42 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy42Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy42_Action1(this),
			new Enemy42_Action2(this),
			new Enemy42_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 2 == 1)
		{
			if (Random.value < 0.5f)
			{
				return enemyActionArray[1];
			}
			return enemyActionArray[2];
		}
		return enemyActionArray[0];
	}
}
