using UnityEngine;

public class Enemy_33 : EnemyBase
{
	private class Enemy33_Action1 : EnemyMean
	{
		private int castTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy33_Action1(EnemyBase enemyBase)
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

		public override void ResetMean()
		{
			base.ResetMean();
			castTime = 0;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_33)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：防御，为自己+30护甲");
			}
		}

		private void Effect()
		{
			thisEnemy.EntityAttr.AddArmor(30 + castTime * 5);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy33_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy33_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_33)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			int armor = thisEnemy.EntityAttr.Armor;
			thisEnemy.EntityAttr.ReduceArmor(thisEnemy.EntityAttr.Armor);
			thisEnemy.EntityAttr.RecoveryHealth(armor);
			((Enemy_33)thisEnemy).SetPreArmorGet(armor);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy33_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy33_Action3(EnemyBase enemyBase)
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
			return ((Enemy_33)thisEnemy).preArmorGet;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_33)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：击晕，对玩家造1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_33 enemy33Ctrl;

	private int preArmorGet;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_33(EnemyAttr attr, EnemyCtrl_33 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy33Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy33_Action1(this),
			new Enemy33_Action2(this),
			new Enemy33_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
		return enemyActionArray[num];
	}

	protected override void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
		base.OnEntityGetHurtOnBattle(healthDmg, armorDmg, isAbsDmg);
		if (currentEnemyMean is Enemy33_Action2)
		{
			currentEnemyMean.UpdateMean();
		}
	}

	public void SetPreArmorGet(int armor)
	{
		preArmorGet = armor;
	}
}
