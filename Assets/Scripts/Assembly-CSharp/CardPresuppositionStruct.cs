using System;
using System.Collections.Generic;

[Serializable]
public class CardPresuppositionStruct
{
	public bool isDefault;

	public string Name;

	public int index;

	public Dictionary<string, int> MainHandcards;

	public Dictionary<string, int> SupHandCards;

	public CardPresuppositionStruct(bool isDefault, string name, int index)
	{
		this.isDefault = isDefault;
		this.index = index;
		Name = name;
		MainHandcards = new Dictionary<string, int>();
		SupHandCards = new Dictionary<string, int>();
	}

	public CardPresuppositionStruct(bool isDefault, string name, int index, Dictionary<string, int> mainhands, Dictionary<string, int> suphands)
	{
		this.isDefault = isDefault;
		Name = name;
		this.index = index;
		MainHandcards = mainhands;
		SupHandCards = suphands;
	}

	public CardPresuppositionStruct(CardPresuppositionStruct presuppositionStruct)
	{
		isDefault = presuppositionStruct.isDefault;
		index = presuppositionStruct.index;
		Name = presuppositionStruct.Name;
		MainHandcards = new Dictionary<string, int>(presuppositionStruct.MainHandcards);
		SupHandCards = new Dictionary<string, int>(presuppositionStruct.SupHandCards);
	}

	public void EquipMainHandCard(string cardCode, int amount)
	{
		if (MainHandcards.ContainsKey(cardCode))
		{
			MainHandcards[cardCode] += amount;
		}
		else
		{
			MainHandcards.Add(cardCode, amount);
		}
	}

	public int GetMainHandCardAmount(string cardCode)
	{
		if (!MainHandcards.TryGetValue(cardCode, out var value))
		{
			return 0;
		}
		return value;
	}

	public int GetSupHandCardAmount(string cardCode)
	{
		if (!SupHandCards.TryGetValue(cardCode, out var value))
		{
			return 0;
		}
		return value;
	}

	public void ReleaseMainHandCard(string cardCode, int amount)
	{
		if (MainHandcards.TryGetValue(cardCode, out var value))
		{
			value -= amount;
			if (value <= 0)
			{
				MainHandcards.Remove(cardCode);
			}
			else
			{
				MainHandcards[cardCode] = value;
			}
		}
	}

	public void EquipSupHandCard(string cardCode, int amount)
	{
		if (SupHandCards.ContainsKey(cardCode))
		{
			SupHandCards[cardCode] += amount;
		}
		else
		{
			SupHandCards.Add(cardCode, amount);
		}
	}

	public void RelaseSupHandCard(string cardCode, int amount)
	{
		if (SupHandCards.TryGetValue(cardCode, out var value))
		{
			value -= amount;
			if (value <= 0)
			{
				SupHandCards.Remove(cardCode);
			}
			else
			{
				SupHandCards[cardCode] = value;
			}
		}
	}
}
