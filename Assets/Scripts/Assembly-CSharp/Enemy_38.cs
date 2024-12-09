using System.Collections.Generic;
using UnityEngine;

public class Enemy_38 : EnemyBase
{
	private class Enemy38_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy38_Action1(EnemyBase enemyBase)
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
			EnemyBase target = allEnemies[0];
			for (int i = 1; i < allEnemies.Count; i++)
			{
				if (allEnemies[i].EntityAttr.Health < target.EntityAttr.Health)
				{
					target = allEnemies[i];
				}
			}
			((EnemyCtrl_38)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { target.EnemyCtrl.transform }, delegate
			{
				Effect(target);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：圣光，为生命值最少的一名己方对象+20护甲，施加1回合神圣庇护");
			}
		}

		private void Effect(EnemyBase target)
		{
			target.EntityAttr.AddArmor(5);
			target.GetBuff(new Buff_HolyProtect(target, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy38_Action2 : AtkEnemyMean
	{
		private int extraDmg;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy38_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
			extraDmg = 0;
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
			return 6 + extraDmg;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_38)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：神圣打击，造成1次{realDmg}的非真实伤害({atkDes})");
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
			extraDmg += 2;
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy38_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy38_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_38)thisEnemy.EnemyCtrl).Action3Anim(array, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityAttr.RecoveryHealth(40);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_38 enemy38Ctrl;

	private bool preAction1;

	private bool isMustAction2;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_38(EnemyAttr attr, EnemyCtrl_38 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy38Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy38_Action1(this),
			new Enemy38_Action2(this),
			new Enemy38_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		isMustAction2 = false;
		preAction1 = Random.value > 0.5f;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		BaseBuff baseBuff = new Buff_HighPriestProtect(this, int.MaxValue);
		GetBuff(baseBuff);
		baseBuff.TakeEffect(this);
	}

	protected override EnemyMean GetNextAction()
	{
		if (isMustAction2)
		{
			isMustAction2 = false;
			return enemyActionArray[1];
		}
		List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			if (allEnemies[i].EntityAttr.MaxHealth != allEnemies[i].EntityAttr.Health)
			{
				isMustAction2 = true;
				return enemyActionArray[2];
			}
		}
		if (preAction1)
		{
			preAction1 = false;
			return enemyActionArray[1];
		}
		preAction1 = true;
		return enemyActionArray[0];
	}
}
