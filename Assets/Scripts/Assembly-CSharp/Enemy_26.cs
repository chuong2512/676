using System.Collections.Generic;
using UnityEngine;

public class Enemy_26 : EnemyBase
{
	private class Enemy26_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy26_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_26)thisEnemy.EnemyCtrl).Action1Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：喘息，给所有己方目标恢复12点生命值, +1力量");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].EntityAttr.RecoveryHealth(12);
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy26_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy26_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_26)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：残喘，对玩家造1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_26 enemy26Ctrl;

	private Buff_HealBreath _buffHealBreath;

	private List<EnemyMean> allMeanList = new List<EnemyMean>(3);

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_26(EnemyAttr attr, EnemyCtrl_26 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy26Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy26_Action1(this),
			new Enemy26_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		ResetMean();
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffHealBreath = new Buff_HealBreath(this, int.MaxValue);
		GetBuff(_buffHealBreath);
	}

	protected override EnemyMean GetNextAction()
	{
		if (allMeanList.Count == 0)
		{
			ResetMean();
		}
		int index = Random.Range(0, allMeanList.Count);
		EnemyMean result = allMeanList[index];
		allMeanList.RemoveAt(index);
		return result;
	}

	private void ResetMean()
	{
		allMeanList.Clear();
		allMeanList.Add(enemyActionArray[0]);
		allMeanList.Add(enemyActionArray[1]);
		allMeanList.Add(enemyActionArray[1]);
	}

	protected override void OnEnemyDeadEffect()
	{
		base.OnEnemyDeadEffect();
		if (Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
		{
			_buffHealBreath.TakeEffect(this);
		}
	}
}
