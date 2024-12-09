using UnityEngine;

public class Enemy_19 : EnemyBase
{
	private class Enemy19_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy19_Action1(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_19)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造1次{realDmg}的非真实伤害({atkDes})");
			}
		}

		protected override int GetBaseDmg()
		{
			return 4;
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
			thisEnemy.GetBuff(new Buff_HoldPosition(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy19_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy19_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_19)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：攻击，对玩家造1次20点的真实伤害,自己自爆死亡");
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
				EnemyBase.EnemyAttackPlayer(20, thisEnemy, isTrue: true);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			thisEnemy.EntityAttr.ReduceHealth(thisEnemy.EntityAttr.Health);
		}
	}

	private EnemyCtrl_19 enemy19Ctrl;

	private int action1Time;

	private bool isLowHealth;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_19(EnemyAttr attr, EnemyCtrl_19 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy19Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy19_Action1(this),
			new Enemy19_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		action1Time = 1;
		isLowHealth = false;
	}

	protected override EnemyMean GetNextAction()
	{
		if ((float)enemyAttr.Health / (float)enemyAttr.MaxHealth <= 0.5f)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[0];
	}

	public override int TakeDamage(int dmg, EntityBase caster, bool isAbsDmg)
	{
		int result = base.TakeDamage(dmg, caster, isAbsDmg);
		if (enemyAttr.Health < 10 && !isLowHealth)
		{
			enemy19Ctrl.SetLowHealthIdleAnim();
			return result;
		}
		if (enemyAttr.Health >= 10 && isLowHealth)
		{
			enemy19Ctrl.SetNormalIdleAnim();
		}
		return result;
	}
}
