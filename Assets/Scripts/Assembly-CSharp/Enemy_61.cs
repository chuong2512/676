using UnityEngine;

public class Enemy_61 : EnemyBase
{
	private class Enemy61_Action1 : AtkEnemyMean
	{
		private int effectTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy61_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new AttackMeanHandler(RealDmg(), 1),
				new BuffMeanHandler()
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			effectTime = 0;
		}

		protected override int GetBaseDmg()
		{
			return effectTime + 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_61)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成1次{realDmg}的非真实伤害({atkDes}), 对自己施加一回合的神圣庇护");
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
			effectTime++;
			thisEnemy.GetBuff(new Buff_HolyProtect(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy61_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy61_Action2(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_61)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成1次{realDmg}的非真实伤害({atkDes}), 触发伤口效果");
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
				Buff_Wound buff_Wound;
				if ((buff_Wound = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Wound) as Buff_Wound) != null)
				{
					buff_Wound.TakeEffect(Singleton<GameManager>.Instance.Player);
				}
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Wound(Singleton<GameManager>.Instance.Player, 3));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy61_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy61_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new DeBuffMeanHandler()
			});
		}

		public override void OnLogic()
		{
			((EnemyCtrl_61)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect(int i)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropHandCard();
			if (i == 2)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private EnemyCtrl_61 _enemyCtrl61;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_61(EnemyAttr attr, EnemyCtrl_61 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl61 = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy61_Action1(this),
			new Enemy61_Action2(this),
			new Enemy61_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	protected override EnemyMean GetNextAction()
	{
		switch (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4)
		{
		case 0:
		case 2:
			return enemyActionArray[0];
		case 1:
			return enemyActionArray[1];
		case 3:
			return enemyActionArray[2];
		default:
			return enemyActionArray[0];
		}
	}
}
