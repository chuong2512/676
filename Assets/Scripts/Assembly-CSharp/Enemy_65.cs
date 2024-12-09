using System.Collections.Generic;
using UnityEngine;

public class Enemy_65 : EnemyBase
{
	private class Enemy65_Action1 : AtkEnemyMean
	{
		private int effectTime;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy65_Action1(EnemyBase enemyBase)
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
				new AttackMeanHandler(RealDmg(), 1)
			});
		}

		protected override int GetBaseDmg()
		{
			return 8 + effectTime * 2;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_65)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成?次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy65_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy65_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_65)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：");
			}
		}

		private void Effect()
		{
			for (int num = 2; num > 0; num--)
			{
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropHandCard();
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy65_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy65_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 2, string.Empty)
			});
		}

		protected override int GetBaseDmg()
		{
			return 4;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_65)thisEnemy.EnemyCtrl).Action3Anim(null, 2, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3，对玩家造成2次{realDmg}的非真实伤害({atkDes})");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Bleeding(Singleton<GameManager>.Instance.Player, 1));
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
			}
			if (i == 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private static int ActionStartIndex;

	private static bool IsEverSetEnemyStatus;

	private EnemyCtrl_65 enemy65Ctrl;

	private bool isCanGetHurt;

	private bool isHidingStatus;

	protected override EnemyMean[] enemyActionArray { get; }

	private static void SetEnemyStatus()
	{
		if (!IsEverSetEnemyStatus)
		{
			IsEverSetEnemyStatus = true;
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			int num = Random.Range(0, allEnemies.Count);
			for (int i = 0; i < allEnemies.Count; i++)
			{
				((Enemy_65)allEnemies[i]).SetIsCanGetHurt(i == num);
			}
		}
	}

	public Enemy_65(EnemyAttr attr, EnemyCtrl_65 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy65Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[3];
		InitEnemyActionArray();
	}

	protected override void OnStartBattle()
	{
		IsEverSetEnemyStatus = false;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyEndRound, OnEndEnemyRound);
		EventManager.RegisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
		GetBuff(new Buff_FancyOrReal(this, int.MaxValue));
	}

	private void OnEndEnemyRound(EventData data)
	{
		SetEnemyStatus();
	}

	private void OnPlayerRound(EventData data)
	{
		IsEverSetEnemyStatus = false;
	}

	public override void OnBattleReady()
	{
		base.OnBattleReady();
		SetEnemyStatus();
	}

	private void SetIsCanGetHurt(bool isCan)
	{
		isCanGetHurt = isCan;
		isHidingStatus = true;
		enemy65Ctrl.ClearStat();
	}

	private void SetRealStatus()
	{
		if (isHidingStatus)
		{
			isHidingStatus = false;
			enemy65Ctrl.SetCanGetHurtStat();
		}
	}

	public override int TakeDamage(int dmg, EntityBase caster, bool isAbsDmg)
	{
		if (!isCanGetHurt)
		{
			return 0;
		}
		SetRealStatus();
		return base.TakeDamage(dmg, caster, isAbsDmg);
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyEndRound, OnEndEnemyRound);
		EventManager.UnregisterEvent(EventEnum.E_PlayerRound, OnPlayerRound);
	}

	private void InitEnemyActionArray()
	{
		for (int i = 0; i < enemyActionArray.Length; i++)
		{
			int index = (i + ActionStartIndex) % 3;
			enemyActionArray[i] = GetEnemyMeanByIndex(index);
		}
		ActionStartIndex++;
	}

	private EnemyMean GetEnemyMeanByIndex(int index)
	{
        switch( index )
		{
			case 0 :return new Enemy65_Action1(this); 
			case 1 :return new Enemy65_Action2(this); 
			case 2 :return new Enemy65_Action3(this); 
			default :return null; 
		};
	}

	protected override EnemyMean GetNextAction()
	{
		return enemyActionArray[Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3];
	}
}
