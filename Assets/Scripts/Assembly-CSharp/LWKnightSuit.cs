using System;

public class LWKnightSuit : Suit
{
	public class LW_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_EntityDamageRestrick, OnPlayerDamageRestrick);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_EntityDamageRestrick, OnPlayerDamageRestrick);
		}

		private void OnPlayerDamageRestrick(EventData data)
		{
			SimpleEventData simpleEventData;
			EntityBase entityBase;
			if ((simpleEventData = data as SimpleEventData) != null && (entityBase = simpleEventData.objValue as EntityBase) != null)
			{
				entityBase.GetBuff(new Buff_BrokenArmor(entityBase, 1));
			}
		}
	}

	public class LW_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerStoringForce, OnPlayerStoringForce);
		}

		private void OnPlayerStoringForce(EventData data)
		{
			Player player = Singleton<GameManager>.Instance.Player;
			int num = player.PlayerAttr.SpecialAttr / 3;
			if (num > 0)
			{
				player.GetBuff(new Buff_DamageRestrik(player, num));
			}
		}
	}

	public class LW_SuitEffect_7 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponPlayerAtkEnemy;

		public override void TakeEffect(BattleEffectData data)
		{
			SimpleEffectData simpleEffectData;
			EntityBase entityBase;
			if ((simpleEffectData = data as SimpleEffectData) != null && (entityBase = simpleEffectData.objData as EntityBase) != null && Singleton<GameManager>.Instance.BattleSystem.BuffSystem.IsEntityGetBuff(entityBase, BuffType.Buff_BrokenArmor))
			{
				BaseBattleEffect.BattleEffectAtkEntity(entityBase, 4, isAbsDmg: true);
			}
		}
	}

	public override SuitType SuitType => SuitType.LW_Knight;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new LW_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new LW_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new LW_SuitEffect_7());
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
