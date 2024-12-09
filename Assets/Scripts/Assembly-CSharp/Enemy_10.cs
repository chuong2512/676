using System.Collections.Generic;
using UnityEngine;

public class Enemy_10 : EnemyBase
{
	private class Enemy10_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy10_Action1(EnemyBase enemyBase)
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
				array[i] = allEnemies[i].EntityTransform;
			}
			((EnemyCtrl_10)thisEnemy.EnemyCtrl).Action1Anim(array, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：守护，为所有目标+2掩护");
			}
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Cover(allEnemies[i], 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy10_Action2 : AtkEnemyMean
	{
		private int castTime;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy10_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
			castTime = 0;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new AttackMeanHandler(RealDmg(), 1),
				new BuffMeanHandler()
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			castTime = 0;
		}

		protected override int GetBaseDmg()
		{
			return 5 + castTime * 2;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_10)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻+守，造成1次{realDmg}的非真实伤害({atkDes})，并且使自己+3护甲");
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
			thisEnemy.EntityAttr.AddArmor(5 + castTime * 2);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_10 enemy10Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_10(EnemyAttr attr, EnemyCtrl_10 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy10Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy10_Action1(this),
			new Enemy10_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (Singleton<EnemyController>.Instance.AllEnemies.Count == 0)
		{
			return enemyActionArray[1];
		}
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 2;
		return enemyActionArray[num];
	}
}
