using UnityEngine;

public class Enemy_32 : EnemyBase
{
	private class Enemy32_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy32_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_32)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：沉睡，恢复20点生命值");
			}
		}

		private void Effect()
		{
			thisEnemy.EntityAttr.RecoveryHealth(20);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy32_Action2 : AtkEnemyMean
	{
		private string atkDes;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy32_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 4, string.Empty)
			});
		}

		protected override int GetBaseDmg()
		{
			return 2;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_32)thisEnemy.EnemyCtrl).Action2Anim(4, null, Effect, thisEnemy.NextAction);
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
				int num = EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (num > 0)
				{
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：喷溅，对玩家造成第{i + 1}次{realDmg}的非真实伤害({atkDes}),伤害未被抵挡给玩家施加1层致命毒药");
					}
					Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, 1));
				}
				else if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：喷溅，对玩家造成第{i + 1}次{realDmg}的非真实伤害({atkDes})");
				}
			}
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private EnemyCtrl_32 enemy32Ctrl;

	private bool isAwake;

	private Buff_PoisonSpread _buffPoisonSpread;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_32(EnemyAttr attr, EnemyCtrl_32 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy32Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy32_Action1(this),
			new Enemy32_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		Enemy32Sleep();
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffPoisonSpread = new Buff_PoisonSpread(this, 1);
		GetBuff(_buffPoisonSpread);
	}

	protected override EnemyMean GetNextAction()
	{
		if (!isAwake && (float)enemyAttr.Health / (float)enemyAttr.MaxHealth < 0.4f)
		{
			Enemy32Awake();
		}
		if (isAwake)
		{
			return enemyActionArray[1];
		}
		return enemyActionArray[0];
	}

	private void Enemy32Awake()
	{
		isAwake = true;
		enemy32Ctrl.EnemyAwake();
	}

	private void Enemy32Sleep()
	{
		isAwake = false;
		enemy32Ctrl.EnemySleep();
	}
}
