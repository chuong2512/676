public class Enemy_998 : EnemyBase
{
	protected class Enemy998_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy998_Action1(EnemyBase enemyBase)
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
			realDmg = RealDmg(out var _);
			((EnemyCtrl_998)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
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

		protected override int GetBaseDmg()
		{
			return 2;
		}
	}

	private EnemyCtrl_998 enemy998Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_998(EnemyAttr attr, EnemyCtrl_998 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy998Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[1]
		{
			new Enemy998_Action1(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		return enemyActionArray[0];
	}
}
