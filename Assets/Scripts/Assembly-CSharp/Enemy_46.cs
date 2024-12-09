using System.Collections.Generic;
using UnityEngine;

public class Enemy_46 : EnemyBase
{
	private class Enemy46_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy46_Action1(EnemyBase enemyBase)
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
			EnemyCtrl_46 enemyCtrl_ = (EnemyCtrl_46)thisEnemy.EnemyCtrl;
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			Transform[] array = new Transform[allEnemies.Count];
			for (int i = 0; i < allEnemies.Count; i++)
			{
				array[i] = allEnemies[i].EntityTransform;
			}
			enemyCtrl_.Action1Anim(array, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityAttr.AddArmor(20);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy46_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy46_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_46)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Silence(Singleton<GameManager>.Instance.Player, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy46_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy46_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_46)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_46 _enemyCtrl46;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_46(EnemyAttr attr, EnemyCtrl_46 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl46 = enemyCtrl;
		Enemy46_Action3 enemy46_Action = new Enemy46_Action3(this);
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy46_Action1(this),
			enemy46_Action,
			new Enemy46_Action2(this),
			enemy46_Action
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4;
		return enemyActionArray[num];
	}
}
