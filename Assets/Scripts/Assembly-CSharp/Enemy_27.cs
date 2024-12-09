using System.Collections.Generic;
using UnityEngine;

public class Enemy_27 : EnemyBase
{
	private class Enemy27_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy27_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_27)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：咆哮, 给所有目标+8护甲");
			}
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityAttr.AddArmor(8);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy27_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy27_Action2(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_27)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：粉碎，对玩家造1次{realDmg}的非真实伤害({atkDes})，并给玩家施加1层破甲buff");
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

	private class Enemy27_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy27_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_27)thisEnemy.EnemyCtrl).Action3Anim(2, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：连续攻击，对玩家造2次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_27 enemy27Ctrl;

	private Buff_DeadProtect _buffDeadProtect;

	private List<EnemyMean> allActions;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_27(EnemyAttr attr, EnemyCtrl_27 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy27Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy27_Action1(this),
			new Enemy27_Action2(this),
			new Enemy27_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		allActions = new List<EnemyMean>
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[2]
		};
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffDeadProtect = new Buff_DeadProtect(this, int.MaxValue);
		GetBuff(_buffDeadProtect);
	}

	protected override EnemyMean GetNextAction()
	{
		EnemyMean enemyMean = currentEnemyMean;
		int index = Random.Range(0, allActions.Count);
		EnemyMean result = allActions[index];
		allActions.RemoveAt(index);
		if (!enemyMean.IsNull())
		{
			allActions.Add(enemyMean);
		}
		return result;
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffDeadProtect.TakeEffect(this);
		}
	}
}
