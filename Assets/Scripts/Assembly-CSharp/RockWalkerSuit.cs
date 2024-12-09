using System;

public class RockWalkerSuit : Suit
{
	private class RockWalker_SuitEffect_2 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		private void OnPlayerEquipRound(EventData data)
		{
			if (Singleton<GameManager>.Instance.Player.PlayerAttr.Armor >= 8)
			{
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawMainHandCards(1);
				Singleton<GameManager>.Instance.Player.PlayerBattleInfo.TryDrawSupHandCards(1);
			}
		}
	}

	private class RockWalker_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		private void OnPlayerEquipRound(EventData data)
		{
			if (Singleton<GameManager>.Instance.Player.PlayerAttr.Armor >= 8)
			{
				Singleton<GameManager>.Instance.Player.PlayerAttr.RecoveryApAmount(1);
			}
		}
	}

	private class RockWalker_SuitEffect_4 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_PlayerEquipRound, OnPlayerEquipRound);
		}

		private void OnPlayerEquipRound(EventData data)
		{
			if (Singleton<GameManager>.Instance.Player.PlayerAttr.Armor >= 8)
			{
				Singleton<GameManager>.Instance.Player.GetBuff(new Buff_Agile(Singleton<GameManager>.Instance.Player, 1));
			}
		}
	}

	public override SuitType SuitType => SuitType.RockWalker;

	public override void OnBattleStart()
	{
		if (suitBattleEffect.Count != 0)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("BattleUI") as BattleUI).AddSuitEffet(SuitType, string.Empty, base.GetSuitBattleEffectDescription).SetEffect();
		}
	}

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 2:
			AddSuitBattleEffect(2, new RockWalker_SuitEffect_2());
			break;
		case 3:
			AddSuitBattleEffect(3, new RockWalker_SuitEffect_3());
			break;
		case 4:
			AddSuitBattleEffect(4, new RockWalker_SuitEffect_4());
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
		case 3:
			RemoveSuitBattleEffect(4);
			break;
		case 2:
			RemoveSuitBattleEffect(3);
			break;
		case 1:
			RemoveSuitBattleEffect(2);
			break;
		}
	}
}
