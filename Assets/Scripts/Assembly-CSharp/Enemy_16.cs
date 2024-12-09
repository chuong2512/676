using System.Collections.Generic;
using UnityEngine;

public class Enemy_16 : EnemyBase
{
	private class Enemy16_Action4 : EnemyMean
	{
		private bool isFirst;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy16_Action4(EnemyBase enemyBase)
			: base(enemyBase)
		{
			isFirst = false;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpecialMeanHandler()
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			isFirst = false;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_16)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：召唤狼群，召唤{((Enemy_16)thisEnemy).SummorAmount}只恐狼");
			}
		}

		private void Effect()
		{
			((Enemy_16)thisEnemy).SummorEnemy(isFirst ? 2 : 3);
		}
	}

	private class Enemy16_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy16_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_16)thisEnemy.EnemyCtrl).Action5Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：低吼，所有己方的目标+2力量");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy16_Action6 : AtkEnemyMean
	{
		private int executeAmount;

		private int extraDmg;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy16_Action6(EnemyBase enemyBase)
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

		public override void ResetMean()
		{
			base.ResetMean();
			executeAmount = 0;
			extraDmg = 0;
		}

		protected override int GetBaseDmg()
		{
			return 8 + extraDmg;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_16)thisEnemy.EnemyCtrl).Action6Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为6：啃咬，造1次{realDmg}的非真实伤害({atkDes})");
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
			executeAmount++;
			if (executeAmount == 2)
			{
				extraDmg += 3;
				executeAmount = 0;
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy16_Action7 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy16_Action7(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new BuffMeanHandler(),
				new DeBuffMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_16)thisEnemy.EnemyCtrl).Action7Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为7：刺耳嚎叫，自身+3力量，并对玩家施加2层破甲buff和2层震荡buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 3));
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_16 enemy16Ctrl;

	private List<EnemyBase> allSummorList = new List<EnemyBase>();

	private bool isEnemy15DeadLastRound;

	private bool isEnemy15Dead;

	private bool isSecondLogicChain;

	private bool isPreAction6;

	private EnemyMean nextMustAction;

	protected override EnemyMean[] enemyActionArray { get; }

	public int SummorAmount => 3 - allSummorList.Count;

	public Enemy_16(EnemyAttr attr, EnemyCtrl_16 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy16Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy16_Action4(this),
			new Enemy16_Action5(this),
			new Enemy16_Action6(this),
			new Enemy16_Action7(this)
		};
	}

	protected override void OnStartBattle()
	{
		isSecondLogicChain = false;
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, EnemyDead);
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_WolfRelationship(this, int.MaxValue));
	}

	private void EnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		if ((simpleEventData = data as SimpleEventData) != null)
		{
			Enemy_3 item;
			if ((item = simpleEventData.objValue as Enemy_3) != null && allSummorList.Contains(item))
			{
				allSummorList.Remove(item);
			}
			else if (simpleEventData.objValue is Enemy_15)
			{
				isEnemy15Dead = true;
				isEnemy15DeadLastRound = true;
			}
		}
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, EnemyDead);
	}

	protected override EnemyMean GetNextAction()
	{
		if (isEnemy15DeadLastRound)
		{
			isEnemy15DeadLastRound = false;
			return enemyActionArray[3];
		}
		if (isEnemy15Dead)
		{
			return enemyActionArray[2];
		}
		if (isSecondLogicChain)
		{
			if (nextMustAction != null)
			{
				EnemyMean result = nextMustAction;
				nextMustAction = null;
				return result;
			}
			if (allSummorList.Count <= 1)
			{
				isPreAction6 = false;
				nextMustAction = enemyActionArray[1];
				return enemyActionArray[0];
			}
			if (isPreAction6)
			{
				isPreAction6 = false;
				return enemyActionArray[1];
			}
			isPreAction6 = true;
			return enemyActionArray[2];
		}
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount == 2)
		{
			isSecondLogicChain = true;
		}
		return enemyActionArray[num];
	}

	public void SummorEnemy(int amount)
	{
		if (allSummorList.Count < amount)
		{
			int summorAmount = SummorAmount;
			List<string> list = new List<string>(summorAmount);
			List<bool> list2 = new List<bool>(summorAmount);
			for (int i = 0; i < summorAmount; i++)
			{
				list.Add("Monster_3");
				list2.Add(item: true);
			}
			List<EnemyBase> list3 = Singleton<EnemyController>.Instance.SummorMonster(list, list2, delegate
			{
				SetEnemyState(EnemyBase.EnemyIdleState);
			});
			for (int j = 0; j < list3.Count; j++)
			{
				allSummorList.Add(list3[j]);
			}
		}
	}
}
