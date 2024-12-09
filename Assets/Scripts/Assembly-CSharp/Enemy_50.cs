using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_50 : EnemyBase
{
	private abstract class LogicHandler
	{
		protected Enemy_50 _enemy50;

		public LogicHandler(Enemy_50 enemy50)
		{
			_enemy50 = enemy50;
		}

		public abstract EnemyMean GetNextAction();
	}

	private class LogicP1 : LogicHandler
	{
		private Queue<EnemyMean> allActionsQueue = new Queue<EnemyMean>(3);

		public LogicP1(Enemy_50 enemy50)
			: base(enemy50)
		{
		}

		public override EnemyMean GetNextAction()
		{
			int num = 0;
			if (_enemy50.leftDragonHand != null && _enemy50.leftDragonHand.IsDead)
			{
				num++;
			}
			if (_enemy50.rightDragonHand != null && _enemy50.rightDragonHand.IsDead)
			{
				num++;
			}
			switch (num)
			{
			case 1:
				_enemy50.SetCurrentHandler(new LogicPreP2(_enemy50));
				return _enemy50.GetNextAction();
			case 2:
				_enemy50.SetCurrentHandler(new LogicPreP3(_enemy50));
				return _enemy50.GetNextAction();
			default:
				if (allActionsQueue.Count == 0)
				{
					InitLogicHandler();
				}
				return allActionsQueue.Dequeue();
			}
		}

		private void InitLogicHandler()
		{
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[0]);
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[1]);
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[3]);
		}
	}

	private class LogicPreP2 : LogicHandler
	{
		public LogicPreP2(Enemy_50 enemy50)
			: base(enemy50)
		{
		}

		public override EnemyMean GetNextAction()
		{
			_enemy50.SetCurrentHandler(new LogicP2(_enemy50));
			return _enemy50.enemyActionArray[4];
		}
	}

	private class LogicP2 : LogicHandler
	{
		private Queue<EnemyMean> allActiionsQueue = new Queue<EnemyMean>(3);

		public LogicP2(Enemy_50 enemy50)
			: base(enemy50)
		{
		}

		public override EnemyMean GetNextAction()
		{
			int num = 0;
			if (_enemy50.leftDragonHand.IsDead)
			{
				num++;
			}
			if (_enemy50.rightDragonHand.IsDead)
			{
				num++;
			}
			if (num == 2)
			{
				_enemy50.SetCurrentHandler(new LogicPreP3(_enemy50));
				return _enemy50.GetNextAction();
			}
			if (allActiionsQueue.Count == 0)
			{
				InitLogicHandler();
			}
			return allActiionsQueue.Dequeue();
		}

		private void InitLogicHandler()
		{
			allActiionsQueue.Enqueue(_enemy50.enemyActionArray[2]);
			allActiionsQueue.Enqueue(_enemy50.enemyActionArray[5]);
			allActiionsQueue.Enqueue(_enemy50.enemyActionArray[1]);
		}
	}

	private class LogicPreP3 : LogicHandler
	{
		public LogicPreP3(Enemy_50 enemy50)
			: base(enemy50)
		{
		}

		public override EnemyMean GetNextAction()
		{
			_enemy50.SetCurrentHandler(new LogicP3(_enemy50));
			return _enemy50.enemyActionArray[6];
		}
	}

	private class LogicP3 : LogicHandler
	{
		private Queue<EnemyMean> allActionsQueue = new Queue<EnemyMean>();

		public LogicP3(Enemy_50 enemy50)
			: base(enemy50)
		{
		}

		public override EnemyMean GetNextAction()
		{
			if (allActionsQueue.Count == 0)
			{
				InitLogicHandler();
			}
			return allActionsQueue.Dequeue();
		}

		private void InitLogicHandler()
		{
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[1]);
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[2]);
			allActionsQueue.Enqueue(_enemy50.enemyActionArray[7]);
		}
	}

	private class Enemy50_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：");
			}
		}

		private void Effect()
		{
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action2 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy50_Action2(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new AttackMeanHandler(RealDmg(), 4 + ((Enemy_50)thisEnemy).extraAction2Time)
			});
		}

		protected override int GetBaseDmg()
		{
			return 12;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action2Anim(null, 4 + ((Enemy_50)thisEnemy).extraAction2Time, Effect, thisEnemy.NextAction);
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
			}
			if (i == 3 + ((Enemy_50)thisEnemy).extraAction2Time)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy50_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy50_Action3(EnemyBase enemyBase)
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
			return 30;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
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
				Buff_Wound buff_Wound;
				if ((buff_Wound = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_Wound) as Buff_Wound) != null)
				{
					buff_Wound.TakeEffect(Singleton<GameManager>.Instance.Player);
				}
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action4 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action4Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为4：");
			}
		}

		private void Effect()
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			for (int i = 0; i < allEnemies.Count; i++)
			{
				allEnemies[i].GetBuff(new Buff_Power(allEnemies[i], 4));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：");
			}
		}

		private void Effect()
		{
			BaseBuff specificBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetSpecificBuff(thisEnemy, BuffType.Buff_MagicSquama);
			thisEnemy.RemoveBuff(specificBuff);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action6 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action6(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action6Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为6：");
			}
		}

		private void Effect()
		{
			Player player = Singleton<GameManager>.Instance.Player;
			player.GetBuff(new Buff_Shocked(player, 2));
			player.GetBuff(new Buff_Silence(player, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action7 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action7(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action7Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为7：");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_Angry(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy50_Action8 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy50_Action8(EnemyBase enemyBase)
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
			((EnemyCtrl_50)thisEnemy.EnemyCtrl).Action8Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为8：");
			}
		}

		private void Effect()
		{
			((Enemy_50)thisEnemy).extraAction2Time++;
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_50 enemy50Ctrl;

	private Enemy_51 rightDragonHand;

	private Enemy_52 leftDragonHand;

	private int extraAction2Time;

	private LogicHandler currentLogic;

	public int CurrentState
	{
		get
		{
			int num = 4;
			if (leftDragonHand == null || leftDragonHand.IsDead)
			{
				num -= 2;
			}
			if (rightDragonHand == null || rightDragonHand.IsDead)
			{
				num--;
			}
			return num;
		}
	}

	protected override EnemyMean[] enemyActionArray { get; }

	public override Transform ArmorTrans => enemy50Ctrl.ArmorTrans;

	public Enemy_50(EnemyAttr attr, EnemyCtrl_50 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy50Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[8]
		{
			new Enemy50_Action1(this),
			new Enemy50_Action2(this),
			new Enemy50_Action3(this),
			new Enemy50_Action4(this),
			new Enemy50_Action5(this),
			new Enemy50_Action6(this),
			new Enemy50_Action7(this),
			new Enemy50_Action8(this)
		};
	}

	public void GetSpike()
	{
		EventManager.RegisterObjRelatedEvent(this, EventEnum.E_GetNewBuff, OnEntityGetBuff);
		EventManager.RegisterObjRelatedEvent(this, EventEnum.E_RemoveBuff, OnEntityRemoveBuff);
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterObjRelatedEvent(this, EventEnum.E_GetNewBuff, OnEntityGetBuff);
		EventManager.UnregisterObjRelatedEvent(this, EventEnum.E_RemoveBuff, OnEntityRemoveBuff);
		if (rightDragonHand != null && !rightDragonHand.IsDead)
		{
			Singleton<EnemyController>.Instance.RemoveMonster(rightDragonHand, isNeedReAdjust: false);
		}
		if (leftDragonHand != null && !leftDragonHand.IsDead)
		{
			Singleton<EnemyController>.Instance.RemoveMonster(leftDragonHand, isNeedReAdjust: false);
		}
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

	protected override EnemyMean GetNextAction()
	{
		return currentLogic.GetNextAction();
	}

	protected override void OnStartBattle()
	{
		currentLogic = new LogicP1(this);
		GetBuff(new Buff_MagicSquama(this, int.MaxValue));
		GetBuff(new Buff_FlameArmor(this, int.MaxValue));
		InitSet();
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
	}

	private void InitSet()
	{
		extraAction2Time = 0;
		enemy50Ctrl.transform.localScale = SingletonDontDestroy<UIManager>.Instance.ScaleRate * Vector3.one;
		enemy50Ctrl.transform.localPosition = new Vector3(0f, -0.25f, 0f);
		enemy50Ctrl.StartCoroutine(InitHand_IE());
	}

	private IEnumerator InitHand_IE()
	{
		yield return null;
		rightDragonHand = (Enemy_51)Singleton<EnemyController>.Instance.AddSpecialMonster("Monster_51", actionFlag: false);
		leftDragonHand = (Enemy_52)Singleton<EnemyController>.Instance.AddSpecialMonster("Monster_52", actionFlag: false);
		rightDragonHand.SetDragonHead(this);
		leftDragonHand.SetDragonHead(this);
		rightDragonHand.SetDragonHeadAnimCtrl(enemy50Ctrl.DragonHeadAnimCtrl);
		leftDragonHand.SetDragonHeadAnimCtrl(enemy50Ctrl.DragonHeadAnimCtrl);
		leftDragonHand.SetInitPosition(enemy50Ctrl.LeftHandPoint.position);
		rightDragonHand.SetInitPosition(enemy50Ctrl.RightHandPoint.position);
	}

	protected override void OnEntityGetHurtOnBattle(int healthDmg, int armorDmg, bool isAbsDmg)
	{
		enemyCtrl.FlashWhite();
		if (healthDmg > 0)
		{
			Singleton<GameHintManager>.Instance.ShowDamageFlowingText(base.EnemyCtrl.transform, isSetParent: true, Vector3.zero, Vector2.one * 0.5f, healthDmg, 0.005f, isAbsDmg);
		}
		if (armorDmg > 0)
		{
			Singleton<GameHintManager>.Instance.ShowArmorDamageFlowingText(enemy50Ctrl.ArmorTrans, isSetParent: false, Vector3.zero, armorDmg, 0.005f, isAbsDmg);
		}
	}

	private void SetCurrentHandler(LogicHandler handler)
	{
		currentLogic = handler;
	}
}
