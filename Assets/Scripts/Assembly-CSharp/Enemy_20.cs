using System.Collections.Generic;
using UnityEngine;

public class Enemy_20 : EnemyBase
{
	private class Enemy20_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy20_Action1(EnemyBase enemyBase)
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
			List<Enemy_21> list = new List<Enemy_21>();
			Enemy_21 target = null;
			for (int i = 0; i < Singleton<EnemyController>.Instance.AllEnemies.Count; i++)
			{
				Enemy_21 item;
				if ((item = Singleton<EnemyController>.Instance.AllEnemies[i] as Enemy_21) != null)
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				target = list[Random.Range(0, list.Count)];
			}
			((EnemyCtrl_20)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { target.EnemyCtrl.transform }, delegate
			{
				Effect(target);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：祝福，使一个邪神之子的护甲+5，力量+1");
			}
		}

		private void Effect(Enemy_21 target)
		{
			if (!target.IsNull())
			{
				target.BlessedByEvilGod();
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy20_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy20_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_20)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：诅咒，对玩家施加2层震荡buff");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_20 enemy20Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_20(EnemyAttr attr, EnemyCtrl_20 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy20Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy20_Action1(this),
			new Enemy20_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_LifeConnect(this, int.MaxValue));
	}

	protected override EnemyMean GetNextAction()
	{
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 2 == 1)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[0];
	}
}
