using System.Collections.Generic;
using UnityEngine;

public class Enemy_34 : EnemyBase
{
	private class Enemy34_Action1 : AtkEnemyMean
	{
		private int castTime;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy34_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
			castTime = 0;
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
			castTime = 0;
		}

		protected override int GetBaseDmg()
		{
			return 12 + castTime * 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_34)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：精准攻击，对玩家造1次{realDmg}的非真实伤害({atkDes})");
			}
		}

		private void Effect()
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else if (EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false) > 0 && !thisEnemy.IsDead)
			{
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent(thisEnemy.EntityName + "被动触发，随机丢弃一张玩家手牌");
				}
				Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_FastAttack).TakeEffect(thisEnemy);
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy34_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy34_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_34)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：防御，获得防御buff进入防御状态并且获得1次格挡反击");
			}
		}

		private void Effect()
		{
			((Enemy_34)thisEnemy).isDefence = true;
			thisEnemy.GetBuff(new Buff_Defence(thisEnemy, 999));
			thisEnemy.GetBuff(new Buff_DefenceRestrik(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy34_Action3 : AtkEnemyMean
	{
		private int castTime;

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy34_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
			castTime = 0;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 4)
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			castTime = 0;
		}

		protected override int GetBaseDmg()
		{
			return 4 + castTime;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_34)thisEnemy.EnemyCtrl).Action3Anim(4, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：快速攻击，对玩家造4次{realDmg}的非真实伤害({atkDes})");
			}
		}

		private void Effect(int i)
		{
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else if (EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false) > 0 && Singleton<GameManager>.Instance.BattleSystem.IsInBattle)
			{
				GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
				if (gameReportUI != null)
				{
					gameReportUI.AddGameReportContent(thisEnemy.EntityName + "被动触发，随机丢弃一张玩家手牌");
				}
				Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_FastAttack).TakeEffect(thisEnemy);
			}
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private EnemyCtrl_34 enemy34Ctrl;

	private bool isDefence;

	private List<EnemyMean> allActions;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_34(EnemyAttr attr, EnemyCtrl_34 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy34Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy34_Action1(this),
			new Enemy34_Action2(this),
			new Enemy34_Action3(this)
		};
		allActions = new List<EnemyMean>
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[2]
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		enemy34Ctrl.IdleAnim();
		isDefence = false;
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_FastAttack(this, int.MaxValue));
	}

	protected override EnemyMean GetNextAction()
	{
		EnemyMean enemyMean = currentEnemyMean;
		int index = Random.Range(0, allActions.Count);
		EnemyMean result = allActions[index];
		allActions.RemoveAt(index);
		if (!enemyMean.IsNull())
		{
			allActions.Add(enemyMean);
		}
		return result;
	}
}
