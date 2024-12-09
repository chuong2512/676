using System;

public class YZKnightSuit : Suit
{
	public class YZ_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_EnemyRound, OnEndPlayerRound);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_EnemyRound, OnEndPlayerRound);
		}

		private void OnEndPlayerRound(EventData data)
		{
			Player player = Singleton<GameManager>.Instance.Player;
			if (player.PlayerBattleInfo.SupHandCardAmount >= 4)
			{
				player.GetBuff(new Buff_JianShou(player, 1));
			}
		}
	}

	public class YZ_SuitEffect_5 : BaseBattleEffect
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
			Player player = Singleton<GameManager>.Instance.Player;
			if (player.PlayerBattleInfo.MainHandCardAmount >= 4)
			{
				player.GetBuff(new Buff_Power(player, 1));
			}
		}
	}

	public class YZ_SuitEffect_7 : BaseBattleEffect
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
			Player player = Singleton<GameManager>.Instance.Player;
			int mainHandCardAmount = player.PlayerBattleInfo.MainHandCardAmount;
			if (mainHandCardAmount >= 8)
			{
				player.GetBuff(new Buff_DefenceToAttack(player, 3));
			}
			else if (mainHandCardAmount >= 6)
			{
				player.GetBuff(new Buff_DefenceToAttack(player, 2));
			}
			else if (mainHandCardAmount >= 4)
			{
				player.GetBuff(new Buff_DefenceToAttack(player, 1));
			}
		}
	}

	public override SuitType SuitType => SuitType.YZ_Knight;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in Hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new YZ_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new YZ_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new YZ_SuitEffect_7());
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
			throw new ArgumentException("Value does not contain in Hash set");
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
