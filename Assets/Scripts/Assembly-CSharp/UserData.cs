using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
	public List<RecordData> AllRecordDatas = new List<RecordData>();

	public Dictionary<PlayerOccupation, Dictionary<string, int>> AllUnlockedMainHandCards;

	public Dictionary<PlayerOccupation, Dictionary<string, int>> AllUnlockedSupHandCards;

	public Dictionary<PlayerOccupation, List<CardPresuppositionStruct>> AllInitCardPresupposition;

	public Dictionary<PlayerOccupation, List<string>> AllUnlockedSkill;

	public HashSet<string> AllUnlockedNormalMonsterIllus = new HashSet<string>();

	public HashSet<string> AllUnlockedBossMonsterIllus = new HashSet<string>();

	public HashSet<string> AllUnlockedEquipmentIllus = new HashSet<string>();

	public HashSet<string> AllUnlockedSkillIllus = new HashSet<string>();

	public HashSet<string> AllUnlockedSpecialCardIllus = new HashSet<string>();

	public HashSet<string> AllUnlockedPlot;

	public HashSet<string> AllPurchasedSkills;

	public HashSet<string> AllPurchasedEquipments;

	public HashSet<string> AllPurchasedSpecialUsualCards;

	public string GameVersion { get; }

	public int UserDataIndex { get; }

	public string UserName { get; private set; }

	public ulong UserPlaySeconds { get; private set; }

	public int UserRebirthCount { get; private set; }

	public int GameCoin { get; private set; }

	public int PreEndMapLevel { get; private set; }

	public bool IsEverFinishGame { get; set; }

	public bool IsUnlockPrephesyCard { get; set; }

	public bool IsDefeatEvilDrag { get; set; }

	public List<string> allTriggeredGameGuideCodes { get; private set; }

	public int UnlockedPlotAmount => AllUnlockedPlot?.Count ?? 0;

	public UserData(string gameVersion, int userDataIndex, string userName)
	{
		GameVersion = gameVersion;
		UserDataIndex = userDataIndex;
		UserName = userName;
		PreEndMapLevel = 0;
		GameCoin = 0;
		UserPlaySeconds = 0uL;
		UserRebirthCount = 0;
		IsUnlockPrephesyCard = false;
		IsDefeatEvilDrag = false;
		LoadOccupationInitSetting();
	}

	public void VerifyUserDataIntegrity()
	{
		foreach (KeyValuePair<PlayerOccupation, OccupationInitSetting> allPlayerOccupationInitSetting in DataManager.Instance.GetAllPlayerOccupationInitSettings())
		{
			if (!AllUnlockedMainHandCards.ContainsKey(allPlayerOccupationInitSetting.Key))
			{
				AddSingleOccpuationInitCardSetting(allPlayerOccupationInitSetting.Key, allPlayerOccupationInitSetting.Value);
				AddSingleOccupationInitUnlockedSkillList(allPlayerOccupationInitSetting.Key, AllUnlockedMainHandCards[allPlayerOccupationInitSetting.Key]);
				AddSingleOccupationInitUnlockedSkillList(allPlayerOccupationInitSetting.Key, AllUnlockedSupHandCards[allPlayerOccupationInitSetting.Key]);
				AddSingleOccupationInitUnlockedEquipmentIllus(allPlayerOccupationInitSetting.Value);
				AddSingleOccupationInitUnlockedSkillIllus(allPlayerOccupationInitSetting.Value);
			}
		}
	}

	public void DefeatEvilDragon()
	{
		IsDefeatEvilDrag = true;
	}

	public void SetPreEndMapLevel(int level)
	{
		PreEndMapLevel = level;
	}

	public void SetUserName(string newName)
	{
		UserName = newName;
	}

	public void AddCoin(int amount, bool isAutoSave)
	{
		GameCoin += amount;
		if (isAutoSave)
		{
			GameSave.SaveUserData();
		}
		EventManager.BroadcastEvent(EventEnum.E_DarkCrystalChanged, null);
	}

	public void ComsumeCoin(int amount, bool isAutoSave)
	{
		GameCoin -= amount;
		if (isAutoSave)
		{
			GameSave.SaveUserData();
		}
		EventManager.BroadcastEvent(EventEnum.E_DarkCrystalChanged, null);
	}

	public bool IsUsualCardUnlocked(PlayerOccupation playerOccupation, string cardCode)
	{
		if (AllUnlockedMainHandCards[playerOccupation].ContainsKey(cardCode) || AllUnlockedSupHandCards[playerOccupation].ContainsKey(cardCode))
		{
			return true;
		}
		return false;
	}

	public void AddNewRecord(RecordData data)
	{
		if (AllRecordDatas.Count >= 100)
		{
			AllRecordDatas.RemoveAt(0);
		}
		AllRecordDatas.Add(data);
		AddUserRebirthCount();
		AddPlayTime((uint)data.timeUsed);
	}

	private void AddUserRebirthCount()
	{
		UserRebirthCount++;
	}

	private void AddPlayTime(uint seconds)
	{
		UserPlaySeconds += seconds;
	}

	public RecordData GetRecordDataByIndex(int index)
	{
		if (index < AllRecordDatas.Count)
		{
			return AllRecordDatas[index];
		}
		throw new ArgumentException("out of array bound");
	}

	public RecordData GetNewestRecordData()
	{
		return AllRecordDatas[AllRecordDatas.Count - 1];
	}

	private void LoadOccupationInitSetting()
	{
		Dictionary<PlayerOccupation, OccupationInitSetting> allPlayerOccupationInitSettings = DataManager.Instance.GetAllPlayerOccupationInitSettings();
		SetInitCardSetting(allPlayerOccupationInitSettings);
		SetInitUnlockedSkillList();
		SetInitUnlockedEquipmentIllus(allPlayerOccupationInitSettings);
		SetInitUnlockedSkillIllus(allPlayerOccupationInitSettings);
	}

	private void SetInitUnlockedSkillIllus(Dictionary<PlayerOccupation, OccupationInitSetting> allInitSetting)
	{
		foreach (KeyValuePair<PlayerOccupation, OccupationInitSetting> item in allInitSetting)
		{
			AddSingleOccupationInitUnlockedSkillIllus(item.Value);
		}
	}

	private void AddSingleOccupationInitUnlockedSkillIllus(OccupationInitSetting initSetting)
	{
		for (int i = 0; i < initSetting.InitSkillArray.Length; i++)
		{
			AllUnlockedSkillIllus.Add(initSetting.InitSkillArray[i]);
		}
	}

	private void SetInitUnlockedEquipmentIllus(Dictionary<PlayerOccupation, OccupationInitSetting> allInitSetting)
	{
		foreach (KeyValuePair<PlayerOccupation, OccupationInitSetting> item in allInitSetting)
		{
			AddSingleOccupationInitUnlockedEquipmentIllus(item.Value);
		}
	}

	private void AddSingleOccupationInitUnlockedEquipmentIllus(OccupationInitSetting initSetting)
	{
		AllUnlockedEquipmentIllus.Add(initSetting.InitHelmet);
		AllUnlockedEquipmentIllus.Add(initSetting.InitBreastplate);
		AllUnlockedEquipmentIllus.Add(initSetting.InitGlove);
		AllUnlockedEquipmentIllus.Add(initSetting.InitShoes);
		AllUnlockedEquipmentIllus.Add(initSetting.InitTrinket);
		AllUnlockedEquipmentIllus.Add(initSetting.InitMainhand);
		AllUnlockedEquipmentIllus.Add(initSetting.InitSuphand);
	}

	public CardPresuppositionStruct AddNewPresupposition(PlayerOccupation occupation, string presuppositionName)
	{
		CardPresuppositionStruct cardPresuppositionStruct = new CardPresuppositionStruct(AllInitCardPresupposition[occupation][0]);
		cardPresuppositionStruct.isDefault = false;
		cardPresuppositionStruct.Name = presuppositionName;
		cardPresuppositionStruct.index = AllInitCardPresupposition[occupation].Count;
		AllInitCardPresupposition[occupation].Add(cardPresuppositionStruct);
		return cardPresuppositionStruct;
	}

	public void UnlockUsualCard(PlayerOccupation playerOccupation, string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (usualCardAttr.HandFlag == HandFlag.MainHand)
		{
			AllUnlockedMainHandCards[playerOccupation].Add(cardCode, usualCardAttr.MaxCardAmount);
		}
		else
		{
			AllUnlockedSupHandCards[playerOccupation].Add(cardCode, usualCardAttr.MaxCardAmount);
		}
		List<string> list = DataManager.Instance.GetUusualCardUnlockConfig(playerOccupation).AllCardToSkills[cardCode];
		for (int i = 0; i < list.Count; i++)
		{
			AllUnlockedSkill[playerOccupation].Add(list[i]);
		}
	}

	public void UnlockAllUsualCard()
	{
		foreach (KeyValuePair<PlayerOccupation, DataManager.UsualUnlockData> item in DataManager.Instance.GetAlUsualCardUnlockConfig())
		{
			foreach (KeyValuePair<string, int> item2 in item.Value.AllCardTimeSpaceNeed)
			{
				UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(item2.Key);
				if (usualCardAttr.HandFlag == HandFlag.MainHand)
				{
					AllUnlockedMainHandCards[item.Key][item2.Key] = usualCardAttr.MaxCardAmount;
				}
				else
				{
					AllUnlockedSupHandCards[item.Key][item2.Key] = usualCardAttr.MaxCardAmount;
				}
			}
		}
	}

	public void SavePresupposition(PlayerOccupation occupation, CardPresuppositionStruct presupposition)
	{
		CardPresuppositionStruct cardPresuppositionByIndex = GetCardPresuppositionByIndex(occupation, presupposition.index);
		cardPresuppositionByIndex.MainHandcards = new Dictionary<string, int>(presupposition.MainHandcards);
		cardPresuppositionByIndex.SupHandCards = new Dictionary<string, int>(presupposition.SupHandCards);
	}

	public void DeletePresupposition(PlayerOccupation occupation, int index)
	{
		for (int i = index + 1; i < AllInitCardPresupposition[occupation].Count; i++)
		{
			AllInitCardPresupposition[occupation][i].index--;
		}
		AllInitCardPresupposition[occupation].RemoveAt(index);
	}

	public List<CardPresuppositionStruct> GetAllCardPresuppositionsByOccupation(PlayerOccupation playerOccupation)
	{
		return AllInitCardPresupposition[playerOccupation];
	}

	public void VarifyPresuppositionName(PlayerOccupation occupation, int index, string newName)
	{
		AllInitCardPresupposition[occupation][index].Name = newName;
	}

	public CardPresuppositionStruct GetCardPresuppositionByIndex(PlayerOccupation occupation, int index)
	{
		return AllInitCardPresupposition[occupation][index];
	}

	private void SetInitUnlockedSkillList()
	{
		AllUnlockedSkill = new Dictionary<PlayerOccupation, List<string>>();
		foreach (KeyValuePair<PlayerOccupation, Dictionary<string, int>> allUnlockedMainHandCard in AllUnlockedMainHandCards)
		{
			AddSingleOccupationInitUnlockedSkillList(allUnlockedMainHandCard.Key, allUnlockedMainHandCard.Value);
		}
		foreach (KeyValuePair<PlayerOccupation, Dictionary<string, int>> allUnlockedSupHandCard in AllUnlockedSupHandCards)
		{
			AddSingleOccupationInitUnlockedSkillList(allUnlockedSupHandCard.Key, allUnlockedSupHandCard.Value);
		}
	}

	private void AddSingleOccupationInitUnlockedSkillList(PlayerOccupation playerOccupation, Dictionary<string, int> handcardUnlocked)
	{
		if (!AllUnlockedSkill.TryGetValue(playerOccupation, out var value))
		{
			value = new List<string>();
		}
		DataManager.UsualUnlockData uusualCardUnlockConfig = DataManager.Instance.GetUusualCardUnlockConfig(playerOccupation);
		foreach (KeyValuePair<string, int> item in handcardUnlocked)
		{
			List<string> list = uusualCardUnlockConfig.AllCardToSkills[item.Key];
			for (int i = 0; i < list.Count; i++)
			{
				value.Add(list[i]);
			}
		}
		AllUnlockedSkill[playerOccupation] = value;
	}

	private void SetInitCardSetting(Dictionary<PlayerOccupation, OccupationInitSetting> allInitSetting)
	{
		AllUnlockedMainHandCards = new Dictionary<PlayerOccupation, Dictionary<string, int>>();
		AllUnlockedSupHandCards = new Dictionary<PlayerOccupation, Dictionary<string, int>>();
		AllInitCardPresupposition = new Dictionary<PlayerOccupation, List<CardPresuppositionStruct>>();
		foreach (KeyValuePair<PlayerOccupation, OccupationInitSetting> item in allInitSetting)
		{
			AddSingleOccpuationInitCardSetting(item.Key, item.Value);
		}
	}

	private void AddSingleOccpuationInitCardSetting(PlayerOccupation playerOccupation, OccupationInitSetting initSetting)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
		Dictionary<string, int> dictionary4 = new Dictionary<string, int>();
		for (int i = 0; i < initSetting.InitMainhandArray.Count; i++)
		{
			CardConfig cardConfig = initSetting.InitMainhandArray[i];
			UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardConfig.CardCode);
			dictionary[cardConfig.CardCode] = usualCardAttr.MaxCardAmount;
			dictionary3.Add(cardConfig.CardCode, cardConfig.CardAmount);
		}
		for (int j = 0; j < initSetting.InitSuphandArray.Count; j++)
		{
			CardConfig cardConfig2 = initSetting.InitSuphandArray[j];
			UsualCardAttr usualCardAttr2 = DataManager.Instance.GetUsualCardAttr(cardConfig2.CardCode);
			dictionary2[cardConfig2.CardCode] = usualCardAttr2.MaxCardAmount;
			dictionary4.Add(cardConfig2.CardCode, cardConfig2.CardAmount);
		}
		AllUnlockedMainHandCards[playerOccupation] = dictionary;
		AllUnlockedSupHandCards[playerOccupation] = dictionary2;
		AllInitCardPresupposition[playerOccupation] = new List<CardPresuppositionStruct>
		{
			new CardPresuppositionStruct(isDefault: true, "DefaultPresupposition", 0, dictionary3, dictionary4)
		};
	}

	public void TriggerGameGuideCode(string code)
	{
		if (allTriggeredGameGuideCodes == null)
		{
			allTriggeredGameGuideCodes = new List<string>();
		}
		allTriggeredGameGuideCodes.Add(code);
		GameSave.SaveUserData();
	}

	public bool IsGameGuideEverTrigger(string code)
	{
		if (allTriggeredGameGuideCodes == null)
		{
			return false;
		}
		return allTriggeredGameGuideCodes.Contains(code);
	}

	public void TryUnlockPlot(string plotCode)
	{
		if (AllUnlockedPlot == null)
		{
			AllUnlockedPlot = new HashSet<string>();
		}
		if (AllUnlockedPlot.Add(plotCode))
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsPlotUnlocked(string plotCode)
	{
		return AllUnlockedPlot?.Contains(plotCode) ?? false;
	}

	public void TryUnlockSpecialUsualCardIllus(string cardCode)
	{
		if (AllUnlockedSpecialCardIllus == null)
		{
			AllUnlockedSpecialCardIllus = new HashSet<string>();
		}
		if (AllUnlockedSpecialCardIllus.Add(cardCode))
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsSpecialUsualCardIlluUnlocked(string cardCode)
	{
		return AllUnlockedSpecialCardIllus?.Contains(cardCode) ?? false;
	}

	public void TryUnlockSkillIllu(string skillCode)
	{
		if (AllUnlockedSkillIllus == null)
		{
			AllUnlockedSkillIllus = new HashSet<string>();
		}
		if (AllUnlockedSkillIllus.Add(skillCode))
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsSkillIlluUnlocked(string skillCode)
	{
		return AllUnlockedSkillIllus?.Contains(skillCode) ?? false;
	}

	public void TryUnlockEquipmentIllu(string equipCode)
	{
		if (AllUnlockedEquipmentIllus == null)
		{
			AllUnlockedEquipmentIllus = new HashSet<string>();
		}
		if (AllUnlockedEquipmentIllus.Add(equipCode))
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsUnlockEquipmentIllu(string equipCode)
	{
		return AllUnlockedEquipmentIllus?.Contains(equipCode) ?? false;
	}

	public void TryUnlockMonsterIllu(List<string> monsterList)
	{
		if (AllUnlockedNormalMonsterIllus == null)
		{
			AllUnlockedNormalMonsterIllus = new HashSet<string>();
		}
		if (AllUnlockedBossMonsterIllus == null)
		{
			AllUnlockedBossMonsterIllus = new HashSet<string>();
		}
		bool flag = false;
		for (int i = 0; i < monsterList.Count; i++)
		{
			EnemyData enemyAttr = DataManager.Instance.GetEnemyAttr(monsterList[i]);
			if (enemyAttr.IsBoss)
			{
				if (AllUnlockedBossMonsterIllus.Add(enemyAttr.EnemyCode))
				{
					flag = true;
				}
			}
			else if (AllUnlockedNormalMonsterIllus.Add(enemyAttr.EnemyCode))
			{
				flag = true;
			}
		}
		if (flag)
		{
			GameSave.SaveUserData();
		}
	}

	public void TryUnlockMonsterIllu(string enemyCode)
	{
		if (AllUnlockedNormalMonsterIllus == null)
		{
			AllUnlockedNormalMonsterIllus = new HashSet<string>();
		}
		if (AllUnlockedBossMonsterIllus == null)
		{
			AllUnlockedBossMonsterIllus = new HashSet<string>();
		}
		bool flag = false;
		EnemyData enemyAttr = DataManager.Instance.GetEnemyAttr(enemyCode);
		if (enemyAttr.IsBoss)
		{
			if (AllUnlockedBossMonsterIllus.Add(enemyAttr.EnemyCode))
			{
				flag = true;
			}
		}
		else if (AllUnlockedNormalMonsterIllus.Add(enemyAttr.EnemyCode))
		{
			flag = true;
		}
		if (flag)
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsBossMonsterIlluUnlocked(string enemyCode)
	{
		return AllUnlockedBossMonsterIllus?.Contains(enemyCode) ?? false;
	}

	public bool IsNormalMonsterIlluUnlocked(string enemyCode)
	{
		return AllUnlockedNormalMonsterIllus?.Contains(enemyCode) ?? false;
	}

	public void PurchaseSpecialUsualCard(string cardCode, bool isAutoSave)
	{
		if (AllPurchasedSpecialUsualCards == null)
		{
			AllPurchasedSpecialUsualCards = new HashSet<string>();
		}
		AllPurchasedSpecialUsualCards.Add(cardCode);
		if (isAutoSave)
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsSpecialUsualCardPurchased(string cardCode)
	{
		return AllPurchasedSpecialUsualCards?.Contains(cardCode) ?? false;
	}

	public void PurchaseSkill(string skillCode, bool isAutoSave)
	{
		if (AllPurchasedSkills == null)
		{
			AllPurchasedSkills = new HashSet<string>();
		}
		AllPurchasedSkills.Add(skillCode);
		if (isAutoSave)
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsSkillPurchased(string skillCode)
	{
		return AllPurchasedSkills?.Contains(skillCode) ?? false;
	}

	public void PurchaseEquipment(string equipCode, bool isAutoSave)
	{
		if (AllPurchasedEquipments == null)
		{
			AllPurchasedEquipments = new HashSet<string>();
		}
		AllPurchasedEquipments.Add(equipCode);
		if (isAutoSave)
		{
			GameSave.SaveUserData();
		}
	}

	public bool IsEquipmentPurchased(string equipCode)
	{
		return AllPurchasedEquipments?.Contains(equipCode) ?? false;
	}
}
