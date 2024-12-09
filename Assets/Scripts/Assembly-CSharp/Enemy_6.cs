using System.Collections.Generic;
using UnityEngine;

public class Enemy_6 : EnemyBase
{
	private class Enemy6_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy6_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_6)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：元素召唤，随机的元素");
			}
		}

		private void Effect()
		{
			((Enemy_6)thisEnemy).SummorElementEntity();
		}
	}

	private class Enemy6_Action2 : AtkEnemyMean
	{
		private const string Enemy6_Action2MeanDesKey = "Enemy6_Action2MeanDesKey";

		private Enemy_6 enemy6;

		private int castAmount;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy6_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
			enemy6 = (Enemy_6)enemyBase;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), enemy6.ElementAmount, GetMeanSpecialDes())
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), enemy6.ElementAmount, GetMeanSpecialDes())
			});
		}

		private string GetMeanSpecialDes()
		{
			return string.Format("Enemy6_Action2MeanDesKey".LocalizeText(), enemy6.ElementAmount);
		}

		protected override int GetBaseDmg()
		{
			return 6;
		}

		public override void OnLogic()
		{
			castAmount = ((Enemy_6)thisEnemy).ElementAmount;
			((Enemy_6)thisEnemy).RemoveAllMonster();
			((EnemyCtrl_6)thisEnemy.EnemyCtrl).Action2Anim(castAmount, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为2：元素发射，造成{castAmount}次{realDmg}的非真实伤害({atkDes})");
			}
		}

		private void Effect(int i)
		{
			if (castAmount == 0)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
				return;
			}
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
			}
			if (i == castAmount - 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy6_Action3 : EnemyMean
	{
		private Enemy_6 enemy6;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy6_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
			enemy6 = (Enemy_6)enemyBase;
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
			((EnemyCtrl_6)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < enemy6.AllElementEntities.Count; i++)
			{
				switch (enemy6.AllElementEntities[i].EnemyCode)
				{
				case "Monster_7":
					thisEnemy.GetBuff(new Buff_Power(thisEnemy, 2));
					num++;
					break;
				case "Monster_8":
					thisEnemy.EntityAttr.RecoveryHealth(5);
					num2 += 5;
					break;
				case "Monster_9":
					thisEnemy.EntityAttr.AddArmor(4);
					num3 += 4;
					break;
				}
			}
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：元素绽放，自身获得力量：{num},回复的生命值{num2}，自身获得的护甲值：{num3}");
			}
			((Enemy_6)thisEnemy).RemoveAllMonster();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_6 enemy6Ctrl;

	private List<ElementEnemy> allElementEntities = new List<ElementEnemy>();

	public List<ElementEnemy> AllElementEntities => allElementEntities;

	public int ElementAmount => allElementEntities.Count;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_6(EnemyAttr attr, EnemyCtrl_6 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy6Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy6_Action1(this),
			new Enemy6_Action2(this),
			new Enemy6_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		allElementEntities.Clear();
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		ElementEnemy item;
		if ((simpleEventData = data as SimpleEventData) != null && (item = simpleEventData.objValue as ElementEnemy) != null && allElementEntities.Contains(item))
		{
			allElementEntities.Remove(item);
			currentEnemyMean.ResetMean();
		}
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		if (allElementEntities.Count > 0)
		{
			for (int i = 0; i < allElementEntities.Count; i++)
			{
				allElementEntities[i].FadeDead();
			}
			allElementEntities.Clear();
		}
	}

	protected override EnemyMean GetNextAction()
	{
            switch (Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 4)
        {
			case 0 :return enemyActionArray[0]; 
			case 1 :return enemyActionArray[1]; 
			case 2 :return enemyActionArray[0]; 
			case 3 :return enemyActionArray[2]; 
			default :return null; 
		};
	}

	public void SummorElementEntity()
	{
		List<string> list = new List<string>(3);
		List<bool> actionFlag = new List<bool>(3) { false, false, false };
		for (int i = 0; i < 3; i++)
		{
			float value = Random.value;
			if (value < 0.33f)
			{
				list.Add("Monster_7");
			}
			else if (value < 0.66f)
			{
				list.Add("Monster_8");
			}
			else
			{
				list.Add("Monster_9");
			}
		}
		List<EnemyBase> list2 = Singleton<EnemyController>.Instance.SummorMonster(list, actionFlag, delegate
		{
			SetEnemyState(EnemyBase.EnemyIdleState);
		});
		for (int j = 0; j < list2.Count; j++)
		{
			allElementEntities.Add((ElementEnemy)list2[j]);
		}
	}

	public void RemoveAllMonster()
	{
		for (int i = 0; i < allElementEntities.Count; i++)
		{
			allElementEntities[i].FadeDead();
		}
		allElementEntities.Clear();
	}
}
