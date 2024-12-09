using System;

public class NatureHugeSuit : Suit
{
	public class NatureHuge_SuitEffect_3 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.None;

		public override void OnAddEffect()
		{
			base.OnAddEffect();
			EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		}

		public override void OnRemoveEffect()
		{
			base.OnRemoveEffect();
			EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		}

		private void OnBattleEnd(EventData data)
		{
			Singleton<GameManager>.Instance.Player.PlayerAttr.VarifyMaxHealth(3);
		}
	}

	public override SuitType SuitType => SuitType.NatureHuge;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		int count = suitInfomation.Count;
		if (count == 3)
		{
			AddSuitBattleEffect(3, new NatureHuge_SuitEffect_3());
		}
	}

	public override void RemoveSuit(string equipCode)
	{
		if (!suitInfomation.Remove(equipCode))
		{
			throw new ArgumentException("Value does not contain in hash set");
		}
		int count = suitInfomation.Count;
		if (count == 2)
		{
			RemoveSuitBattleEffect(3);
		}
	}
}
