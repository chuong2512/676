using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_55 : EnemyBase
{
	private class Enemy55_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy55_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_55)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：");
			}
		}

		private void Effect()
		{
			Enemy_55 obj = (Enemy_55)thisEnemy;
			int buffRound = (obj.isLowHealthLoop ? 1 : 2);
			obj.SummorMonster56(1, buffRound);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy55_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy55_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_55)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：");
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

	private class Enemy55_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy55_Action3(EnemyBase enemyBase)
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
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_55)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成1次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy55_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy55_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_55)thisEnemy.EnemyCtrl).Action4Anim(null, 3, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成3次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy55_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy55_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_55)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：");
			}
		}

		private void Effect()
		{
			((Enemy_55)thisEnemy).SummorMonster56(2, 2);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_55 enemy55Ctrl;

	private List<Enemy_56> _enemy56List;

	private Func<EnemyMean> currentLogic;

	private int lowHealthRoundOffset;

	private bool isLowHealthLoop;

	private float healthRate => (float)enemyAttr.Health / (float)enemyAttr.MaxHealth;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_55(EnemyAttr attr, EnemyCtrl_55 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy55Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[5]
		{
			new Enemy55_Action1(this),
			new Enemy55_Action2(this),
			new Enemy55_Action3(this),
			new Enemy55_Action4(this),
			new Enemy55_Action5(this)
		};
	}

	protected override void OnStartBattle()
	{
		isLowHealthLoop = false;
		_enemy56List = new List<Enemy_56>();
		currentLogic = HighHealthLogic;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		Enemy_56 item;
		if ((simpleEventData = data as SimpleEventData) != null && (item = simpleEventData.objValue as Enemy_56) != null)
		{
			_enemy56List.Remove(item);
		}
	}

	private void SummorMonster56(int amount, int buffRound)
	{
		for (int i = 0; i < amount; i++)
		{
			Enemy_56 enemy_ = Singleton<EnemyController>.Instance.AddMonster("Monster_56", actionFlag: true) as Enemy_56;
			enemy_.SetBuffRound(buffRound);
			_enemy56List.Add(enemy_);
		}
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		if (_enemy56List != null)
		{
			for (int i = 0; i < _enemy56List.Count; i++)
			{
				_enemy56List[i].Dead();
			}
		}
	}

	protected override EnemyMean GetNextAction()
	{
		return currentLogic();
	}

	private EnemyMean HighHealthLogic()
	{
		if (healthRate > 0.5f)
		{
            switch (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3)
			{
				case 0 :return enemyActionArray[0]; 
				case 1 :return enemyActionArray[1]; 
				case 2 :return enemyActionArray[3]; 
				default :return enemyActionArray[0]; 
			};
		}
		return PreLowHealthLogicA();
	}

	private EnemyMean PreLowHealthLogicA()
	{
		isLowHealthLoop = true;
		currentLogic = PreLowHealthLogicB;
		return enemyActionArray[4];
	}

	private EnemyMean PreLowHealthLogicB()
	{
		lowHealthRoundOffset = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
		currentLogic = LowHealthLogic;
		return enemyActionArray[1];
	}

	private EnemyMean LowHealthLogic()
	{
        switch ((Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount - lowHealthRoundOffset) % 3)
		{
			case 0 :return enemyActionArray[2]; 
			case 1 :return enemyActionArray[3]; 
			case 2 :return enemyActionArray[0]; 
			default :return enemyActionArray[0]; 
		};
	}
}
