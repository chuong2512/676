using System;

public class ThiefSuit : Suit
{
	public class Thief_SuitEffect_4 : BaseBattleEffect
	{
		public override BattleEffectType BattleEffectType => BattleEffectType.UponPlayerGetSpecialCardExtraAmount;

		public override void TakeEffect(BattleEffectData data, out int IntData)
		{
			IntData = 1;
		}
	}

	public override SuitType SuitType => SuitType.Thief;

	public override void AddSuit(string equipCode)
	{
		if (!suitInfomation.Add(equipCode))
		{
			throw new ArgumentException("Same value in hash set");
		}
		int count = suitInfomation.Count;
		if (count == 4)
		{
			AddSuitBattleEffect(4, new Thief_SuitEffect_4());
		}
	}

	public override void RemoveSuit(string equipCode)
	{
		if (!suitInfomation.Remove(equipCode))
		{
			throw new ArgumentException("Value does not contain in hash set");
		}
		int count = suitInfomation.Count;
		if (count == 3)
		{
			RemoveSuitBattleEffect(4);
		}
	}

	public override void OnBattleStart()
	{
	}
}
