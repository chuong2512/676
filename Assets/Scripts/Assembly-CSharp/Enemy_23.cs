using UnityEngine;

public class Enemy_23 : EnemyBase
{
	private class Enemy23_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy23_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_23)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：昏迷，无任何行为");
			}
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy23_Action2 : AtkEnemyMean
	{
		private const string Enemy23_ActionSpecialDesKey = "Enemy23_ActionSpecialDesKey";

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy23_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 1, "Enemy23_ActionSpecialDesKey".LocalizeText())
			});
		}

		protected override int GetBaseDmg()
		{
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_23)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：粉碎攻击，对玩家造1次{realDmg}的非真实伤害({atkDes}),并对自己造成10点真实伤害");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 3));
			}
			thisEnemy.TakeDamage(10, null, isAbsDmg: true);
			thisEnemy.GetBuff(new Buff_BrokenArmor(thisEnemy, 3));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy23_Action3 : AtkEnemyMean
	{
		private const string Enemy23_ActionSpecialDesKey = "Enemy23_ActionSpecialDesKey";

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy23_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 3, "Enemy23_ActionSpecialDesKey".LocalizeText())
			});
		}

		protected override int GetBaseDmg()
		{
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_23)thisEnemy.EnemyCtrl).Action3Anim(3, null, Effect, thisEnemy.NextAction);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：连续碎击，对玩家造3次{realDmg}的非真实伤害({atkDes}),每次攻击会对自己造成10点真实伤害");
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
			thisEnemy.TakeDamage(10, null, isAbsDmg: true);
			if (i == 2)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy23_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy23_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_23)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：守护，为自己施加40护甲");
			}
		}

		private void Effect()
		{
			thisEnemy.EntityAttr.AddArmor(40);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_23 enemy23Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_23(EnemyAttr attr, EnemyCtrl_23 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy23Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy23_Action1(this),
			new Enemy23_Action2(this),
			new Enemy23_Action3(this),
			new Enemy23_Action4(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4;
		return enemyActionArray[num];
	}
}
