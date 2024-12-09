using System.Collections.Generic;
using UnityEngine;

public class Enemy_51 : EnemyBase
{
	private abstract class LogicHandler
	{
		protected Enemy_51 leftHand;

		public LogicHandler(Enemy_51 leftHand)
		{
			this.leftHand = leftHand;
		}

		public abstract EnemyMean GetNextAction();
	}

	private class LogicP1 : LogicHandler
	{
		private Queue<EnemyMean> allActionsQueue = new Queue<EnemyMean>();

		public LogicP1(Enemy_51 leftHand)
			: base(leftHand)
		{
		}

		public override EnemyMean GetNextAction()
		{
			if (allActionsQueue.Count == 0)
			{
				InitHandler();
			}
			return allActionsQueue.Dequeue();
		}

		private void InitHandler()
		{
			allActionsQueue.Enqueue(leftHand.enemyActionArray[1]);
			allActionsQueue.Enqueue(leftHand.enemyActionArray[2]);
			allActionsQueue.Enqueue(leftHand.enemyActionArray[0]);
		}
	}

	private class LogicPreP2 : LogicHandler
	{
		public LogicPreP2(Enemy_51 leftHand)
			: base(leftHand)
		{
		}

		public override EnemyMean GetNextAction()
		{
			leftHand.SetCurrentHandler(new LogicP2(leftHand));
			return leftHand.enemyActionArray[4];
		}
	}

	private class LogicP2 : LogicHandler
	{
		private EnemyMean preEnemyMean;

		private List<EnemyMean> allActonsList = new List<EnemyMean>();

		public LogicP2(Enemy_51 leftHand)
			: base(leftHand)
		{
			allActonsList.Add(leftHand.enemyActionArray[0]);
			allActonsList.Add(leftHand.enemyActionArray[1]);
			allActonsList.Add(leftHand.enemyActionArray[3]);
			preEnemyMean = null;
		}

		public override EnemyMean GetNextAction()
		{
			int index = Random.Range(0, allActonsList.Count);
			EnemyMean result = allActonsList[index];
			allActonsList.RemoveAt(index);
			if (preEnemyMean != null)
			{
				allActonsList.Add(preEnemyMean);
			}
			preEnemyMean = result;
			return result;
		}
	}

	private class Enemy51_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy51_Action1(EnemyBase enemyBase)
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

		protected override int GetBaseDmg()
		{
			return 20 + (((Enemy_51)thisEnemy).isGetSpike ? 10 : 0);
		}

		public override void OnLogic()
		{
			((EnemyCtrl_51)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成1次{realDmg}的非真实伤害({atkDes})");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Wound(Singleton<GameManager>.Instance.Player, 3 + (((Enemy_51)thisEnemy).isGetSpike ? 1 : 0)));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy51_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy51_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_51)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Cover(thisEnemy, 10));
			Enemy_50 dragonHead = ((Enemy_51)thisEnemy).dragonHead;
			dragonHead.GetBuff(new Buff_Cover(dragonHead, 10));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy51_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy51_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_51)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：");
			}
		}

		private void Effect()
		{
			Player player = Singleton<GameManager>.Instance.Player;
			float value = Random.value;
			if (value < 0.25f)
			{
				player.GetBuff(new Buff_Shocked(player, 2));
			}
			else if (value < 0.5f)
			{
				player.GetBuff(new Buff_Freeze(player, 1));
			}
			else if (value < 0.75f)
			{
				player.PlayerBattleInfo.ClearMainHandCards(null);
			}
			else
			{
				player.GetBuff(new Buff_Power(player, -1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy51_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy51_Action4(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 4, string.Empty)
			});
		}

		protected override int GetBaseDmg()
		{
			return 8;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_51)thisEnemy.EnemyCtrl).Action4Anim(null, 4, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1，对玩家造成?次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 3)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy51_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy51_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_51)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：");
			}
		}

		private void Effect()
		{
			((Enemy_51)thisEnemy).GetSpike();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_51 enemy51Ctrl;

	private Enemy_50 dragonHead;

	private bool isGetSpike;

	private LogicHandler currentHandler;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_51(EnemyAttr attr, EnemyCtrl_51 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy51Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[5]
		{
			new Enemy51_Action1(this),
			new Enemy51_Action2(this),
			new Enemy51_Action3(this),
			new Enemy51_Action4(this),
			new Enemy51_Action5(this)
		};
	}

	public void SetDragonHeadAnimCtrl(Monster50AnimCtrl monster50AnimCtrl)
	{
		enemy51Ctrl.SetDragonHeadCtrl(monster50AnimCtrl);
	}

	public void SetDragonHead(Enemy_50 enemy50)
	{
		dragonHead = enemy50;
	}

	private void GetSpike()
	{
		dragonHead.GetSpike();
		isGetSpike = true;
		EventManager.RegisterObjRelatedEvent(this, EventEnum.E_GetNewBuff, OnEntityGetBuff);
		EventManager.RegisterObjRelatedEvent(this, EventEnum.E_RemoveBuff, OnEntityRemoveBuff);
	}

	public void SetInitPosition(Vector3 pos)
	{
		enemy51Ctrl.transform.position = pos;
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterObjRelatedEvent(this, EventEnum.E_GetNewBuff, OnEntityGetBuff);
		EventManager.UnregisterObjRelatedEvent(this, EventEnum.E_RemoveBuff, OnEntityRemoveBuff);
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEntityGetBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Cover)
		{
			GetBuff(new Buff_DamageRestrik(this, 5));
		}
	}

	private void OnEntityRemoveBuff(EventData data)
	{
		BuffEventData buffEventData;
		if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Cover)
		{
			BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(this, BuffType.Buff_DamageRestrik);
			RemoveBuff(specificBuff);
		}
	}

	protected override void OnStartBattle()
	{
		SetCurrentHandler(new LogicP1(this));
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		isGetSpike = false;
		GetBuff(new Buff_FlameArmor(this, int.MaxValue));
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
	}

	private void OnEnemyDead(EventData data)
	{
		SetCurrentHandler(new LogicPreP2(this));
	}

	protected override EnemyMean GetNextAction()
	{
		return currentHandler.GetNextAction();
	}

	private void SetCurrentHandler(LogicHandler logicHandler)
	{
		currentHandler = logicHandler;
	}
}
