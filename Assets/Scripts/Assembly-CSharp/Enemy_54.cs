using UnityEngine;

public class Enemy_54 : EnemyBase
{
	private class Enemy54_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy54_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 2)
			});
		}

		protected override int GetBaseDmg()
		{
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_54)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成2次{realDmg}的非真实伤害({atkDes})");
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
				int value = EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
				((Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith)).AddFaith(value);
			}
			if (i == 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy54_Action2 : EnemyMean
	{
		private int effectTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy54_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void ResetMean()
		{
			base.ResetMean();
			effectTime = 0;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new BuffMeanHandler()
			});
		}

		private int PwdAmount()
		{
			return 1 + effectTime;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_54)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, PwdAmount()));
			effectTime++;
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy54_Action3 : AtkEnemyMean
	{
		private const string SpecialAtkDesKey = "Enemy53_Action3SpecialAtkDesKey";

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy54_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 1, GetSpecialAtkDes())
			});
		}

		private string GetSpecialAtkDes()
		{
			return string.Format("Enemy53_Action3SpecialAtkDesKey".LocalizeText(), GetFaith());
		}

		protected override int GetBaseDmg()
		{
			return GetFaith();
		}

		private int GetFaith()
		{
			Buff_Faith buff_Faith = (Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith);
			int result = 0;
			if (buff_Faith != null)
			{
				result = buff_Faith.FaithAmount;
			}
			return result;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_54)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：攻击，造成1次{realDmg}的非真实伤害({atkDes})");
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
			((Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith))?.ClearFaith();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy54_Action4 : EnemyMean
	{
		private int realHeal;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy54_Action4(EnemyBase enemyBase)
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

		private int GetFaith()
		{
			Buff_Faith buff_Faith = (Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith);
			int result = 0;
			if (buff_Faith != null)
			{
				result = buff_Faith.FaithAmount;
			}
			return result;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_54)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realHeal = GetFaith();
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：圣甲术");
			}
		}

		private void Effect()
		{
			if (realHeal > 0)
			{
				thisEnemy.GetBuff(new Buff_Heal(thisEnemy, realHeal));
			}
			((Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith))?.ClearFaith();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_54 _enemyCtrl54;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_54(EnemyAttr attr, EnemyCtrl_54 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl54 = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy54_Action1(this),
			new Enemy54_Action2(this),
			new Enemy54_Action3(this),
			new Enemy54_Action4(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_Faith(this));
	}

	protected override EnemyMean GetNextAction()
	{
        switch (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 6) 
		{
			case 0 :return enemyActionArray[0]; 
			case 1 :return enemyActionArray[1]; 
			case 2 :return enemyActionArray[2]; 
			case 3 :return enemyActionArray[0]; 
			case 4 :return enemyActionArray[1]; 
			case 5 :return enemyActionArray[3]; 
			default :return null; 
		};
	}
}
