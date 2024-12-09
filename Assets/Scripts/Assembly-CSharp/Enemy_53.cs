using UnityEngine;

public class Enemy_53 : EnemyBase
{
	private class Enemy53_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy53_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_53)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1，进入防御状态，并拥有1回合的格挡反击");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Defence(thisEnemy, int.MaxValue));
			thisEnemy.GetBuff(new Buff_DefenceRestrik(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy53_Action2 : AtkEnemyMean
	{
		private int effectTime;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy53_Action2(EnemyBase enemyBase)
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

		public override void ResetMean()
		{
			base.ResetMean();
			effectTime = 0;
		}

		protected override int GetBaseDmg()
		{
			return 20 + effectTime * 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_53)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：攻击，造成1次{realDmg}的非真实伤害({atkDes}), 自身获得弃盾效果");
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
			thisEnemy.GetBuff(new Buff_ThrowShield(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy53_Action3 : AtkEnemyMean
	{
		private const string SpecialAtkDesKey = "Enemy53_Action3SpecialAtkDesKey";

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy53_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_53)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
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

	private class Enemy53_Action4 : EnemyMean
	{
		private int realArmor;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy53_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_53)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			realArmor = GetFaith();
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：圣甲术");
			}
		}

		private void Effect()
		{
			if (realArmor > 0)
			{
				thisEnemy.EntityAttr.AddArmor(realArmor);
			}
			((Buff_Faith)Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_Faith))?.ClearFaith();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_53 _enemyCtrl53;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_53(EnemyAttr attr, EnemyCtrl_53 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl53 = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy53_Action1(this),
			new Enemy53_Action2(this),
			new Enemy53_Action3(this),
			new Enemy53_Action4(this)
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
