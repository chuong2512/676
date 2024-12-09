using System.Collections.Generic;
using UnityEngine;

public class Enemy_49 : EnemyBase
{
	private class Enemy49_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy49_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_49)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			Enemy_49 obj = (Enemy_49)thisEnemy;
			obj.SummorMainHand();
			obj.SummorSupHand();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy49_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy49_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_49)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 4));
				allEnemies[i].EntityAttr.AddArmor(10);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy49_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy49_Action3(EnemyBase enemyBase)
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
			return 30;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_49)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：致命一击，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy49_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy49_Action4(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new StunMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_49)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy49_Action5 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy49_Action5(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_49)thisEnemy.EnemyCtrl).Action5Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为5：攻击，造成3次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_49 _enemyCtrl49;

	private static readonly string[] MainHandSummorArray = new string[2] { "Monster_45", "Monster_47" };

	private static readonly string[] SupHandSummorArray = new string[3] { "Monster_44", "Monster_46", "Monster_48" };

	private EnemyBase MainHandMonster;

	private EnemyBase SupHandMonster;

	private int roundOffset;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_49(EnemyAttr attr, EnemyCtrl_49 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl49 = enemyCtrl;
		enemyActionArray = new EnemyMean[5]
		{
			new Enemy49_Action1(this),
			new Enemy49_Action2(this),
			new Enemy49_Action3(this),
			new Enemy49_Action4(this),
			new Enemy49_Action5(this)
		};
	}

	protected override void OnStartBattle()
	{
		roundOffset = 0;
		MainHandMonster = null;
		SupHandMonster = null;
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	protected override EnemyMean GetNextAction()
	{
		if (currentEnemyMean is Enemy49_Action3)
		{
			roundOffset++;
			return enemyActionArray[3];
		}
		if (MainHandMonster == null || SupHandMonster == null)
		{
			roundOffset++;
			return enemyActionArray[0];
		}
        switch ((Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount - roundOffset) % 3) 
		{
			case 0 :return enemyActionArray[1]; 
			case 1 :return enemyActionArray[4]; 
			case 2 :return enemyActionArray[2]; 
			default :return enemyActionArray[3]; 
		};
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		EnemyBase enemyBase;
		if ((simpleEventData = data as SimpleEventData) != null && (enemyBase = simpleEventData.objValue as EnemyBase) != null)
		{
			if (MainHandMonster != null && enemyBase.EnemyCode == MainHandMonster.EnemyCode)
			{
				MainHandMonster = null;
			}
			else if (SupHandMonster != null && enemyBase.EnemyCode == SupHandMonster.EnemyCode)
			{
				SupHandMonster = null;
			}
		}
	}

	private void SummorMainHand()
	{
		if (MainHandMonster == null)
		{
			MainHandMonster = Singleton<EnemyController>.Instance.SummorMonster(new List<string>(1) { MainHandSummorArray[Random.Range(0, MainHandSummorArray.Length)] }, new List<bool>(1) { true }, null)[0];
		}
	}

	private void SummorSupHand()
	{
		if (SupHandMonster == null)
		{
			SupHandMonster = Singleton<EnemyController>.Instance.SummorMonster(new List<string>(1) { SupHandSummorArray[Random.Range(0, SupHandSummorArray.Length)] }, new List<bool>(1) { true }, null)[0];
		}
	}
}
