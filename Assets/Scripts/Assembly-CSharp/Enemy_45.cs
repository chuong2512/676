using System.Collections.Generic;
using UnityEngine;

public class Enemy_45 : EnemyBase
{
	private class Enemy45_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy45_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 3)
			});
		}

		protected override int GetBaseDmg()
		{
			return 7;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_45)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成3次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 2)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy45_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy45_Action2(EnemyBase enemyBase)
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

		protected override int GetBaseDmg()
		{
			return 16;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_45)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成1次{realDmg}的非真实伤害({atkDes})");
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
			thisEnemy.GetBuff(new Buff_HoldPosition(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy45_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy45_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_45)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 3));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_45 _enemyCtrl45;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_45(EnemyAttr attr, EnemyCtrl_45 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl45 = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy45_Action1(this),
			new Enemy45_Action2(this),
			new Enemy45_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
		return enemyActionArray[num];
	}
}
