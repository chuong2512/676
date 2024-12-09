using UnityEngine;

public class Enemy_35 : EnemyBase
{
	private class Enemy35_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy35_Action1(EnemyBase enemyBase)
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
			return 15;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：右攻击，造成1次{realDmg}的非真实伤害({atkDes}),并使玩家丢弃所有的主手手牌");
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
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearMainHandCards(null);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy35_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy35_Action2(EnemyBase enemyBase)
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
			return 15;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：左攻击，造成1次{realDmg}的非真实伤害({atkDes}),并使玩家丢弃所有的副手手牌");
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
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearSupHandCards(null);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy35_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy35_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：咆哮，对玩家施加3层破甲buff，自身获得3层招架buff");
			}
		}

		private void Effect()
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 3));
			thisEnemy.GetBuff(new Buff_HoldPosition(thisEnemy, 3));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy35_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy35_Action4(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 4)
			});
		}

		protected override int GetBaseDmg()
		{
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action4Anim(4, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：乱舞，造成4次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy35_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy35_Action5(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new StunMeanHandler()
			});
			((Enemy_35)thisEnemy).enemy35Ctrl.StunIdle();
		}

		public override void OnLogic()
		{
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy35_Action6 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy35_Action6(EnemyBase enemyBase)
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
			((EnemyCtrl_35)thisEnemy.EnemyCtrl).Action6Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为6：愤怒，为自己施加一层愤怒");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Angry(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_35 enemy35Ctrl;

	private bool isEverLowHealth;

	private int battleRoundOffset;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_35(EnemyAttr attr, EnemyCtrl_35 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy35Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[6]
		{
			new Enemy35_Action1(this),
			new Enemy35_Action2(this),
			new Enemy35_Action3(this),
			new Enemy35_Action4(this),
			new Enemy35_Action5(this),
			new Enemy35_Action6(this)
		};
	}

	protected override void OnStartBattle()
	{
		isEverLowHealth = false;
		currentEnemyMean = enemyActionArray[4];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		if (isEverLowHealth)
		{
			switch ((Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount - battleRoundOffset) % 3)
			{
			case 0:
				return enemyActionArray[2];
			case 1:
				if (Random.value < 0.5f)
				{
					return enemyActionArray[0];
				}
				return enemyActionArray[1];
			case 2:
				return enemyActionArray[3];
			}
		}
		if ((float)enemyAttr.Health / (float)enemyAttr.MaxHealth < 0.5f)
		{
			isEverLowHealth = true;
			battleRoundOffset = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
			return enemyActionArray[5];
		}
		switch (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4)
		{
		case 1:
			return enemyActionArray[2];
		case 2:
			if (Random.value < 0.5f)
			{
				return enemyActionArray[0];
			}
			return enemyActionArray[1];
		case 3:
			return enemyActionArray[3];
		case 0:
			return enemyActionArray[4];
		default:
			return null;
		}
	}
}
