using System.Collections.Generic;
using UnityEngine;

public class Enemy_63 : EnemyBase
{
	private class Enemy63_Action1 : AtkEnemyMean
	{
		private int effectTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy63_Action1(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), GetAtkTime(), string.Empty)
			});
		}

		public override void ResetMean()
		{
			base.ResetMean();
			effectTime = 0;
		}

		private int GetAtkTime()
		{
			return effectTime + 2;
		}

		protected override int GetBaseDmg()
		{
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_63)thisEnemy.EnemyCtrl).Action1Anim(null, GetAtkTime(), Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：攻击，造成{GetAtkTime()}次{realDmg}的非真实伤害({atkDes}), 每次攻击附加1层流血");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Bleeding(Singleton<GameManager>.Instance.Player, 1));
			}
			if (i == effectTime + 1)
			{
				effectTime++;
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy63_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy63_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_63)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Heal(allEnemies[i], 4));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy63_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy63_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_63)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			int mainHandCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardAmount;
			int supHandCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardAmount;
			if (mainHandCardAmount > supHandCardAmount)
			{
				DropMain();
			}
			else if (mainHandCardAmount < supHandCardAmount)
			{
				DropSup();
			}
			else if (Random.value < 0.5f)
			{
				DropMain();
			}
			else
			{
				DropSup();
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}

		private void DropMain()
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearMainHandCards(null);
		}

		private void DropSup()
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ClearSupHandCards(null);
		}
	}

	private EnemyCtrl_63 _enemyCtrl63;

	private List<EnemyMean> allMeans;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_63(EnemyAttr attr, EnemyCtrl_63 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		_enemyCtrl63 = enemyCtrl;
		allMeans = new List<EnemyMean>(3);
		enemyActionArray = new EnemyMean[3]
		{
			new Enemy63_Action1(this),
			new Enemy63_Action2(this),
			new Enemy63_Action3(this)
		};
	}

	protected override void OnStartBattle()
	{
		ResetAllMeans();
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
	}

	private void ResetAllMeans()
	{
		allMeans.Clear();
		for (int i = 0; i < enemyActionArray.Length; i++)
		{
			allMeans.Add(enemyActionArray[i]);
		}
	}

	protected override EnemyMean GetNextAction()
	{
		if (allMeans.Count > 0)
		{
			return allMeans[Random.Range(0, allMeans.Count)];
		}
		ResetAllMeans();
		return enemyActionArray[0];
	}
}
