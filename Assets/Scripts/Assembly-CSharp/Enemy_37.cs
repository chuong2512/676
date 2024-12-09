using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_37 : EnemyBase
{
	private class Enemy37_Action1 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy37_Action1(EnemyBase enemyBase)
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
			return 6;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action1Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为1：寒冰吐息，造成1次{realDmg}的非真实伤害({atkDes})");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Freeze(Singleton<GameManager>.Instance.Player, 1));
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy37_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy37_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action2Anim(null, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为2：刺骨寒意，对玩家施加3层破甲buff，1层震荡buff，1层沉默buff");
			}
		}

		private void Effect()
		{
			Player player = Singleton<GameManager>.Instance.Player;
			player.GetBuff(new Buff_BrokenArmor(player, 3));
			player.GetBuff(new Buff_Shocked(player, 1));
			player.GetBuff(new Buff_Silence(player, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy37_Action3 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy37_Action3(EnemyBase enemyBase)
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
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action3Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为3：冰棱，对自己施加1层反伤buff");
			}
		}

		private void Effect()
		{
			thisEnemy.GetBuff(new Buff_DamageRestrik(thisEnemy, 1));
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy37_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy37_Action4(EnemyBase enemyBase)
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
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action4Anim(2, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：冰霜之拳，造成2次{realDmg}的非真实伤害({atkDes})");
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
			if (i == 1)
			{
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy37_Action5 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy37_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action5Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为5：碎裂，移除自身身上除了反伤外的所有效果");
			}
		}

		private void Effect()
		{
			BaseBuff[] allBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetAllBuff(thisEnemy);
			if (!allBuff.IsNull())
			{
				for (int i = 0; i < allBuff.Length; i++)
				{
					if (allBuff[i].BuffType != BuffType.Buff_DamageRestrik)
					{
						thisEnemy.RemoveBuff(allBuff[i]);
					}
				}
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy37_Action6 : AtkEnemyMean
	{
		private int atkTime;

		private int extraDmg;

		private const string Enemy23_ActionSpecialDesKey = "Enemy23_ActionSpecialDesKey";

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy37_Action6(EnemyBase enemyBase)
			: base(enemyBase)
		{
			atkTime = 1;
			extraDmg = 0;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), atkTime, "Enemy23_ActionSpecialDesKey".LocalizeText())
			});
		}

		protected override int GetBaseDmg()
		{
			return 4 + extraDmg;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action6Anim(atkTime, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为6：寒冰溅射，造成{atkTime}次{realDmg}的非真实伤害({atkDes})");
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
			thisEnemy.TakeDamage(4, null, isAbsDmg: true);
			if (i == atkTime - 1)
			{
				atkTime++;
				extraDmg++;
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private class Enemy37_Action7 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy37_Action7(EnemyBase enemyBase)
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
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action7Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为7：移除身上的所有效果");
			}
		}

		private void Effect()
		{
			BaseBuff[] allBuff = Singleton<GameManager>.Instance.BattleSystem.BuffSystem.GetAllBuff(thisEnemy);
			if (!allBuff.IsNull())
			{
				for (int i = 0; i < allBuff.Length; i++)
				{
					thisEnemy.RemoveBuff(allBuff[i]);
				}
			}
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	public class Enemy37_Action8 : AtkEnemyMean
	{
		private int castTime;

		private const string Enemy23_ActionSpecialDesKey = "Enemy23_ActionSpecialDesKey";

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy37_Action8(EnemyBase enemyBase)
			: base(enemyBase)
		{
			castTime = 0;
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 4, "Enemy23_ActionSpecialDesKey".LocalizeText())
			});
		}

		protected override int GetBaseDmg()
		{
			return 8 + castTime;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action8Anim(4, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为8：破釜沉舟，造成4次{realDmg}的非真实伤害({atkDes})");
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
			thisEnemy.TakeDamage(4, null, isAbsDmg: true);
			if (i == 3)
			{
				castTime++;
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	public class Enemy37_Action9 : AtkEnemyMean
	{
		private const string Enemy23_ActionSpecialDesKey = "Enemy23_ActionSpecialDesKey";

		public override bool ItWillBreakDefence => true;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy37_Action9(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 1, "Enemy23_ActionSpecialDesKey".LocalizeText())
			});
		}

		protected override int GetBaseDmg()
		{
			return 20;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_37)thisEnemy.EnemyCtrl).Action9Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为9：绝望寒冰，造成1次{realDmg}的非真实伤害({atkDes})");
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
			thisEnemy.TakeDamage(10, null, isAbsDmg: true);
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private EnemyCtrl_37 enemy37Ctrl;

	private List<EnemyMean> randomList;

	private bool isEverRevived;

	private int battleRoundOffset;

	private Func<EnemyMean> Handler;

	private int loopCounter;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_37(EnemyAttr attr, EnemyCtrl_37 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy37Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[9]
		{
			new Enemy37_Action1(this),
			new Enemy37_Action2(this),
			new Enemy37_Action3(this),
			new Enemy37_Action4(this),
			new Enemy37_Action5(this),
			new Enemy37_Action6(this),
			new Enemy37_Action7(this),
			new Enemy37_Action8(this),
			new Enemy37_Action9(this)
		};
	}

	protected override void OnStartBattle()
	{
		isEverRevived = false;
		enemy37Ctrl.IdleAnim();
		GetBuff(new Buff_FreezeArmor(this, int.MaxValue));
		randomList = new List<EnemyMean>(3)
		{
			enemyActionArray[0],
			enemyActionArray[1],
			enemyActionArray[3]
		};
		Handler = Action3;
		currentEnemyMean = GetNextAction();
		currentEnemyMean.OnSetMean();
		loopCounter = 0;
	}

	public override void Dead()
	{
		if (isEverRevived)
		{
			isDead = true;
			string soundName = "VoiceAct/" + base.EnemyCode + "_Dead";
			SingletonDontDestroy<AudioManager>.Instance.PlaySound(soundName);
			Singleton<GameManager>.Instance.BattleSystem.BuffSystem.RemoveBuff(this);
			EventManager.BroadcastEvent(EventEnum.E_EnemyDead, new SimpleEventData
			{
				objValue = this
			});
			enemyCtrl.OnEnemyDead(OnEnemyDeadEffect);
		}
		else
		{
			isEverRevived = true;
			enemyAttr.RecoveryHealth(enemyAttr.MaxHealth);
			currentEnemyMean = enemyActionArray[6];
			currentEnemyMean.OnSetMean();
			Handler = Action6Loop;
			enemy37Ctrl.RebornAnim();
		}
	}

	protected override EnemyMean GetNextAction()
	{
		return Handler();
	}

	private EnemyMean Action3()
	{
		Handler = Action1;
		return enemyActionArray[2];
	}

	private EnemyMean Action1()
	{
		Handler = Loop;
		return enemyActionArray[0];
	}

	private EnemyMean Loop()
	{
		if ((float)enemyAttr.Health / (float)enemyAttr.MaxHealth < 0.5f)
		{
			Handler = Action6Loop;
			return enemyActionArray[2];
		}
		return HighHealthLoop();
	}

	private EnemyMean Action6Loop()
	{
		if (isEverRevived)
		{
			battleRoundOffset = Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount % 3;
			Handler = ReviveLoop;
			return Handler();
		}
		return enemyActionArray[5];
	}

	private EnemyMean HighHealthLoop()
	{
		loopCounter++;
		if (loopCounter == 3)
		{
			randomList.Add(currentEnemyMean);
			loopCounter = 0;
			return enemyActionArray[2];
		}
		int index = UnityEngine.Random.Range(0, randomList.Count);
		EnemyMean result = randomList[index];
		randomList.RemoveAt(index);
		if (!currentEnemyMean.IsNull() && !(currentEnemyMean is Enemy37_Action3))
		{
			randomList.Add(currentEnemyMean);
		}
		return result;
	}

	private EnemyMean ReviveLoop()
	{
        switch ((Singleton<GameManager>.Instance.BattleSystem.BattleRoundAmount - battleRoundOffset) % 3) 
		{
			case 0 :return enemyActionArray[1]; 
			case 1 :return enemyActionArray[7]; 
			case 2 :return enemyActionArray[8]; 
			default :return null; 
		};
	}
}
