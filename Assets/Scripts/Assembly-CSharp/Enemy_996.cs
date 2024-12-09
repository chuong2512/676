using UnityEngine;

public class Enemy_996 : EnemyBase
{
	protected class Enemy996_Action1 : EnemyMean
	{
		private MeanHandler meanHandler;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy996_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_996)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Defence(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_996 enemy996Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_996(EnemyAttr attr, EnemyCtrl_996 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy996Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[1]
		{
			new Enemy996_Action1(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		GetBuff(new Buff_Defence(this, 1));
	}

	protected override EnemyMean GetNextAction()
	{
		return enemyActionArray[0];
	}
}
