using UnityEngine;

public class Enemy_17 : EnemyBase
{
	private class Enemy17_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy17_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_17)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：守，为自己施加{4}层反伤buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_DamageRestrik(thisEnemy, 4));
			thisEnemy.EntityAttr.AddArmor(8);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy17_Action2 : AtkEnemyMean
	{
		private int atkDmg;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy17_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
			atkDmg = 3;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 3)
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			atkDmg = 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_17)thisEnemy.EnemyCtrl).Action2Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：连击，造3次{realDmg}的非真实伤害({atkDes})");
			}
		}

		protected override int GetBaseDmg()
		{
			return atkDmg;
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
				atkDmg++;
				BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_DamageRestrik);
				if (!specificBuff.IsNull())
				{
					thisEnemy.RemoveBuff(specificBuff);
				}
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy17_Action3 : AtkEnemyMean
	{
		private int armorAmount;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy17_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
			armorAmount = 5;
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
			armorAmount = 5;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_17)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造1次{realDmg}的非真实伤害({atkDes}), 自身护甲+{armorAmount},移除自己的反伤buff");
			}
		}

		protected override int GetBaseDmg()
		{
			return 5;
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
			thisEnemy.EntityAttr.AddArmor(armorAmount);
			armorAmount++;
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_DamageRestrik);
			if (!specificBuff.IsNull())
			{
				thisEnemy.RemoveBuff(specificBuff);
			}
		}
	}

	private EnemyCtrl_17 enemy17Ctrl;

	private bool isEverActon3;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_17(EnemyAttr attr, EnemyCtrl_17 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy17Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy17_Action1(this),
			new Enemy17_Action2(this),
			new Enemy17_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[2];
		currentEnemyMean.OnSetMean();
		isEverActon3 = true;
	}

	protected override EnemyMean GetNextAction()
	{
		if (enemyAttr.Armor > 0)
		{
			if (isEverActon3)
			{
				isEverActon3 = false;
				return enemyActionArray[0];
			}
			isEverActon3 = true;
			return enemyActionArray[2];
		}
		return enemyActionArray[1];
	}
}
