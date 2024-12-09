using UnityEngine;

public class Enemy_48 : EnemyBase
{
	private class Enemy48_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy48_Action1(EnemyBase enemyBase)
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
			return 12;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_48)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成1次{realDmg}的非真实伤害({atkDes}), 并施加2层中毒");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_DeadPoison(Singleton<GameManager>.Instance.Player, 2));
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy48_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy48_Action2(EnemyBase enemyBase)
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
			return 16;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_48)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成1次{realDmg}的非真实伤害({atkDes}), 并施加1层躲闪");
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
			thisEnemy.GetBuff(new Buff_Dodge(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy48_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy48_Action3(EnemyBase enemyBase)
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
			return 7;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_48)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造成3次{realDmg}的非真实伤害({atkDes})");
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

	private EnemyCtrl_48 _enemyCtrl48;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_48(EnemyAttr attr, EnemyCtrl_48 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl48 = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy48_Action1(this),
			new Enemy48_Action2(this),
			new Enemy48_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[Random.Range(0, 2)];
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
            switch(Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 2)
		{
            case 0:return enemyActionArray[Random.Range(0, 2)]; 
			case 1:
                return enemyActionArray[2]; 
			default: return enemyActionArray[0]; 
		};
	}
}
