public class Enemy_40 : EnemyBase
{
	private class Enemy40_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy40_Action1(EnemyBase enemyBase)
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
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_40)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：A面，造成1次{realDmg}的非真实伤害({atkDes}),并施加2层破甲buff");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy40_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy40_Action2(EnemyBase enemyBase)
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
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_40)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：B面，造成1次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_40 enemy40Ctrl;

	private bool isSideA;

	private int switchTime;

	private Buff_DoubleSide _buffDoubleSide;

	protected override EnemyMean[] enemyActionArray { get; }

	public bool IsSideA => isSideA;

	public int SwitchTime => switchTime;

	public Enemy_40(EnemyAttr attr, EnemyCtrl_40 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy40Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[2]
		{
			new Enemy40_Action1(this),
			new Enemy40_Action2(this)
		};
	}

	protected override void OnStartBattle()
	{
		switchTime = 0;
		isSideA = true;
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		enemy40Ctrl.SetIdleAnim();
	}

	public void ResetEnemySwitchTime()
	{
		switchTime = 0;
		_buffDoubleSide.UpdateBuffHint();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		_buffDoubleSide = new Buff_DoubleSide(this, int.MaxValue);
		GetBuff(_buffDoubleSide);
		isSideA = true;
	}

	public override void StartBattleAction()
	{
		_buffDoubleSide.TakeEffect(this);
		SetEnemyState(EnemyBase.EnemyOnBattleState);
		currentEnemyMean.OnLogic();
		if (currentEnemyMean.ItWillBreakDefence)
		{
			TryRemoveDefence();
		}
	}

	protected override void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
		base.OnEntityGetHurtOnBattle(healthDmg, armorDmg, isAbsDmg);
		if (isSideA)
		{
			enemy40Ctrl.AtoBAnim();
			isSideA = false;
		}
		else
		{
			enemy40Ctrl.BtoAAnim();
			isSideA = true;
		}
		switchTime++;
		_buffDoubleSide.UpdateBuffHint();
	}

	protected override EnemyMean GetNextAction()
	{
		if (isSideA)
		{
			return enemyActionArray[0];
		}
		return enemyActionArray[1];
	}
}
