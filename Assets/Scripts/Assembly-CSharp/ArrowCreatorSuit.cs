using System;

public class ArrowCreatorSuit : Suit
{
	private class ArrowCreator_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_SpecialArrowEffect, OnSpecialArrowEffect);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_SpecialArrowEffect, OnSpecialArrowEffect);
		}

		private void OnSpecialArrowEffect(EventData data)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
		}
	}

	private class ArrowCreator_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_AddSpecialAttr, OnAddSpecialAttr);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_AddSpecialAttr, OnAddSpecialAttr);
		}

		private void OnAddSpecialAttr(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null)
			{
				int intValue = simpleEventData.intValue;
				Singleton<GameManager>.Instance.Player.PlayerAttr.AddArmor(intValue);
			}
		}
	}

	private class ArrowCreator_SuitEffect_7 : BaseBattleEffect
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
			((ArcherPlayerAttr)Singleton<GameManager>.Instance.Player.PlayerAttr).AddRandomSpecialArrows(3, isNeedReplacePreEffect: true);
		}
	}

	public override SuitType SuitType => SuitType.ArrowCreator;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new ArrowCreator_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new ArrowCreator_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new ArrowCreator_SuitEffect_7());
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
