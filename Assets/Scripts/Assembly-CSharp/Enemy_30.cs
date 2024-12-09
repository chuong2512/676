using UnityEngine;

public class Enemy_30 : EnemyBase
{
	private class Enemy30_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy30_Action1(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：投石，对玩家造1次{realDmg}的非真实伤害({atkDes}),给玩家施加2层破甲buff");
			}
		}

		protected override int GetBaseDmg()
		{
			return 12;
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy30_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：蓄力，自身护甲+10");
			}
		}

		private void Effect()
		{
			thisEnemy.EntityAttr.AddArmor(10);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy30_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：蓄力攻击，对玩家造1次{realDmg}的非真实伤害({atkDes}),给自身施加1层破甲buff");
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
			thisEnemy.GetBuff(new Buff_BrokenArmor(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy30_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：昏迷，无任何行为");
			}
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy30_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：拿取武器，拾起大叔");
			}
		}

		private void Effect()
		{
			int num = 0;
			Buff_Heal buff_Heal;
			if ((buff_Heal = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Heal) as Buff_Heal) != null)
			{
				num = buff_Heal.BuffRemainRound;
			}
			if (num < 10)
			{
				thisEnemy.GetBuff(new Buff_Heal(thisEnemy, 10 - num));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action6 : AtkEnemyMean
	{
		private string atkDes;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy30_Action6(EnemyBase enemyBase)
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
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action6Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out atkDes);
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

	private class Enemy30_Action7 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy30_Action7(EnemyBase enemyBase)
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
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action7Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为7：强化蓄力，自身力量+2，获得2层治愈buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 2));
			thisEnemy.GetBuff(new Buff_Heal(thisEnemy, 2));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy30_Action8 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy30_Action8(EnemyBase enemyBase)
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
			return 40;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_30)thisEnemy.EnemyCtrl).Action8Anim(null, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				string atkDes;
				int num = RealDmg(out atkDes);
				int num2 = EnemyBase.EnemyAttackPlayer(num, thisEnemy, isTrue: false);
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (num2 > 0)
				{
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为8：猛锤，对玩家造1次{num}的非真实伤害({atkDes}),伤害未被抵挡，给玩家施加2层震荡buff和2层破甲buff");
					}
					Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
					Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
				}
				else if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为8：猛锤，对玩家造1次{num}的非真实伤害({atkDes})");
				}
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_30 enemy30Ctrl;

	private bool newLoop;

	private int newLoopOffset;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_30(EnemyAttr attr, EnemyCtrl_30 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy30Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[8]
		{
			new Enemy30_Action1(this),
			new Enemy30_Action2(this),
			new Enemy30_Action3(this),
			new Enemy30_Action4(this),
			new Enemy30_Action5(this),
			new Enemy30_Action6(this),
			new Enemy30_Action7(this),
			new Enemy30_Action8(this)
		};
	}

	public override void StartBattleAction()
	{
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (gameReportUI != null)
		{
			gameReportUI.AddGameReportContent(EntityName + "被动触发：获得1层治愈buff");
		}
		GetBuff(new Buff_Heal(this, 1));
		SetEnemyState(EnemyBase.EnemyOnBattleState);
		currentEnemyMean.OnLogic();
		if (currentEnemyMean.ItWillBreakDefence)
		{
			TryRemoveDefence();
		}
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		newLoop = false;
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_Heal(this, 3));
	}

	protected override EnemyMean GetNextAction()
	{
		if (newLoop)
		{
                switch ((Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount - newLoopOffset) % 4)
            {
				case 1 :return enemyActionArray[5]; 
				case 2 :return enemyActionArray[6]; 
				case 3 :return enemyActionArray[7]; 
				case 0 :return enemyActionArray[3];
                default: return null; 
			};
		}
		if ((float)enemyAttr.Health / (float)enemyAttr.MaxHealth >= 0.5f)
		{
            switch(Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4)
			{
				case 1 :return enemyActionArray[0]; 
				case 2 :return enemyActionArray[1]; 
				case 3 :return enemyActionArray[2]; 
				case 0 :return enemyActionArray[3]; 
				default :return null; 
			};
		}
		if (!currentEnemyMean.IsNull() && currentEnemyMean is Enemy30_Action3)
		{
			return enemyActionArray[3];
		}
		newLoop = true;
		newLoopOffset = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4;
		return enemyActionArray[4];
	}
}
