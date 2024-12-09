using System.Collections.Generic;
using UnityEngine;

public class Enemy_18 : EnemyBase
{
	private class Enemy18_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy18_Action1(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_18)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：沉默，造1次{realDmg}的非真实伤害({atkDes}), 给玩家施加1回合的沉默buff");
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
				Player player = Singleton<GameManager>.Instance.Player;
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
				player.GetBuff(new Buff_BrokenArmor(player, 1));
				player.GetBuff(new Buff_Shocked(player, 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy18_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy18_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_18)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：蓄力，所有友军+2力量");
			}
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy18_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy18_Action3(EnemyBase enemyBase)
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

		public override void OnLogic()
		{
			((EnemyCtrl_18)thisEnemy.EnemyCtrl).Action3Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：连续攻击，造3次{realDmg}的非真实伤害({atkDes})");
			}
		}

		protected override int GetBaseDmg()
		{
			return 3;
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

	private EnemyCtrl_18 enemy18Ctrl;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_18(EnemyAttr attr, EnemyCtrl_18 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy18Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy18_Action1(this),
			new Enemy18_Action2(this),
			new Enemy18_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		GetBuff(new Buff_SilenceRing(this, int.MaxValue));
	}

	protected override EnemyMean GetNextAction()
	{
		int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
		return enemyActionArray[num];
	}
}
