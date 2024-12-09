using System;

public class PhantomRangerSuit : Suit
{
	private class PhantomRanger_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_EnemyDead, OnEnemyDead);
		}

		private void OnEnemyDead(EventData data)
		{
			Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShadowEscape(Singleton<GameManager>.Instance.Player, 1));
		}
	}

	private class PhantomRanger_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnRemoveBuff);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnGetNewBuff);
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnRemoveBuff);
		}

		private void OnGetNewBuff(EventData data)
		{
			BuffEventData buffEventData;
			if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_ShadowEscape)
			{
				EventManager.RegisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecualAttr);
			}
		}

		private void OnRemoveBuff(EventData data)
		{
			BuffEventData buffEventData;
			if ((buffEventData = data as BuffEventData) != null && buffEventData.buffType == BuffType.Buff_ShadowEscape)
			{
				EventManager.UnregisterEvent(EventEnum.E_ComsumeSpecialAttr, OnComsumeSpecualAttr);
			}
		}

		private void OnComsumeSpecualAttr(EventData data)
		{
			SimpleEventData simpleEventData;
			EntityBase[] array;
			if ((simpleEventData = data as SimpleEventData) != null && !simpleEventData.boolValue && (array = simpleEventData.objValue as EntityBase[]) != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].GetBuff(new Buff_Bleeding(array[i], 1));
				}
			}
		}
	}

	private class PhantomRanger_SuitEffect_7 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_OnDodgeBuffEffect, OnDodgeBuffEffect);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_OnDodgeBuffEffect, OnDodgeBuffEffect);
		}

		private void OnDodgeBuffEffect(EventData data)
		{
			if (Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(Singleton<GameManager>.Instance.Player, BuffType.Buff_ShadowEscape))
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_ShadowEscape(Singleton<GameManager>.Instance.Player, 1));
			}
		}
	}

	public override SuitType SuitType => SuitType.PhantomRanger;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new PhantomRanger_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new PhantomRanger_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new PhantomRanger_SuitEffect_7());
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
