using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_31 : EnemyBase
{
	private class Enemy31_Action1 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy31_Action1(EnemyBase enemyBase)
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
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action1Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent(thisEnemy.EntityName + "执行行为1：元素召唤，召唤4各相同的元素");
			}
		}

		private void Effect()
		{
			((Enemy_31)thisEnemy).SummorMonster();
		}
	}

	private class Enemy31_Action2 : EnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => false;

		public Enemy31_Action2(EnemyBase enemyBase)
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
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action2Anim(new Transform[1] { thisEnemy.EnemyCtrl.transform }, Effect, thisEnemy.NextAction);
		}

		private void Effect()
		{
			((Enemy_31)thisEnemy).AbsordElement();
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy31_Action3 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy31_Action3(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[2]
			{
				new AttackMeanHandler(RealDmg(), 1),
				new BuffMeanHandler()
			});
		}

		protected override int GetBaseDmg()
		{
			return 10;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action3Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为3：烈焰冲击，对玩家造2次{realDmg}的非真实伤害({atkDes})");
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
			thisEnemy.GetBuff(new Buff_Power(thisEnemy, 1));
			((Enemy_31)thisEnemy).AddElement("Monster_7");
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy31_Action4 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy31_Action4(EnemyBase enemyBase)
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
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action4Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为4：寒流冲击，对玩家造1次{realDmg}的非真实伤害({atkDes}),并给玩家施加1层震荡buff");
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
				EnemyBase.EnemyAttackPlayer(RealDmg(), thisEnemy, isTrue: false);
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 2));
			}
			((Enemy_31)thisEnemy).AddElement("Monster_8");
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy31_Action5 : AtkEnemyMean
	{
		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy31_Action5(EnemyBase enemyBase)
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
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action5Anim(null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out var atkDes);
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为5：岩石冲击，对玩家造1次{realDmg}的非真实伤害({atkDes}),并为自己+4护甲");
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
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 2));
			}
			((Enemy_31)thisEnemy).AddElement("Monster_9");
			thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
		}
	}

	private class Enemy31_Action6 : AtkEnemyMean
	{
		private List<string> allElements;

		private string atkDes;

		public override bool ItWillBreakDefence => false;

		public override bool IsNeedUpdateByBuffChange => true;

		public Enemy31_Action6(EnemyBase enemyBase)
			: base(enemyBase)
		{
		}

		public override void OnSetMean()
		{
			thisEnemy.EnemyCtrl.AddMean(new MeanHandler[1]
			{
				new SpeAtkMeanHandler(RealDmg(), 5, string.Empty)
			});
			allElements = ((Enemy_31)thisEnemy).AllAbsordElements;
		}

		protected override int GetBaseDmg()
		{
			return 5;
		}

		public override void OnLogic()
		{
			((EnemyCtrl_31)thisEnemy.EnemyCtrl).Action6Anim(5, null, Effect, thisEnemy.NextAction);
			realDmg = RealDmg(out atkDes);
		}

		private void Effect(int i)
		{
			GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
			if (thisEnemy.IsDead)
			{
				return;
			}
			if (EnemyBase.IsPlayerCanDodgeAttack(out var buff))
			{
				buff.TakeEffect(Singleton<GameManager>.Instance.Player);
			}
			else
			{
				EnemyBase.EnemyAttackPlayer(realDmg, thisEnemy, isTrue: false);
				switch (allElements[i])
				{
				case "Monster_7":
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为6：元素连弹，对玩家造成第{i + 1}次{realDmg}的非真实伤害({atkDes})，因为法杖上的火元素额外造成1点真实伤害");
					}
					Singleton<GameManager>.Instance.Player.TakeDamage(1, thisEnemy, isAbsDmg: true);
					Singleton<GameManager>.Instance.Player.PlayerBattleInfo.RandomDropHandCard();
					break;
				case "Monster_8":
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为6：元素连弹，对玩家造成第{i + 1}次{realDmg}的非真实伤害({atkDes}),因为法杖上的水元素额外给玩家施加1层震荡buff");
					}
					Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Shocked(Singleton<GameManager>.Instance.Player, 1));
					break;
				case "Monster_9":
					if (gameReportUI != null)
					{
						gameReportUI.AddGameReportContent($"{thisEnemy.EntityName}执行行为6：元素连弹，对玩家造成第{i + 1}次{realDmg}的非真实伤害({atkDes},因为法杖上的土元素额外给玩家施加1层破甲buff");
					}
					Singleton<GameManager>.Instance.Player.GetBuff(new Buff_BrokenArmor(Singleton<GameManager>.Instance.Player, 1));
					break;
				}
			}
			if (i == 4)
			{
				((Enemy_31)thisEnemy).ClearAllElements();
				thisEnemy.SetEnemyState(EnemyBase.EnemyIdleState);
			}
		}
	}

	private const string FireElement = "Monster_7";

	private const string OceanElement = "Monster_8";

	private const string EarthElement = "Monster_9";

	private const int MaxAbsordAmount = 5;

	private string currentElementName;

	private Queue<string> summorQueue = new Queue<string>();

	private List<ElementEnemy> allElementEntities = new List<ElementEnemy>();

	private List<string> allAbsordElements = new List<string>(5);

	private EnemyCtrl_31 enemy31Ctrl;

	private bool isEndAction;

	public List<string> AllAbsordElements => allAbsordElements;

	protected override EnemyMean[] enemyActionArray { get; }

	public Enemy_31(EnemyAttr attr, EnemyCtrl_31 enemyCtrl)
		: base(attr, enemyCtrl)
	{
		enemy31Ctrl = enemyCtrl;
		enemyActionArray = new EnemyMean[6]
		{
			new Enemy31_Action1(this),
			new Enemy31_Action2(this),
			new Enemy31_Action3(this),
			new Enemy31_Action4(this),
			new Enemy31_Action5(this),
			new Enemy31_Action6(this)
		};
	}

	protected override void OnStartBattle()
	{
		summorQueue.Clear();
		summorQueue.Enqueue("Monster_8");
		summorQueue.Enqueue("Monster_9");
		summorQueue.Enqueue("Monster_7");
		currentEnemyMean = enemyActionArray[0];
		currentEnemyMean.OnSetMean();
		EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		enemy31Ctrl.SetSkin(Monster31AnimCtrl.SkinType.Fire);
		enemy31Ctrl.ClearAllSlots();
		enemy31Ctrl.SetIdleAnim();
		currentElementName = "Monster_7";
	}

	public override void NextAction()
	{
		enemyCtrl.ShowEnemyMean();
		currentEnemyMean = ((SingletonDontDestroy<Game>.Instance.isTest && enemyCtrl.actionIndex > 0) ? enemyActionArray[enemyCtrl.actionIndex - 1] : GetNextAction());
		currentEnemyMean.OnSetMean();
		enemy31Ctrl.StartCoroutine(TryEndEnemyRound_IE());
	}

	private IEnumerator TryEndEnemyRound_IE()
	{
		while (!isEndAction)
		{
			yield return null;
		}
		Singleton<GameManager>.Instance.BattleSystem.EndEnemyRound();
	}

	protected override EnemyMean GetNextAction()
	{
		isEndAction = true;
		if (allAbsordElements.Count == 5)
		{
			return enemyActionArray[5];
		}
		if (currentEnemyMean is Enemy31_Action1)
		{
			return enemyActionArray[1];
		}
		if (currentEnemyMean is Enemy31_Action2)
		{
			switch(currentElementName)
			{
                case "Monster_7": return enemyActionArray[2]; 
				case "Monster_8": return enemyActionArray[3]; 
				case "Monster_9":
                    return enemyActionArray[4];
                    default: return null; 
			};
		}
		if (currentEnemyMean is Enemy31_Action3 || currentEnemyMean is Enemy31_Action4 || currentEnemyMean is Enemy31_Action5 || currentEnemyMean is Enemy31_Action6)
		{
			isEndAction = false;
			currentElementName = summorQueue.Dequeue();
			switch (currentElementName)
			{
			case "Monster_7":
				enemy31Ctrl.ChangeSkin(delegate
				{
					enemy31Ctrl.SetSkin(Monster31AnimCtrl.SkinType.Fire);
				}, delegate
				{
					enemy31Ctrl.SetIdleAnim();
					isEndAction = true;
				});
				break;
			case "Monster_8":
				enemy31Ctrl.ChangeSkin(delegate
				{
					enemy31Ctrl.SetSkin(Monster31AnimCtrl.SkinType.Water);
				}, delegate
				{
					enemy31Ctrl.SetIdleAnim();
					isEndAction = true;
				});
				break;
			case "Monster_9":
				enemy31Ctrl.ChangeSkin(delegate
				{
					enemy31Ctrl.SetSkin(Monster31AnimCtrl.SkinType.Earth);
				}, delegate
				{
					enemy31Ctrl.SetIdleAnim();
					isEndAction = true;
				});
				break;
			}
			return enemyActionArray[0];
		}
		return null;
	}

	private void ClearAllElements()
	{
		allAbsordElements.Clear();
		enemy31Ctrl.ClearAllSlots();
	}

	private void OnEnemyDead(EventData data)
	{
		SimpleEventData simpleEventData;
		ElementEnemy item;
		if ((simpleEventData = data as SimpleEventData) != null && (item = simpleEventData.objValue as ElementEnemy) != null && allElementEntities.Contains(item))
		{
			allElementEntities.Remove(item);
		}
	}

	public override void Dead()
	{
		base.Dead();
		EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		ClearAllElements();
		if (allElementEntities.Count > 0)
		{
			for (int i = 0; i < allElementEntities.Count; i++)
			{
				allElementEntities[i].FadeDead();
			}
			allElementEntities.Clear();
		}
	}

	public void SummorMonster()
	{
		List<EnemyBase> list = Singleton<EnemyController>.Instance.SummorMonster(new List<string>(4) { currentElementName, currentElementName, currentElementName, currentElementName }, new List<bool>(4) { false, false, false, false }, delegate
		{
			SetEnemyState(EnemyBase.EnemyIdleState);
		});
		for (int i = 0; i < list.Count; i++)
		{
			allElementEntities.Add((ElementEnemy)list[i]);
		}
		summorQueue.Enqueue(currentElementName);
	}

	public void AbsordElement()
	{
		int num = 0;
		for (int i = 0; i < allElementEntities.Count; i++)
		{
			if (allAbsordElements.Count < 5)
			{
				AddElement(allElementEntities[i].EnemyCode);
				num++;
			}
			allElementEntities[i].FadeDead();
		}
		GameReportUI gameReportUI = SingletonDontDestroy<UIManager>.Instance.GetView("GameReportUI") as GameReportUI;
		if (currentElementName == "Monster_7")
		{
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{EntityName}执行行为2：元素吸收，自身获得{num}层力量buff");
			}
			GetBuff(new Buff_Power(this, num));
		}
		else if (currentElementName == "Monster_8")
		{
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{EntityName}执行行为2：元素吸收，自身恢复{num * 5}点生命值");
			}
			enemyAttr.RecoveryHealth(20 * num);
		}
		else
		{
			if (gameReportUI != null)
			{
				gameReportUI.AddGameReportContent($"{EntityName}执行行为2：元素吸收，自身获得{num * 4}点护甲");
			}
			enemyAttr.AddArmor(20 * num);
		}
		allElementEntities.Clear();
	}

	public void AddElement(string elementCode)
	{
		switch (elementCode)
		{
		case "Monster_7":
			enemy31Ctrl.SetSlot(allAbsordElements.Count, Monster31AnimCtrl.SkinType.Fire);
			break;
		case "Monster_8":
			enemy31Ctrl.SetSlot(allAbsordElements.Count, Monster31AnimCtrl.SkinType.Water);
			break;
		case "Monster_9":
			enemy31Ctrl.SetSlot(allAbsordElements.Count, Monster31AnimCtrl.SkinType.Earth);
			break;
		}
		allAbsordElements.Add(elementCode);
	}
}
