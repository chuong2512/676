using UnityEngine;

public class Enemy_41 : EnemyBase
{
	private class Enemy41_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy41_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_41)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：增强，力量+3");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 3));
			thisEnemy.EntityAttr.AddArmor(40);
			((Enemy_41)thisEnemy).GetShieldAnim();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy41_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy41_Action2(EnemyBase enemyBase)
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
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_41)thisEnemy.EnemyCtrl).Action2Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：A面，造成3次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_41 enemy41Ctrl;

	private Buff_MultipleArmor buff;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_41(EnemyAttr attr, EnemyCtrl_41 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy41Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy41_Action1(this),
			new Enemy41_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		GetShieldAnim();
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		enemy41Ctrl.SetIdleAnim();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		buff = new Buff_MultipleArmor(this, int.MaxValue);
		GetBuff(buff);
	}

	public override void StartBattleAction()
	{
		SetEnemyState(EnemyBase.EnemyOnBattleState);
		currentEnemyMean.OnLogic();
		if (currentEnemyMean.ItWillBreakDefence)
		{
			TryRemoveDefence();
		}
	}

	public void OnArmorBroken()
	{
		enemy41Ctrl.LoseShield();
		buff.TakeEffect(this);
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	public void GetShieldAnim()
	{
		enemy41Ctrl.GetShiled(null);
	}

	protected override EnemyMean GetNextAction()
	{
		return enemyActionArray[1];
	}
}
