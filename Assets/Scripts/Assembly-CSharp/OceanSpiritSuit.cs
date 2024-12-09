using System;

public class OceanSpiritSuit : Suit
{
	private class OceanSpirit_SuitEffect_2 : BaseBattleEffect
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
			PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
			if ((float)playerAttr.Health / (float)playerAttr.MaxHealth >= 0.5f)
			{
				playerAttr.RecoveryApAmount(1);
			}
		}
	}

	private class OceanSpirit_SuitEffect_3 : BaseBattleEffect
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
			if ((float)player.PlayerAttr.Health / (float)player.PlayerAttr.MaxHealth >= 0.5f)
			{
				player.GetBuff(new Buff_Power(player, 1));
			}
		}
	}

	private class OceanSpirit_SuitEffect_4 : BaseBattleEffect
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
			if ((float)player.PlayerAttr.Health / (float)player.PlayerAttr.MaxHealth >= 0.5f)
			{
				player.GetBuff(new Buff_Dodge(player, 1));
			}
		}
	}

	public override SuitType SuitType => SuitType.OceanSpirit;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 2:
			AddSuitBattleEffect(2, new OceanSpirit_SuitEffect_2());
			break;
		case 3:
			AddSuitBattleEffect(3, new OceanSpirit_SuitEffect_3());
			break;
		case 4:
			AddSuitBattleEffect(4, new OceanSpirit_SuitEffect_4());
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
