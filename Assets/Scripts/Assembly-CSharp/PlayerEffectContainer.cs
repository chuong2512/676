using System.Collections.Generic;

public class PlayerEffectContainer
{
	private Dictionary<BattleEffectType, List<BaseBattleEffect>> allBattleEffectContainer = new Dictionary<BattleEffectType, List<BaseBattleEffect>>();

	public void ClearBattleEffect()
	{
		allBattleEffectContainer.Clear();
	}

	public void AddBattleEffect(BaseBattleEffect effect)
	{
		if (allBattleEffectContainer.TryGetValue(effect.BattleEffectType, out var value))
		{
			if (!value.Contains(effect))
			{
				value.Add(effect);
				effect.OnAddEffect();
			}
		}
		else
		{
			value = new List<BaseBattleEffect> { effect };
			allBattleEffectContainer[effect.BattleEffectType] = value;
			effect.OnAddEffect();
		}
	}

	public void RemoveBattleEffect(BaseBattleEffect effect)
	{
		if (allBattleEffectContainer.TryGetValue(effect.BattleEffectType, out var value) && value.Remove(effect))
		{
			effect.OnRemoveEffect();
			if (value.Count == 0)
			{
				allBattleEffectContainer.Remove(effect.BattleEffectType);
			}
		}
	}

	public void TakeEffect(BattleEffectType type, BattleEffectData data)
	{
		List<BaseBattleEffect> value = null;
		if (allBattleEffectContainer.TryGetValue(type, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].TakeEffect(data);
			}
		}
	}

	public void TakeEffect(BattleEffectType type, BattleEffectData data, out int IntData)
	{
		List<BaseBattleEffect> value = null;
		int num = 0;
		if (allBattleEffectContainer.TryGetValue(type, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].TakeEffect(data, out int IntData2);
				num += IntData2;
			}
		}
		IntData = num;
	}

	public void TakeEffect(BattleEffectType type, BattleEffectData data, out float FloatData)
	{
		List<BaseBattleEffect> value = null;
		float num = 0f;
		if (allBattleEffectContainer.TryGetValue(type, out value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].TakeEffect(data, out float FloatData2);
				num += FloatData2;
			}
		}
		FloatData = num;
	}

	public void TakeEffect(BattleEffectType type, BattleEffectData data, out int IntData, out Dictionary<string, int> detail)
	{
		int num = 0;
		detail = new Dictionary<string, int>();
		if (allBattleEffectContainer.TryGetValue(type, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				value[i].TakeEffect(data, out var IntData2, out var source);
				num += IntData2;
				if (IntData2 != 0)
				{
					detail.Add(source, IntData2);
				}
			}
		}
		IntData = num;
	}
}
