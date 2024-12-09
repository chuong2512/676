using System;

public class TWKnightSuit : Suit
{
	public class TW_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingUsualCardEffectTimeAdd;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "S_K_2")
			{
				IntData = 1;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	public class TW_SuitEffect_5 : BaseBattleEffect
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
			if ((simpleEventData = data as SimpleEventData) != null && simpleEventData.stringValue == "BC_O_4")
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Stable(Singleton<GameManager>.Instance.Player, 1));
			}
		}
	}

	public class TW_SuitEffect_7 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnPlayerGetNewBuff);
		}

		private void OnPlayerGetNewBuff(EventData data)
		{
			BuffEventData buffEventData;
			if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Defence)
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_JianShou(Singleton<GameManager>.Instance.Player, 1));
			}
		}
	}

	public override SuitType SuitType => SuitType.TW_Knight;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new TW_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new TW_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new TW_SuitEffect_7());
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
