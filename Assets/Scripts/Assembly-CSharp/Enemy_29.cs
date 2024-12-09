using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_29 : EnemyBase
{
	private class Enemy29_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy29_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_29)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：呼唤，随机召唤2只梦魇怪");
			}
		}

		private void Effect()
		{
			((Enemy_29)thisEnemy).SummorRandomEnemy();
		}
	}

	private class Enemy29_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy29_Action2(EnemyBase enemyBase)
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
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			Transform[] array = new Transform[allEnemies.Count];
			for (int i = 0; i < allEnemies.Count; i++)
			{
				array[i] = allEnemies[i].EnemyCtrl.transform;
			}
			((EnemyCtrl_29)thisEnemy.EnemyCtrl).Action2Anim(array, delegate
			{
				Effect(allEnemies);
			}, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：增强，所有己方目标力量+2");
			}
		}

		private void Effect(List<EnemyBase> allEnemies)
		{
			for (int i = 0; i < allEnemies.Count; i++)
			{
				EnemyBase enemyBase = allEnemies[i];
				enemyBase.GetBuff(new Buff_Power(enemyBase, 2));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy29_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy29_Action3(EnemyBase enemyBase)
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
			return 3;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_29)thisEnemy.EnemyCtrl).Action3Anim(3, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：进攻，对玩家造3次{realDmg}的非真实伤害({atkDes})");
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

	private class Enemy29_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy29_Action4(EnemyBase enemyBase)
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
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_29)thisEnemy.EnemyCtrl).Action4Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：禁言，对玩家造1次{realDmg}的非真实伤害({atkDes}),给玩家施加1层沉默buff");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Silence(Singleton<GameManager>.Instance.Player, 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_29 enemy29Ctrl;

	private readonly List<string> allSummoredList;

	private List<EnemyBase> allSummoredEntities = new List<EnemyBase>();

	private bool loop;

	private bool isMustAction2;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_29(EnemyAttr attr, EnemyCtrl_29 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		allSummoredList = new List<string> { "Monster_24", "Monster_25", "Monster_26", "Monster_27" };
		enemy29Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[4]
		{
			new Enemy29_Action1(this),
			new Enemy29_Action2(this),
			new Enemy29_Action3(this),
			new Enemy29_Action4(this)
		};
	}

	protected override void OnStartBattle()
	{
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		loop = false;
		isMustAction2 = false;
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		EnemyBase item;
		if ((simpleEventData = data as SimpleEventData) != null && (item = simpleEventData.objValue as EnemyBase) != null && allSummoredEntities.Contains(item))
		{
			allSummoredEntities.Remove(item);
		}
	}

	protected override EnemyMean GetNextAction()
	{
		if (!loop)
		{
			loop = true;
			return enemyActionArray[1];
		}
		return LoopLogic();
	}

	private EnemyMean LoopLogic()
	{
		if (isMustAction2)
		{
			isMustAction2 = false;
			return enemyActionArray[1];
		}
		if (allSummoredEntities.Count > 0)
		{
			int num = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 2;
			return enemyActionArray[2 + num];
		}
		isMustAction2 = true;
		return enemyActionArray[0];
	}

	public void SummorRandomEnemy()
	{
		List<EnemyBase> list = Singleton<EnemyController>.Instance.SummorMonster(allSummoredList.RandomFromList(2).ToList(), new List<bool>(2) { true, true }, delegate
		{
			SetEnemyState(EnemyBase.EnemyIdleState);
		});
		for (int i = 0; i < list.Count; i++)
		{
			allSummoredEntities.Add(list[i]);
		}
	}
}
