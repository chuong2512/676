using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : EnemyBase
{
	private class Enemy2_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy2_Action1(EnemyBase enemyBase)
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
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			Transform[] array = new Transform[allEnemies.Count];
			for (int i = 0; i < allEnemies.Count; i++)
			{
				array[i] = allEnemies[i].EnemyCtrl.transform;
			}
			((EnemyCtrl_2)thisEnemy.EnemyCtrl).Action1Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：鼓舞， 为所有己方目标+3护甲+1力量");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityAttr.AddArmor(3);
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy2_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy2_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 4)
			});
		}

		protected override int GetBaseDmg()
		{
			return 2;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_2)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：连续打击，造成4次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy2_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy2_Action3(EnemyBase enemyBase)
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
			return 7;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_2)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：重击，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_2 enemy2Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_2(EnemyAttr attr, EnemyCtrl_2 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy2Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy2_Action1(this),
			new Enemy2_Action2(this),
			new Enemy2_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount == 1)
		{
			return enemyActionArray[0];
		}
		float value = Random.value;
		if (Singleton<EnemyController>.Instance.AllEnemies.Count == 1)
		{
			if (value > 0.5f)
			{
				return enemyActionArray[1];
			}
			return enemyActionArray[2];
		}
		if (currentEnemyMean is Enemy2_Action1)
		{
			if (value > 0.5f)
			{
				return enemyActionArray[1];
			}
			return enemyActionArray[2];
		}
		if (value < 0.33f)
		{
			return enemyActionArray[0];
		}
		if (value < 0.66f)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[2];
	}
}
