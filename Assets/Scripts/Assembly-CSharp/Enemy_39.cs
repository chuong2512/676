using System.Collections.Generic;
using UnityEngine;

public class Enemy_39 : EnemyBase
{
	private class Enemy39_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy39_Action1(EnemyBase enemyBase)
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
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_39)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：寒冰之触，造成1次{realDmg}的非真实伤害({atkDes}),并施加1层冻结buff");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Freeze(Singleton<GameManager>.Instance.Player, 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy39_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy39_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_39)thisEnemy.EnemyCtrl).Action2Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：寒冰之墙，为所有友方对象+5层掩护");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Cover(allEnemies[i], 5));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy39_Action3 : AtkEnemyMean
	{
		private int atkTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy39_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
			atkTime = 2;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), atkTime)
			});
		}

		protected override int GetBaseDmg()
		{
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_39)thisEnemy.EnemyCtrl).Action3Anim(atkTime, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：冰锥术，造成{atkTime}次{realDmg}的非真实伤害({atkDes})");
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
			if (i == atkTime - 1)
			{
				atkTime++;
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private EnemyCtrl_39 enemy39Ctrl;

	private List<EnemyMean> allActions;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_39(EnemyAttr attr, EnemyCtrl_39 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy39Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy39_Action1(this),
			new Enemy39_Action2(this),
			new Enemy39_Action3(this)
		};
		allActions = new List<EnemyMean>
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[2]
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
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
}
