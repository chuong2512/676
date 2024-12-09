using System;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalFlameSuit : Suit
{
	private class ImmortalFlame_SuitEffect_2 : BaseBattleEffect
	{
		private ImmortalFlameSuit suit;

		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public ImmortalFlame_SuitEffect_2(ImmortalFlameSuit suit)
		{
			this.suit = suit;
		}

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		private void OnPlayerDrawCard(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null)
			{
				suit.AddImmortalFlameAmount(simpleEventData.intValue);
			}
		}

		private void OnPlayerEndRound(EventData data)
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			BaseBattleEffect.BattleEffectAtkEntity(allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)], suit.immortalFlameAmount, isAbsDmg: true);
			suit.RemoveImmortalFlameAmount(suit.immortalFlameAmount);
		}
	}

	private class ImmortalFlame_SuitEffect_3 : BaseBattleEffect
	{
		private ImmortalFlameSuit suit;

		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public ImmortalFlame_SuitEffect_3(ImmortalFlameSuit suit)
		{
			this.suit = suit;
		}

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		private void OnPlayerDrawCard(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null)
			{
				suit.AddImmortalFlameAmount(simpleEventData.intValue);
			}
		}

		private void OnPlayerEndRound(EventData data)
		{
			List<EnemyBase> allEnemies = Singleton<EnemyController>.Instance.AllEnemies;
			BaseBattleEffect.BattleEffectAtkEntity(allEnemies[UnityEngine.Random.Range(0, allEnemies.Count)], suit.immortalFlameAmount, isAbsDmg: true);
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(suit.immortalFlameAmount);
			suit.RemoveImmortalFlameAmount(suit.immortalFlameAmount);
		}
	}

	private class ImmortalFlame_SuitEffect_4 : BaseBattleEffect
	{
		private ImmortalFlameSuit suit;

		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public ImmortalFlame_SuitEffect_4(ImmortalFlameSuit suit)
		{
			this.suit = suit;
		}

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.RegisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerDrawCard, OnPlayerDrawCard);
			EventManager.UnregisterEvent(EventEnum.E_PlayerEndRound, OnPlayerEndRound);
		}

		private void OnPlayerDrawCard(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null)
			{
				suit.AddImmortalFlameAmount(simpleEventData.intValue);
			}
		}

		private void OnPlayerEndRound(EventData data)
		{
			List<EnemyBase> list = new List<EnemyBase>(Singleton<EnemyController>.Instance.AllEnemies);
			for (int i = 0; i < list.Count; i++)
			{
				BaseBattleEffect.BattleEffectAtkEntity(list[i], suit.immortalFlameAmount, isAbsDmg: true);
			}
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(suit.immortalFlameAmount);
			suit.RemoveImmortalFlameAmount(suit.immortalFlameAmount);
		}
	}

	private int immortalFlameAmount;

	private EquipEffectIconCtrl _effectIconCtrl;

	public override SuitType SuitType => SuitType.ImmortalFlame;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 2:
			AddSuitBattleEffect(2, new ImmortalFlame_SuitEffect_2(this));
			break;
		case 3:
			AddSuitBattleEffect(3, new ImmortalFlame_SuitEffect_3(this));
			RemoveSuitBattleEffect(2);
			break;
		case 4:
			AddSuitBattleEffect(4, new ImmortalFlame_SuitEffect_4(this));
			RemoveSuitBattleEffect(3);
			break;
		}
	}

	public override void OnBattleStart()
	{
		if (suitBattleEffect.Count != 0)
		{
			BattleUI battleUI = SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI;
			_effectIconCtrl = battleUI.AddSuitEffet(SuitType, "0", base.GetSuitBattleEffectDescription);
			_effectIconCtrl.SetEffect();
			immortalFlameAmount = 0;
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
		case 3:
			AddSuitBattleEffect(3, new ImmortalFlame_SuitEffect_3(this));
			RemoveSuitBattleEffect(4);
			break;
		case 2:
			AddSuitBattleEffect(2, new ImmortalFlame_SuitEffect_2(this));
			RemoveSuitBattleEffect(3);
			break;
		case 1:
			RemoveSuitBattleEffect(2);
			break;
		}
	}

	public void AddImmortalFlameAmount(int amount)
	{
		immortalFlameAmount += amount;
		_effectIconCtrl.UpdateEquipHint(immortalFlameAmount.ToString());
	}

	public void RemoveImmortalFlameAmount(int amount)
	{
		immortalFlameAmount -= amount;
		_effectIconCtrl.UpdateEquipHint(immortalFlameAmount.ToString());
	}
}
