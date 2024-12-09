using System;
using System.Collections.Generic;
using UnityEngine;

public class PoisonShooterSuit : Suit
{
	private class PoisonShooter_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerUseUsualCard, OnPlayerUseUsualCard);
		}

		private void OnPlayerUseUsualCard(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "BC_O_6")
			{
				List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
				EnemyBase enemyBase = allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)];
				enemyBase.GetBuff(new Buff_DeadPoison(enemyBase, 1));
			}
		}
	}

	private class PoisonShooter_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecialAttr);
		}

		private void OnComsumeSpecialAttr(EventData data)
		{
			SimpleEventData simpleEventData;
			EntityBase[] array;
			if ((simpleEventData = data as SimpleEventData) != null && (array = simpleEventData.objValue as EntityBase[]) != null)
			{
				int intValue = simpleEventData.intValue;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].GetBuff(new Buff_DeadPoison(array[i], intValue));
				}
			}
		}
	}

	private class PoisonShooter_SuitEffect_7 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		private void OnPlayerEndRound(EventData data)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_PoisonGas(Singleton<GameManager>.Instance.Player, 1));
		}
	}

	public override SuitType SuitType => SuitType.PoisonShooter;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new PoisonShooter_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new PoisonShooter_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new PoisonShooter_SuitEffect_7());
			break;
		case 4:
		case 6:
			break;
		}
	}

	public override void RemoveSuit(string equipCode)
	{
		if (!suitInfomation.Remove(equipCode))
		{
			throw new ArgumentException("Value does not contain in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 2:
			RemoveSuitBattleEffect(3);
			break;
		case 4:
			RemoveSuitBattleEffect(5);
			break;
		case 6:
			RemoveSuitBattleEffect(7);
			break;
		case 3:
		case 5:
			break;
		}
	}
}
