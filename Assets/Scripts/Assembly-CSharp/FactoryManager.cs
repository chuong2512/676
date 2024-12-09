using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class FactoryManager
{
	private static Dictionary<string, UsualCard> usualCardMap = new Dictionary<string, UsualCard>();

	private static Dictionary<string, SkillCard> skillCardMap = new Dictionary<string, SkillCard>();

	private static Dictionary<string, EquipmentCard> equipmentCardMap = new Dictionary<string, EquipmentCard>();

	public static UsualCard GetUsualCard(string cardCode)
	{
		UsualCard value = null;
		if (usualCardMap.TryGetValue(cardCode, out value))
		{
			return value;
		}
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (usualCardAttr.IsNull())
		{
			Debug.LogError("Can not get usual card instance : " + cardCode);
			return null;
		}
		string typeName = ((usualCardAttr.HandFlag == HandFlag.BothHand) ? "SpecialUsualCard_" : "UsualCard_") + cardCode;
		value = Assembly.GetExecutingAssembly().CreateInstance(typeName, ignoreCase: false, BindingFlags.Default, null, new object[1] { usualCardAttr }, null, null) as UsualCard;
		if (value != null)
		{
			usualCardMap[cardCode] = value;
			return value;
		}
		Debug.LogError("Can not get usual card instance : " + cardCode);
		return null;
	}

	public static SkillCard GetSkillCard(PlayerOccupation playerOccupation, string cardCode)
	{
		SkillCard value = null;
		if (skillCardMap.TryGetValue(cardCode, out value))
		{
			return value;
		}
		string typeName = "SkillCard_" + cardCode;
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, cardCode);
		value = Assembly.GetExecutingAssembly().CreateInstance(typeName, ignoreCase: false, BindingFlags.Default, null, new object[1] { skillCardAttr }, null, null) as SkillCard;
		if (value != null)
		{
			skillCardMap[cardCode] = value;
			return value;
		}
		Debug.LogError("Can not get skill card instance : " + cardCode);
		return null;
	}

	public static EquipmentCard GetEquipmentCard(string cardCode)
	{
		EquipmentCard value = null;
		if (equipmentCardMap.TryGetValue(cardCode, out value))
		{
			return value;
		}
		string typeName = "EquipCard_" + cardCode;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(cardCode);
		value = Assembly.GetExecutingAssembly().CreateInstance(typeName, ignoreCase: false, BindingFlags.Default, null, new object[1] { equipmentCardAttr }, null, null) as EquipmentCard;
		if (value != null)
		{
			equipmentCardMap[cardCode] = value;
			return value;
		}
		Debug.LogError("Can not get equipment card instance : " + cardCode);
		return null;
	}

	public static ProphesyCard GetProphesyCard(string cardCode)
	{
		string typeName = "ProphesyCard_" + cardCode;
		ProphesyCard prophesyCard = Assembly.GetExecutingAssembly().CreateInstance(typeName, ignoreCase: false, BindingFlags.Default, null, null, null, null) as ProphesyCard;
		if (prophesyCard != null)
		{
			return prophesyCard;
		}
		Debug.LogError("Can not get prophesy card instance : " + cardCode);
		return null;
	}

	public static IEnumerable<BaseGameEvent> GetAllGameEventInstance()
	{
		return from t in typeof(BaseGameEvent).Assembly.GetTypes()
			where typeof(BaseGameEvent).IsAssignableFrom(t)
			where !t.IsAbstract && t.IsClass
			select (BaseGameEvent)Activator.CreateInstance(t);
	}

	public static IEnumerable<BaseGift> GetAllGiftInstance()
	{
		return from t in typeof(BaseGift).Assembly.GetTypes()
			where typeof(BaseGift).IsAssignableFrom(t)
			where !t.IsAbstract && t.IsClass
			select (BaseGift)Activator.CreateInstance(t);
	}
}
