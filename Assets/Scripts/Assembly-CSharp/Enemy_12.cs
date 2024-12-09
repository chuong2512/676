using System.Collections.Generic;
using UnityEngine;

public class Enemy_12 : EnemyBase
{
	private class Enemy12_Action1 : EnemyMean
	{
		private EnemyBase targetEnemy;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy12_Action1(EnemyBase enemyBase)
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

		public void SetTarget(EnemyBase enemyBase)
		{
			targetEnemy = enemyBase;
		}

		public override void OnLogic()
		{
			EnemyCtrl_12 obj = (EnemyCtrl_12)thisEnemy.EnemyCtrl;
			if (targetEnemy.IsDead)
			{
				targetEnemy = thisEnemy;
				Debug.Log("Reset target enemy : " + targetEnemy.EntityName);
			}
			obj.Action1Anim(new Transform[1] { targetEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：治愈，对一名友方目标施加4层治愈");
			}
		}

		private void Effect()
		{
			targetEnemy.GetBuff(new Buff_Heal(targetEnemy, 4));
			targetEnemy.GetBuff(new Buff_Power(targetEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy12_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy12_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_12)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：虚弱，使玩家的力量-1");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, -1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy12_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy12_Action3(EnemyBase enemyBase)
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
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_12)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：敲打，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_12 enemy12Ctrl;

	public bool isEverHeal;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_12(EnemyAttr attr, EnemyCtrl_12 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy12Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy12_Action1(this),
			new Enemy12_Action2(this),
			new Enemy12_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (Singleton<EnemyController>.Instance.AllEnemies.Count == 1)
		{
			return enemyActionArray[2];
		}
		if (!isEverHeal)
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				if (allEnemies[i].EntityAttr.Health < allEnemies[i].EntityAttr.MaxHealth)
				{
					((Enemy12_Action1)enemyActionArray[0]).SetTarget(allEnemies[i]);
					isEverHeal = true;
					return enemyActionArray[0];
				}
			}
			return enemyActionArray[2];
		}
		isEverHeal = false;
		if (Random.value < 0.5f)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[2];
	}

	public override void NextAction()
	{
		enemyCtrl.ShowEnemyMean();
		currentEnemyMean = ((SingletonDontDestroy<Game>.Instance.isTest && enemyCtrl.actionIndex > 0) ? enemyActionArray[enemyCtrl.actionIndex - 1] : GetNextAction());
		currentEnemyMean.OnSetMean();
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}
}
