using System;

public class STKnightSuit : Suit
{
	public class ST_SuitEffect_3 : BaseBattleEffect
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
			if (player.PlayerAttr.SpecialAttr >= 5)
			{
				player.PlayerAttr.AddArmor(5);
			}
		}
	}

	public class ST_SuitEffect_5 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponUsingSkillCardPowUp;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			base.TakeEffect(data, out IntData);
			SimpleEffectData simpleEffectData;
			if ((simpleEffectData = data as SimpleEffectData) != null && simpleEffectData.strData == "S_K_0")
			{
				IntData = Singleton<GameManager>.Instance.Player.PlayerAttr.SpecialAttr;
			}
			else
			{
				IntData = 0;
			}
		}
	}

	public class ST_SuitEffect_7 : BaseBattleEffect
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
			int playerCurrentCardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.PlayerCurrentCardAmount;
			Singleton<GameManager>.Instance.Player.PlayerAttr.AddSpecialAttr(playerCurrentCardAmount);
		}
	}

	public override SuitType SuitType => SuitType.ST_Knight;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		switch (suitInfomation.Count)
		{
		case 3:
			AddSuitBattleEffect(3, new ST_SuitEffect_3());
			break;
		case 5:
			AddSuitBattleEffect(5, new ST_SuitEffect_5());
			break;
		case 7:
			AddSuitBattleEffect(7, new ST_SuitEffect_7());
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
