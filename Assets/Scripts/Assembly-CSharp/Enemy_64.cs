using System.Collections.Generic;
using UnityEngine;

public class Enemy_64 : EnemyBase
{
	private class Enemy64_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy64_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_64)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, -1));
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy64_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy64_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_64)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_PoisonSpread(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy64_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy64_Action3(EnemyBase enemyBase)
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
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_64)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造成{2}次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_64 _enemyCtrl64;

	private List<EnemyMean> allMeans;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_64(EnemyAttr attr, EnemyCtrl_64 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl64 = enemyCtrl;
		allMeans = new List<EnemyMean>(3);
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy64_Action1(this),
			new Enemy64_Action2(this),
			new Enemy64_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		ResetAllMeans();
		currentEnemyMean = enemyActionArray[2];
		currentEnemyMean.OnSetMean();
		GetBuff(new Buff_PoisonSpread(this, 1));
	}

	private void ResetAllMeans()
	{
		allMeans.Clear();
		for (int i = 0; i < enemyActionArray.Length; i++)
		{
			allMeans.Add(enemyActionArray[i]);
		}
	}

	protected override EnemyMean GetNextAction()
	{
		if (allMeans.Count > 0)
		{
			return allMeans[Random.Range(0, allMeans.Count)];
		}
		ResetAllMeans();
		return enemyActionArray[0];
	}
}
