using System.Collections.Generic;
using UnityEngine;

public class AllRandomInventory
{
	private static AllRandomInventory _instance;

	private Dictionary<EquipmentType, List<string>> allPlayerEquipRandomInventory;

	private readonly Dictionary<PlayerOccupation, List<SuitType>> PlayerOccupationToEpicSuitType = new Dictionary<PlayerOccupation, List<SuitType>>
	{
		{
			PlayerOccupation.Knight,
			new List<SuitType>(4)
			{
				SuitType.YZ_Knight,
				SuitType.TW_Knight,
				SuitType.LW_Knight,
				SuitType.ST_Knight
			}
		},
		{
			PlayerOccupation.Archer,
			new List<SuitType>(4)
			{
				SuitType.ArrowCreator,
				SuitType.Sniper,
				SuitType.PoisonShooter,
				SuitType.PhantomRanger
			}
		}
	};

	private List<string> allPlayerSkillRandomInventory;

	private Dictionary<string, SpecialUsualCardAttr> allPlayerSpecialCardRandomInventory;

	public static AllRandomInventory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AllRandomInventory();
			}
			return _instance;
		}
	}

	private AllRandomInventory()
	{
	}

	public void InitPlayerRandomInventory(PlayerOccupation playerOccupation, bool removeInit)
	{
		InitPlayerEquipRandomInventory(playerOccupation, removeInit);
		InitPlayerSkillRandomInventory(playerOccupation, removeInit);
		InitPlayerSpecialUsualCardRandomInventory();
	}

	private void InitPlayerEquipRandomInventory(PlayerOccupation playerOccupation, bool removeInit)
	{
		allPlayerEquipRandomInventory = new Dictionary<EquipmentType, List<string>>();
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.Helmet, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.Breastplate, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.Glove, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.Ornament, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.Shoes, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.MainHandWeapon, playerOccupation);
		InitPlayerSpecificEquipTypeRandomInventory(EquipmentType.SupHandWeapon, playerOccupation);
		if (removeInit)
		{
			RemovePlayerInitEquip(playerOccupation);
		}
	}

	private void RemovePlayerInitEquip(PlayerOccupation playerOccupation)
	{
		OccupationInitSetting playerOccupationInitSetting = DataManager.Instance.GetPlayerOccupationInitSetting(playerOccupation);
		allPlayerEquipRandomInventory[EquipmentType.Helmet].Remove(playerOccupationInitSetting.InitHelmet);
		allPlayerEquipRandomInventory[EquipmentType.Breastplate].Remove(playerOccupationInitSetting.InitBreastplate);
		allPlayerEquipRandomInventory[EquipmentType.Glove].Remove(playerOccupationInitSetting.InitGlove);
		allPlayerEquipRandomInventory[EquipmentType.Ornament].Remove(playerOccupationInitSetting.InitTrinket);
		allPlayerEquipRandomInventory[EquipmentType.Shoes].Remove(playerOccupationInitSetting.InitShoes);
		allPlayerEquipRandomInventory[EquipmentType.MainHandWeapon].Remove(playerOccupationInitSetting.InitMainhand);
		allPlayerEquipRandomInventory[EquipmentType.SupHandWeapon].Remove(playerOccupationInitSetting.InitSuphand);
	}

	private void InitPlayerSpecificEquipTypeRandomInventory(EquipmentType equipmentType, PlayerOccupation playerOccupation)
	{
		allPlayerEquipRandomInventory[equipmentType] = new List<string>();
		foreach (KeyValuePair<string, EquipmentCardAttr> singleTypeEquipmentCardData in DataManager.Instance.GetSingleTypeEquipmentCardDatas(equipmentType))
		{
			if ((singleTypeEquipmentCardData.Value.Occupation == playerOccupation || singleTypeEquipmentCardData.Value.Occupation == PlayerOccupation.None) && (!singleTypeEquipmentCardData.Value.IsNeedPurchased || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsEquipmentPurchased(singleTypeEquipmentCardData.Value.CardCode)))
			{
				allPlayerEquipRandomInventory[equipmentType].Add(singleTypeEquipmentCardData.Key);
			}
		}
	}

	public void InitPlayerEquipRandomInventory(EquipmentType equipmentType, List<string> allGottenEquips, string equipedCode)
	{
		for (int i = 0; i < allGottenEquips.Count; i++)
		{
			allPlayerEquipRandomInventory[equipmentType].Remove(allGottenEquips[i]);
		}
		allPlayerEquipRandomInventory[equipmentType].Remove(equipedCode);
	}

	public void AddEquipment(string equipCode, EquipmentType equipmentType)
	{
		allPlayerEquipRandomInventory[equipmentType].Add(equipCode);
	}

	public void RemoveEquipment(string equipCode, EquipmentType equipmentType)
	{
		allPlayerEquipRandomInventory[equipmentType].Remove(equipCode);
	}

	public void RemoveEquipment(string equipCode)
	{
		RemoveEquipment(equipCode, DataManager.Instance.GetEquipmentCardAttr(equipCode).EquipmentType);
	}

	public List<string> AllSatisfiedEquipsPlayerNotHave(int sourceLimit)
	{
		if (sourceLimit > 64)
		{
			return new List<string>(1);
		}
		Player player = Singleton<GameManager>.Instance.Player;
		List<string> list = new List<string>();
		EquipmentType preEquipType = EquipmentType.Breastplate;
		if (Random.value <= 0.1f)
		{
			preEquipType = EquipmentType.Ornament;
			list = AllSatisfiedEquipsByType(EquipmentType.Ornament, sourceLimit);
		}
		else
		{
			Dictionary<EquipmentType, float> dictionary = new Dictionary<EquipmentType, float>(6)
			{
				{
					EquipmentType.Helmet,
					Mathf.Pow(0.5f, player.PlayerInventory.AllHelmets.Count)
				},
				{
					EquipmentType.Breastplate,
					Mathf.Pow(0.5f, player.PlayerInventory.AllBreasplates.Count)
				},
				{
					EquipmentType.Glove,
					Mathf.Pow(0.5f, player.PlayerInventory.AllGloves.Count)
				},
				{
					EquipmentType.Shoes,
					Mathf.Pow(0.5f, player.PlayerInventory.AllShoes.Count)
				},
				{
					EquipmentType.MainHandWeapon,
					Mathf.Pow(0.5f, player.PlayerInventory.AllMainHands.Count)
				},
				{
					EquipmentType.SupHandWeapon,
					Mathf.Pow(0.5f, player.PlayerInventory.AllSupHands.Count)
				}
			};
			float num = 0f;
			foreach (KeyValuePair<EquipmentType, float> item in dictionary)
			{
				num += item.Value;
			}
			float value = Random.value;
			float num2 = 0f;
			foreach (KeyValuePair<EquipmentType, float> item2 in dictionary)
			{
				float num3 = item2.Value / num;
				num2 += num3;
				if (num2 > value)
				{
					preEquipType = item2.Key;
					list = AllSatisfiedEquipsByType(item2.Key, sourceLimit);
					break;
				}
			}
		}
		if (list.Count != 0)
		{
			return list;
		}
		return AllSatisfiedEquipsPlayerNotHave(preEquipType, sourceLimit);
	}

	private List<string> AllSatisfiedEquipsPlayerNotHave(EquipmentType preEquipType, int sourceLimit)
	{
		List<EquipmentType> list = new List<EquipmentType>(7)
		{
			EquipmentType.Helmet,
			EquipmentType.Breastplate,
			EquipmentType.Glove,
			EquipmentType.Ornament,
			EquipmentType.Shoes,
			EquipmentType.MainHandWeapon,
			EquipmentType.SupHandWeapon
		};
		list.Remove(preEquipType);
		while (list.Count > 0)
		{
			EquipmentType equipmentType = list[Random.Range(0, list.Count)];
			List<string> list2 = AllSatisfiedEquipsByType(equipmentType, sourceLimit);
			if (list2.Count == 0)
			{
				list.Remove(equipmentType);
				continue;
			}
			return list2;
		}
		return AllSatisfiedEquipsPlayerNotHave(sourceLimit << 1);
	}

	private List<string> AllSatisfiedEquipsByType(EquipmentType equipType, int sourceLimit)
	{
		List<string> list = new List<string>();
		int num = 1 << Singleton<GameManager>.Instance.CurrentMapLevel - 1;
		foreach (string item in allPlayerEquipRandomInventory[equipType])
		{
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item, equipType);
			if ((equipmentCardAttr.SourceLimit & sourceLimit) > 0 && (num & equipmentCardAttr.StageLimit) > 0)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<string> AllSatisfiedRandomOrnament()
	{
		List<string> list = AllSatisfiedEquipsByType(EquipmentType.Ornament, 2);
		if (list.Count > 0)
		{
			return list;
		}
		return AllSatisfiedEquipsByType(EquipmentType.Ornament, 32);
	}

	public List<string> AllSatisfiedEpicSuitEquips(PlayerOccupation playerOccupation)
	{
		Player player = Singleton<GameManager>.Instance.Player;
		List<SuitType> list = PlayerOccupationToEpicSuitType[playerOccupation];
		Dictionary<SuitType, float> dictionary = new Dictionary<SuitType, float>();
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			SuitHandler.SuitInfo suitInfo = SuitHandler.GetSuitInfo(list[i]);
			int count = player.PlayerEquipment.SuitHandler.GetContainSuits(list[i]).Count;
			if (suitInfo.SuitEquips.Count != count)
			{
				float num2 = Mathf.Pow(2f, count);
				dictionary.Add(list[i], num2);
				num += num2;
			}
		}
		float value = Random.value;
		float num3 = 0f;
		SuitType suitType = SuitType.None;
		foreach (KeyValuePair<SuitType, float> item in dictionary)
		{
			float num4 = item.Value / num;
			num3 += num4;
			if (num3 > value)
			{
				suitType = item.Key;
				break;
			}
		}
		List<string> list2 = new List<string>();
		SuitHandler.SuitInfo suitInfo2 = SuitHandler.GetSuitInfo(suitType);
		for (int j = 0; j < suitInfo2.SuitEquips.Count; j++)
		{
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(suitInfo2.SuitEquips[j]);
			if (allPlayerEquipRandomInventory[equipmentCardAttr.EquipmentType].Contains(suitInfo2.SuitEquips[j]))
			{
				list2.Add(suitInfo2.SuitEquips[j]);
			}
		}
		if (list2.Count == 0)
		{
			list2 = AllSatisfiedEquipsPlayerNotHave(int.MaxValue);
		}
		return list2;
	}

	public List<string> AllSatisfiedEpicSuitEquips(PlayerOccupation playerOccupation, EquipmentType equipmentType)
	{
		List<string> list = new List<string>();
		foreach (string item in allPlayerEquipRandomInventory[equipmentType])
		{
			EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item);
			if ((equipmentCardAttr.SourceLimit & 0x20) > 0 && equipmentCardAttr.Occupation == playerOccupation)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public Dictionary<int, List<string>> AllSatisfiedEquipsWithMapLevel(int sourceLimit)
	{
		Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>
		{
			{
				1,
				new List<string>()
			},
			{
				2,
				new List<string>()
			},
			{
				3,
				new List<string>()
			}
		};
		foreach (KeyValuePair<EquipmentType, List<string>> item in allPlayerEquipRandomInventory)
		{
			foreach (string item2 in item.Value)
			{
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item2, item.Key);
				if ((equipmentCardAttr.SourceLimit & sourceLimit) > 0)
				{
					dictionary[equipmentCardAttr.ShopLimit].Add(item2);
				}
			}
		}
		return dictionary;
	}

	public List<string> AllSatisfiedEquipsWithStageLimit(int sourceLimit, int stageLimit)
	{
		List<string> list = new List<string>();
		int num = 1 << stageLimit - 1;
		foreach (KeyValuePair<EquipmentType, List<string>> item in allPlayerEquipRandomInventory)
		{
			foreach (string item2 in item.Value)
			{
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item2, item.Key);
				if ((equipmentCardAttr.SourceLimit & sourceLimit) > 0 && (num & equipmentCardAttr.StageLimit) > 0)
				{
					list.Add(item2);
				}
			}
		}
		return list;
	}

	public List<string> AllStatisfiedConditionEquips(int sourceLimit, EquipmentType type)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<EquipmentType, List<string>> item in allPlayerEquipRandomInventory)
		{
			foreach (string item2 in item.Value)
			{
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(item2, item.Key);
				if ((equipmentCardAttr.SourceLimit & sourceLimit) > 0 && equipmentCardAttr.EquipmentType == type)
				{
					list.Add(item2);
				}
			}
		}
		return list;
	}

	public List<string> AllHaventGetEquips()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<EquipmentType, List<string>> item in allPlayerEquipRandomInventory)
		{
			foreach (string item2 in item.Value)
			{
				list.Add(item2);
			}
		}
		return list;
	}

	private void InitPlayerSkillRandomInventory(PlayerOccupation playerOccupation, bool removeInit)
	{
		allPlayerSkillRandomInventory = new List<string>(SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSkill[playerOccupation]);
		for (int i = 0; i < allPlayerSkillRandomInventory.Count; i++)
		{
			SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, allPlayerSkillRandomInventory[i]);
			if (skillCardAttr.IsNeedPurchased && !SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSkillPurchased(skillCardAttr.CardCode))
			{
				allPlayerSkillRandomInventory.RemoveAt(i);
				i--;
			}
		}
		if (removeInit)
		{
			RemovePlayerInitSkill(playerOccupation);
		}
	}

	private void RemovePlayerInitSkill(PlayerOccupation playerOccupation)
	{
		OccupationInitSetting playerOccupationInitSetting = DataManager.Instance.GetPlayerOccupationInitSetting(playerOccupation);
		for (int i = 0; i < playerOccupationInitSetting.InitSkillArray.Length; i++)
		{
			allPlayerSkillRandomInventory.Remove(playerOccupationInitSetting.InitSkillArray[i]);
		}
	}

	public void InitPlayerSkillRandomInventory(List<string> allGottenSkills, List<string> allEquipedSkills)
	{
		for (int i = 0; i < allGottenSkills.Count; i++)
		{
			allPlayerSkillRandomInventory.Remove(allGottenSkills[i]);
		}
		for (int j = 0; j < allEquipedSkills.Count; j++)
		{
			allPlayerSkillRandomInventory.Remove(allEquipedSkills[j]);
		}
	}

	public List<string> AllStatisfiedConditionSkills(int levelLimit, PlayerOccupation playerOccupation)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < allPlayerSkillRandomInventory.Count; i++)
		{
			SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, allPlayerSkillRandomInventory[i]);
			if (levelLimit > skillCardAttr.LevelLimit)
			{
				list.Add(allPlayerSkillRandomInventory[i]);
			}
		}
		return list;
	}

	public void RemovePlayerSkill(string skillCode)
	{
		allPlayerSkillRandomInventory.Remove(skillCode);
	}

	public void AddPlayerSkill(string skillCode)
	{
		allPlayerSkillRandomInventory.Add(skillCode);
	}

	private void InitPlayerSpecialUsualCardRandomInventory()
	{
		allPlayerSpecialCardRandomInventory = new Dictionary<string, SpecialUsualCardAttr>();
		Dictionary<string, SpecialUsualCardAttr> allSpecialUsualCardDatas = DataManager.Instance.GetAllSpecialUsualCardDatas();
		if (allSpecialUsualCardDatas == null)
		{
			return;
		}
		foreach (KeyValuePair<string, SpecialUsualCardAttr> item in allSpecialUsualCardDatas)
		{
			if (!item.Value.IsNeedPurchased || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSpecialUsualCardPurchased(item.Key))
			{
				allPlayerSpecialCardRandomInventory.Add(item.Key, item.Value);
			}
		}
	}

	public void InitPlayerSpecialCardRandomInventory(Dictionary<string, int> allGottenDic, Dictionary<string, int> allEquipedMainDic, Dictionary<string, int> allEquipedSupDic)
	{
		InitPlayerSpecialUsualCardRandomInventory();
		TryRemovePlayerEverGottenOnlyOneAttrCard(allGottenDic);
		TryRemovePlayerEverGottenOnlyOneAttrCard(allEquipedMainDic);
		TryRemovePlayerEverGottenOnlyOneAttrCard(allEquipedSupDic);
	}

	private void TryRemovePlayerEverGottenOnlyOneAttrCard(Dictionary<string, int> tmpDic)
	{
		foreach (KeyValuePair<string, int> item in tmpDic)
		{
			SpecialUsualCardAttr specialUsualCardAttr = DataManager.Instance.GetSpecialUsualCardAttr(item.Key);
			if (specialUsualCardAttr != null && specialUsualCardAttr.IsOnlyOne)
			{
				allPlayerSpecialCardRandomInventory.Remove(item.Key);
			}
		}
	}

	public List<string> AllStatisfiedSpecialUsualCards(int sourceLimit)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, SpecialUsualCardAttr> item in allPlayerSpecialCardRandomInventory)
		{
			if ((item.Value.SourceLimit & sourceLimit) > 0)
			{
				list.Add(item.Key);
			}
		}
		return list;
	}

	public void RemoveSpecialUsualCard(string cardCode)
	{
		allPlayerSpecialCardRandomInventory.Remove(cardCode);
	}
}
