using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
	public struct UsualUnlockData
	{
		public Dictionary<string, int> AllCardTimeSpaceNeed;

		public Dictionary<string, List<string>> AllCardToSkills;

		public Dictionary<string, List<string>> AllUnlockConditions;
	}

	private static DataManager _instance;

	private Dictionary<string, CompleteHelpData> allCompleteHelpDatas;

	private Dictionary<string, UsualCardAttr> allUsualCardDatas = new Dictionary<string, UsualCardAttr>();

	private Dictionary<string, SpecialUsualCardAttr> allSpecialUsualCardDatas = new Dictionary<string, SpecialUsualCardAttr>();

	private Dictionary<PlayerOccupation, Dictionary<string, SkillCardAttr>> allSkillCardDatas = new Dictionary<PlayerOccupation, Dictionary<string, SkillCardAttr>>();

	private Dictionary<EquipmentType, Dictionary<string, EquipmentCardAttr>> allEquipmentCardDatas = new Dictionary<EquipmentType, Dictionary<string, EquipmentCardAttr>>();

	private Dictionary<string, EnemyData> allEnemyDatas = new Dictionary<string, EnemyData>();

	private Dictionary<PlayerOccupation, OccupationInitSetting> allOccupationInitSettings = new Dictionary<PlayerOccupation, OccupationInitSetting>();

	private Dictionary<PlayerOccupation, UsualUnlockData> allUsualCardUnlockConfig;

	private Dictionary<PlayerOccupation, OccupationData> allOccupationDatas;

	private Dictionary<string, ProphesyCardData> allProphesyCardDatas = new Dictionary<string, ProphesyCardData>();

	private Dictionary<BuffType, BuffData> allBuffDatas;

	private Dictionary<string, GameEventData> allGameEventDatas = new Dictionary<string, GameEventData>();

	private Dictionary<EquipmentType, Dictionary<string, ItemPurchasedData>> allEquipmentPurchasedDatas;

	private Dictionary<PlayerOccupation, Dictionary<string, ItemPurchasedData>> allSkillPurchasedDatas;

	private Dictionary<string, ItemPurchasedData> allSpecialCardPurchasedDatas;

	private Dictionary<BaseGift.GiftName, GiftData> allGiftDatas;

	private Dictionary<string, BossHeapData> allBossHeapDatas;

	private Dictionary<string, PlotData> allPlotDatas = new Dictionary<string, PlotData>();

	private Dictionary<string, GuideTipData> allGuideTipDatas;

	public static DataManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new DataManager();
			}
			return _instance;
		}
	}

	public Dictionary<string, EnemyHeapData> AllEnemyHeapDatas { get; } = new Dictionary<string, EnemyHeapData>();


	public Dictionary<string, PlotData> AllPlotDatas => allPlotDatas;

	private DataManager()
	{
	}

	public void LoadData()
	{
		LoadUsualCardData();
		LoadSkillCardData();
		LoadEquipmentCardDatas();
		LoadEnemyAttrData();
		LoadEnemyHeapDatas();
		LoadOccupationInitSetting();
		LoadUsualCardUnlockConfig();
		LoadOccupationDatas();
		LoadProphesyCardData();
		LoadBuffData();
		LoadGameEventData();
		LoadPurchasedData();
		LoadGiftData();
		LoadBossHeapData();
		LoadPlotData();
		LoadGuideTipData();
		LoadHelpData();
	}

	public CompleteHelpData GetCompleteHelpData(string code)
	{
		return allCompleteHelpDatas[code];
	}

	private void LoadHelpData()
	{
		allCompleteHelpDatas = new Dictionary<string, CompleteHelpData>();
		AllCompleteHelpData allCompleteHelpData = JsonUtility.FromJson<AllCompleteHelpData>("HelpData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allCompleteHelpData.AllComplepteHelpDatas.Count; i++)
		{
			allCompleteHelpDatas.Add(allCompleteHelpData.AllComplepteHelpDatas[i].HelpCode, allCompleteHelpData.AllComplepteHelpDatas[i]);
		}
	}

	public UsualCardAttr GetUsualCardAttr(string cardCode)
	{
		if (allUsualCardDatas.TryGetValue(cardCode, out var value))
		{
			return value;
		}
		if (allSpecialUsualCardDatas.TryGetValue(cardCode, out var value2))
		{
			return value2;
		}
		Debug.LogError("The card you wanna get do not exist, check UsualCardData in Resources Config file...: " + cardCode);
		return null;
	}

	public SpecialUsualCardAttr GetSpecialUsualCardAttr(string cardCode)
	{
		if (!allSpecialUsualCardDatas.TryGetValue(cardCode, out var value))
		{
			return null;
		}
		return value;
	}

	public Dictionary<string, UsualCardAttr> GetAllUsualCardDatas()
	{
		return allUsualCardDatas;
	}

	public Dictionary<string, SpecialUsualCardAttr> GetAllSpecialUsualCardDatas()
	{
		return allSpecialUsualCardDatas;
	}

	private void LoadUsualCardData()
	{
		AllUsualCard allUsualCard = JsonUtility.FromJson<AllUsualCard>("UsualCardData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allUsualCard.AllUsualCardAttrs.Count; i++)
		{
			allUsualCardDatas[allUsualCard.AllUsualCardAttrs[i].CardCode] = allUsualCard.AllUsualCardAttrs[i];
		}
		LoadSpecialUsualCardData();
	}

	private void LoadSpecialUsualCardData()
	{
		AllSpecialUsualCard allSpecialUsualCard = JsonUtility.FromJson<AllSpecialUsualCard>("SpecialUsualCardData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allSpecialUsualCard.AllSpecialUsualCardAttrs.Count; i++)
		{
			allSpecialUsualCardDatas[allSpecialUsualCard.AllSpecialUsualCardAttrs[i].CardCode] = allSpecialUsualCard.AllSpecialUsualCardAttrs[i];
		}
	}

	public SkillCardAttr GetSkillCardAttr(PlayerOccupation playerOccupation, string cardCode)
	{
		SkillCardAttr value = null;
		if (!allSkillCardDatas[playerOccupation].TryGetValue(cardCode, out value))
		{
			Debug.LogError("The skill card attr you wanna load do not exist, check Resources SkillCardData file..." + cardCode);
		}
		return value;
	}

	public Dictionary<PlayerOccupation, Dictionary<string, SkillCardAttr>> GetAllSkillCardDatas()
	{
		return allSkillCardDatas;
	}

	public Dictionary<string, SkillCardAttr> GetAllPlayerOccupationSkillCardDatas(PlayerOccupation occupation)
	{
		return allSkillCardDatas[occupation];
	}

	private void LoadSkillCardData()
	{
		string[] names = Enum.GetNames(typeof(PlayerOccupation));
		for (int i = 0; i < names.Length; i++)
		{
			PlayerOccupation playerOccupation = (PlayerOccupation)Enum.Parse(typeof(PlayerOccupation), names[i]);
			if (playerOccupation != 0)
			{
				AllSkillData allSkillData = JsonUtility.FromJson<AllSkillData>($"{names[i]}SkillData.json".GetAllPlatformStreamingAssetsData());
				Dictionary<string, SkillCardAttr> dictionary = new Dictionary<string, SkillCardAttr>();
				for (int j = 0; j < allSkillData.AllSkillDatas.Count; j++)
				{
					dictionary.Add(allSkillData.AllSkillDatas[j].CardCode, allSkillData.AllSkillDatas[j]);
				}
				allSkillCardDatas.Add(playerOccupation, dictionary);
			}
		}
	}

	public EquipmentCardAttr GetEquipmentCardAttr(string cardCode)
	{
		EquipmentCardAttr value = null;
		foreach (KeyValuePair<EquipmentType, Dictionary<string, EquipmentCardAttr>> allEquipmentCardData in allEquipmentCardDatas)
		{
			if (allEquipmentCardData.Value.TryGetValue(cardCode, out value))
			{
				return value;
			}
		}
		Debug.LogError("The equipment card attr you wanna load do not exist, check Resources EquipmentCardData file...." + cardCode);
		return null;
	}

	public EquipmentCardAttr GetEquipmentCardAttr(string cardCode, EquipmentType type)
	{
		if (allEquipmentCardDatas[type].TryGetValue(cardCode, out var value))
		{
			return value;
		}
		Debug.LogError("The equipment card attr you wanna load do not exist, check Resources EquipmentCardData file...." + cardCode);
		return null;
	}

	public Dictionary<EquipmentType, Dictionary<string, EquipmentCardAttr>> GetAllEquipmentCardDatas()
	{
		return allEquipmentCardDatas;
	}

	public Dictionary<string, EquipmentCardAttr> GetSingleTypeEquipmentCardDatas(EquipmentType type)
	{
		return allEquipmentCardDatas[type];
	}

	private void LoadEquipmentCardDatas()
	{
		LoadTrinketAttrData();
		LoadHelmetAttrData();
		LoadArmorAttrData();
		LoadGloveAttrData();
		LoadShoesAttrData();
		LoadMaihandAttrData();
		LoadOffhandAttrData();
	}

	private void LoadTrinketAttrData()
	{
		AllTrinket allTrinket = JsonUtility.FromJson<AllTrinket>("Equipment_TrinketData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.Ornament, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allTrinket.AllTrinketAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.Ornament][allTrinket.AllTrinketAttrs[i].CardCode] = allTrinket.AllTrinketAttrs[i];
		}
	}

	private void LoadHelmetAttrData()
	{
		AllHead allHead = JsonUtility.FromJson<AllHead>("Equipment_HeadData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.Helmet, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allHead.AllHeadAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.Helmet][allHead.AllHeadAttrs[i].CardCode] = allHead.AllHeadAttrs[i];
		}
	}

	private void LoadArmorAttrData()
	{
		AllArmor allArmor = JsonUtility.FromJson<AllArmor>("Equipment_ArmorData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.Breastplate, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allArmor.AllArmorAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.Breastplate][allArmor.AllArmorAttrs[i].CardCode] = allArmor.AllArmorAttrs[i];
		}
	}

	private void LoadGloveAttrData()
	{
		AllHands allHands = JsonUtility.FromJson<AllHands>("Equipment_HandsData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.Glove, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allHands.AllHandsAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.Glove][allHands.AllHandsAttrs[i].CardCode] = allHands.AllHandsAttrs[i];
		}
	}

	private void LoadShoesAttrData()
	{
		AllShoes allShoes = JsonUtility.FromJson<AllShoes>("Equipment_ShoesData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.Shoes, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allShoes.AllShoesAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.Shoes][allShoes.AllShoesAttrs[i].CardCode] = allShoes.AllShoesAttrs[i];
		}
	}

	private void LoadMaihandAttrData()
	{
		AllMainHand allMainHand = JsonUtility.FromJson<AllMainHand>("Equipment_MainHandData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.MainHandWeapon, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allMainHand.AllMainHandAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.MainHandWeapon][allMainHand.AllMainHandAttrs[i].CardCode] = allMainHand.AllMainHandAttrs[i];
		}
	}

	private void LoadOffhandAttrData()
	{
		AllOffHand allOffHand = JsonUtility.FromJson<AllOffHand>("Equipment_OffHandData.json".GetAllPlatformStreamingAssetsData());
		allEquipmentCardDatas.Add(EquipmentType.SupHandWeapon, new Dictionary<string, EquipmentCardAttr>());
		for (int i = 0; i < allOffHand.AllOffHandAttrs.Count; i++)
		{
			allEquipmentCardDatas[EquipmentType.SupHandWeapon][allOffHand.AllOffHandAttrs[i].CardCode] = allOffHand.AllOffHandAttrs[i];
		}
	}

	public EnemyData GetEnemyAttr(string enemyCode)
	{
		EnemyData value = null;
		if (!allEnemyDatas.TryGetValue(enemyCode, out value))
		{
			Debug.LogError("The enemy you wanna load do not exist, check Resources EnemyData file...");
		}
		return value;
	}

	public Dictionary<string, EnemyData> GetAllEnemyAttrs()
	{
		return allEnemyDatas;
	}

	private void LoadEnemyAttrData()
	{
		AllEnemyData allEnemyData = JsonUtility.FromJson<AllEnemyData>("EnemyData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEnemyData.AllEnemyDatas.Count; i++)
		{
			allEnemyDatas[allEnemyData.AllEnemyDatas[i].EnemyCode] = allEnemyData.AllEnemyDatas[i];
		}
	}

	public EnemyHeapData GetSpecificEnemyHeap(string enemyHeapCode)
	{
		EnemyHeapData value = null;
		if (AllEnemyHeapDatas.TryGetValue(enemyHeapCode, out value))
		{
			return value;
		}
		Debug.LogError("Cannot load specific enemy heap data : " + enemyHeapCode);
		return null;
	}

	public void LoadEnemyHeapDatas()
	{
		AllEnemyHeapData allEnemyHeapData = JsonUtility.FromJson<AllEnemyHeapData>("EnemyHeap.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEnemyHeapData.AllEnemyHeapDatas.Count; i++)
		{
			AllEnemyHeapDatas[allEnemyHeapData.AllEnemyHeapDatas[i].EnemyHeapCode] = allEnemyHeapData.AllEnemyHeapDatas[i];
		}
	}

	public OccupationInitSetting GetPlayerOccupationInitSetting(PlayerOccupation playerOccupation)
	{
		if (!allOccupationInitSettings.ContainsKey(playerOccupation))
		{
			throw new KeyNotFoundException("Cannot find player occupation init setting, please add init setting at config file");
		}
		return allOccupationInitSettings[playerOccupation];
	}

	public Dictionary<PlayerOccupation, OccupationInitSetting> GetAllPlayerOccupationInitSettings()
	{
		return allOccupationInitSettings;
	}

	public void LoadOccupationInitSetting()
	{
		AllOccupationSetting allOccupationSetting = JsonUtility.FromJson<AllOccupationSetting>("OccupationInitSetting.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allOccupationSetting.OccupationSettings.Count; i++)
		{
			allOccupationInitSettings.Add((PlayerOccupation)Enum.Parse(typeof(PlayerOccupation), allOccupationSetting.OccupationSettings[i].PlayerOccupation), allOccupationSetting.OccupationSettings[i]);
		}
	}

	public Dictionary<PlayerOccupation, UsualUnlockData> GetAlUsualCardUnlockConfig()
	{
		return allUsualCardUnlockConfig;
	}

	public UsualUnlockData GetUusualCardUnlockConfig(PlayerOccupation occupation)
	{
		return allUsualCardUnlockConfig[occupation];
	}

	public void LoadUsualCardUnlockConfig()
	{
		allUsualCardUnlockConfig = new Dictionary<PlayerOccupation, UsualUnlockData>();
		AllUnlockCardConfig allUnlockCardConfig = JsonUtility.FromJson<AllUnlockCardConfig>("UsualCardUnlockConfig.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allUnlockCardConfig.AllConfigs.Count; i++)
		{
			PlayerOccupation key = (PlayerOccupation)Enum.Parse(typeof(PlayerOccupation), allUnlockCardConfig.AllConfigs[i].PlayerOccupation);
			UsualUnlockData usualUnlockData = default(UsualUnlockData);
			usualUnlockData.AllCardToSkills = new Dictionary<string, List<string>>();
			usualUnlockData.AllCardTimeSpaceNeed = new Dictionary<string, int>();
			usualUnlockData.AllUnlockConditions = new Dictionary<string, List<string>>();
			UsualUnlockData value = usualUnlockData;
			for (int j = 0; j < allUnlockCardConfig.AllConfigs[i].AllCards.Count; j++)
			{
				value.AllCardTimeSpaceNeed.Add(allUnlockCardConfig.AllConfigs[i].AllCards[j].CardCode, allUnlockCardConfig.AllConfigs[i].AllCards[j].SpaceTimeAmount);
			}
			for (int k = 0; k < allUnlockCardConfig.AllConfigs[i].AllCardUnlockInfos.Count; k++)
			{
				value.AllCardToSkills.Add(allUnlockCardConfig.AllConfigs[i].AllCardUnlockInfos[k].CardCode, allUnlockCardConfig.AllConfigs[i].AllCardUnlockInfos[k].Skills);
				value.AllUnlockConditions.Add(allUnlockCardConfig.AllConfigs[i].AllCardUnlockInfos[k].CardCode, allUnlockCardConfig.AllConfigs[i].AllCardUnlockInfos[k].UnlockConditions);
			}
			allUsualCardUnlockConfig[key] = value;
		}
	}

	public OccupationData GetOccupationData(PlayerOccupation occupation)
	{
		return allOccupationDatas[occupation];
	}

	private void LoadOccupationDatas()
	{
		this.allOccupationDatas = new Dictionary<PlayerOccupation, OccupationData>();
		AllOccupationDatas allOccupationDatas = JsonUtility.FromJson<AllOccupationDatas>("OccupationData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allOccupationDatas.AllOccupations.Count; i++)
		{
			PlayerOccupation key = (PlayerOccupation)Enum.Parse(typeof(PlayerOccupation), allOccupationDatas.AllOccupations[i].PlayerOccupation);
			this.allOccupationDatas.Add(key, allOccupationDatas.AllOccupations[i]);
		}
	}

	public Dictionary<string, ProphesyCardData> GetAllProphesyCardDatas()
	{
		return allProphesyCardDatas;
	}

	public ProphesyCardData GetProphesyCardDataByCardData(string cardCode)
	{
		return allProphesyCardDatas[cardCode];
	}

	private void LoadProphesyCardData()
	{
		AllProphesyCardData allProphesyCardData = JsonUtility.FromJson<AllProphesyCardData>("ProphesyCardData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allProphesyCardData.AllProphesyCardDatas.Count; i++)
		{
			allProphesyCardDatas[allProphesyCardData.AllProphesyCardDatas[i].CardCode] = allProphesyCardData.AllProphesyCardDatas[i];
		}
	}

	public BuffData GetBuffDataByBuffType(BuffType buffType)
	{
		return allBuffDatas[buffType];
	}

	private void LoadBuffData()
	{
		allBuffDatas = new Dictionary<BuffType, BuffData>();
		AllBuffData allBuffData = JsonUtility.FromJson<AllBuffData>("BuffData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allBuffData.AllBuffDatas.Count; i++)
		{
			BuffType key = (BuffType)Enum.Parse(typeof(BuffType), allBuffData.AllBuffDatas[i].BuffType);
			allBuffDatas.Add(key, allBuffData.AllBuffDatas[i]);
		}
	}

	public GameEventData GetGameEventData(string eventCode)
	{
		if (allGameEventDatas.TryGetValue(eventCode, out var value))
		{
			return value;
		}
		throw new KeyNotFoundException("game event data not exist, event code : " + eventCode);
	}

	private void LoadGameEventData()
	{
		AllGameEventData allGameEventData = JsonUtility.FromJson<AllGameEventData>("GameEventData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allGameEventData.AllGameEventDatas.Count; i++)
		{
			allGameEventDatas[allGameEventData.AllGameEventDatas[i].EventCode] = allGameEventData.AllGameEventDatas[i];
		}
	}

	private void LoadPurchasedData()
	{
		LoadEquipmentPurchasedData();
		LoadSkillPurchasedData();
		LoadCardPurchasedData();
	}

	public ItemPurchasedData GetEquipmentPutchasedData(EquipmentType equipmentType, string equipCode)
	{
		return allEquipmentPurchasedDatas[equipmentType][equipCode];
	}

	public Dictionary<string, ItemPurchasedData> GetAllEquipmentPurchasedDatasByType(EquipmentType equipmentType)
	{
		return allEquipmentPurchasedDatas[equipmentType];
	}

	private void LoadEquipmentPurchasedData()
	{
		allEquipmentPurchasedDatas = new Dictionary<EquipmentType, Dictionary<string, ItemPurchasedData>>();
		LoadBreasplatePurchasedDatas();
		LoadHelmetPurchasedDatas();
		LoadGlovePurchasedDatas();
		LoadMainhandPurchasedDatas();
		LoadSuphandPurchasedDatas();
		LoadShoesPurchasedDatas();
		LoadOrnamentPurchasedDatas();
	}

	private void LoadBreasplatePurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_Armor.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.Breastplate] = dictionary;
	}

	private void LoadHelmetPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_Head.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.Helmet] = dictionary;
	}

	private void LoadGlovePurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_Hands.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.Glove] = dictionary;
	}

	private void LoadMainhandPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_MainHand.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.MainHandWeapon] = dictionary;
	}

	private void LoadSuphandPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_OffHand.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.SupHandWeapon] = dictionary;
	}

	private void LoadShoesPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_Shoes.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.Shoes] = dictionary;
	}

	private void LoadOrnamentPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllEquipmentPurchasedData allEquipmentPurchasedData = JsonUtility.FromJson<AllEquipmentPurchasedData>("EquipPurchasedData_Trinket.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allEquipmentPurchasedData.AllEquipPurchasedDatas.Count; i++)
		{
			dictionary[allEquipmentPurchasedData.AllEquipPurchasedDatas[i].ItemCode] = allEquipmentPurchasedData.AllEquipPurchasedDatas[i];
		}
		allEquipmentPurchasedDatas[EquipmentType.Ornament] = dictionary;
	}

	public ItemPurchasedData GetSkillPurchasedData(PlayerOccupation playerOccupation, string skillCode)
	{
		return allSkillPurchasedDatas[playerOccupation][skillCode];
	}

	public Dictionary<string, ItemPurchasedData> GetAllPurchasedDatasByOccupation(PlayerOccupation playerOccupation)
	{
		return allSkillPurchasedDatas[playerOccupation];
	}

	private void LoadSkillPurchasedData()
	{
		allSkillPurchasedDatas = new Dictionary<PlayerOccupation, Dictionary<string, ItemPurchasedData>>();
		LoadArcherPurchasedDatas();
		LoadKnightPurchasedDatas();
	}

	private void LoadArcherPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllSkillPurchasedData allSkillPurchasedData = JsonUtility.FromJson<AllSkillPurchasedData>("ArcherSkillPurchasedData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allSkillPurchasedData.AllSkillPurchasedDatas.Count; i++)
		{
			dictionary[allSkillPurchasedData.AllSkillPurchasedDatas[i].ItemCode] = allSkillPurchasedData.AllSkillPurchasedDatas[i];
		}
		allSkillPurchasedDatas[PlayerOccupation.Archer] = dictionary;
	}

	private void LoadKnightPurchasedDatas()
	{
		Dictionary<string, ItemPurchasedData> dictionary = new Dictionary<string, ItemPurchasedData>();
		AllSkillPurchasedData allSkillPurchasedData = JsonUtility.FromJson<AllSkillPurchasedData>("KnightSkillPurchasedData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allSkillPurchasedData.AllSkillPurchasedDatas.Count; i++)
		{
			dictionary[allSkillPurchasedData.AllSkillPurchasedDatas[i].ItemCode] = allSkillPurchasedData.AllSkillPurchasedDatas[i];
		}
		allSkillPurchasedDatas[PlayerOccupation.Knight] = dictionary;
	}

	public ItemPurchasedData GetSpecialCardPurchasedData(string cardCode)
	{
		return allSpecialCardPurchasedDatas[cardCode];
	}

	public Dictionary<string, ItemPurchasedData> GetAllSpecialCardPurchasedDatas()
	{
		return allSpecialCardPurchasedDatas;
	}

	private void LoadCardPurchasedData()
	{
		allSpecialCardPurchasedDatas = new Dictionary<string, ItemPurchasedData>();
		AllSpecialCardPurchasedData allSpecialCardPurchasedData = JsonUtility.FromJson<AllSpecialCardPurchasedData>("SpecialUsualPurchasedData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allSpecialCardPurchasedData.AllSpecialUsualCardPurchasedDatas.Count; i++)
		{
			allSpecialCardPurchasedDatas[allSpecialCardPurchasedData.AllSpecialUsualCardPurchasedDatas[i].ItemCode] = allSpecialCardPurchasedData.AllSpecialUsualCardPurchasedDatas[i];
		}
	}

	public GiftData GetGiftDataByGiftName(BaseGift.GiftName giftName)
	{
		return allGiftDatas[giftName];
	}

	private void LoadGiftData()
	{
		allGiftDatas = new Dictionary<BaseGift.GiftName, GiftData>();
		AllGiftData allGiftData = JsonUtility.FromJson<AllGiftData>("GiftData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allGiftData.AllGiftDatas.Count; i++)
		{
			allGiftDatas[allGiftData.AllGiftDatas[i].GiftName] = allGiftData.AllGiftDatas[i];
		}
	}

	public BossHeapData GetBossHeapData(string heapCode)
	{
		return allBossHeapDatas[heapCode];
	}

	private void LoadBossHeapData()
	{
		allBossHeapDatas = new Dictionary<string, BossHeapData>();
		AllBossHeapData allBossHeapData = JsonUtility.FromJson<AllBossHeapData>("BossHeapData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allBossHeapData.AllBossHeapDatas.Count; i++)
		{
			allBossHeapDatas[allBossHeapData.AllBossHeapDatas[i].HeapCode] = allBossHeapData.AllBossHeapDatas[i];
		}
	}

	public PlotData GetPlotData(string plotCode)
	{
		return allPlotDatas[plotCode];
	}

	private void LoadPlotData()
	{
		allPlotDatas = new Dictionary<string, PlotData>();
		AllPlotData allPlotData = JsonUtility.FromJson<AllPlotData>("PlotData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allPlotData.AllPlotDatas.Count; i++)
		{
			allPlotDatas[allPlotData.AllPlotDatas[i].PlotCode] = allPlotData.AllPlotDatas[i];
		}
	}

	public GuideTipData GetGuideTipData(string code)
	{
		return allGuideTipDatas[code];
	}

	private void LoadGuideTipData()
	{
		this.allGuideTipDatas = new Dictionary<string, GuideTipData>();
		AllGuideTipDatas allGuideTipDatas = JsonUtility.FromJson<AllGuideTipDatas>("GuideTipData.json".GetAllPlatformStreamingAssetsData());
		for (int i = 0; i < allGuideTipDatas.allGuideTipDatas.Count; i++)
		{
			this.allGuideTipDatas.Add(allGuideTipDatas.allGuideTipDatas[i].TipCode, allGuideTipDatas.allGuideTipDatas[i]);
		}
	}
}
