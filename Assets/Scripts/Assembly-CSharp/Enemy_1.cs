using UnityEngine;

public class Enemy_1 : EnemyBase
{
	private class Enemy1_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy1_Action1(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_1)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：突刺，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy1_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy1_Action2(EnemyBase enemyBase)
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
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_1)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：破甲，造成1次{realDmg}的非真实伤害({atkDes})");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_1 enemy1Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_1(EnemyAttr attr, EnemyCtrl_1 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy1Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy1_Action1(this),
			new Enemy1_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (Random.value > 0.5f)
		{
			return enemyActionArray[0];
		}
		return enemyActionArray[1];
	}
}
