using System.Collections.Generic;
using UnityEngine;

public class Enemy_4 : EnemyBase
{
	private class Enemy4_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy4_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_4)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：狼嚎，对玩家施加2层震荡buff");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy4_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy4_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpecialMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_4)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：呼叫恐狼，召唤{((Enemy_4)thisEnemy).SummorAmount}只恐狼");
			}
		}

		private void Effect()
		{
			((Enemy_4)thisEnemy).SummorMonster3();
		}
	}

	private class Enemy4_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy4_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_4)thisEnemy.EnemyCtrl).Action3Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：低吼，所有己方目标+1力量");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy4_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy4_Action4(EnemyBase enemyBase)
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
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_4)thisEnemy.EnemyCtrl).Action4Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：咬，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_4 enemy4Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public int SummorAmount => 2 - Singleton<EnemyController>.Instance.GetSpecificMonsterAmount("Monster_3");

	public Enemy_4(EnemyAttr attr, EnemyCtrl_4 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy4Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy4_Action1(this),
			new Enemy4_Action2(this),
			new Enemy4_Action3(this),
			new Enemy4_Action4(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		float value = Random.value;
		if (Singleton<EnemyController>.Instance.GetSpecificMonsterAmount("Monster_3") >= 2)
		{
			if (value < 0.5f)
			{
				return enemyActionArray[2];
			}
			return enemyActionArray[3];
		}
		if (value < 0.5f)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[3];
	}

	protected void SummorMonster3()
	{
		int summorAmount = SummorAmount;
		List<string> list = new List<string>(summorAmount);
		List<bool> list2 = new List<bool>(summorAmount);
		for (int i = 0; i < summorAmount; i++)
		{
			list.Add("Monster_3");
			list2.Add(item: true);
		}
		Singleton<EnemyController>.Instance.SummorMonster(list, list2, delegate
		{
			SetEnemyState(EnemyBase.EnemyIdleState);
		});
	}
}
