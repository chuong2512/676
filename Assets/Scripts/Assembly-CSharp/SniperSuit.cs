using System;

public class SniperSuit : Suit
{
	private class Sniper_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_ShootArrowWhenAim, ShootArrowWhenAim);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_ShootArrowWhenAim, ShootArrowWhenAim);
		}

		private void ShootArrowWhenAim(EventData data)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Cover(Singleton<GameManager>.Instance.Player, 2));
		}
	}

	private class Sniper_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetAimBuff, OnAddAimBuff);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetAimBuff, OnAddAimBuff);
		}

		private void OnAddAimBuff(EventData data)
		{
			SimpleEventData simpleEventData;
			if ((simpleEventData = data as SimpleEventData) != null)
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Power(Singleton<GameManager>.Instance.Player, simpleEventData.intValue));
			}
		}
	}

	private class Sniper_SuitEffect_7 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetSameBuff, OnGetSameBuff);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetSameBuff, OnGetSameBuff);
		}

		private void AddShootStrengthen(int Amount)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShootStrengthen(Singleton<GameManager>.Instance.Player, Amount));
		}

		private void OnGetNewBuff(EventData data)
		{
			BuffEventData buffEventData;
			if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Cover)
			{
				AddShootStrengthen(buffEventData.intValue);
			}
		}

		private void OnGetSameBuff(EventData data)
		{
			BuffEventData buffEventData;
			if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_Cover)
			{
				AddShootStrengthen(buffEventData.intValue);
			}
		}
	}

	public override SuitType SuitType => SuitType.Sniper;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new Sniper_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new Sniper_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new Sniper_SuitEffect_7());
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
