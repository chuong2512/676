using System.Collections.Generic;

public class PlayerInventory
{
	private class EquipInventoryHandler
	{
		protected List<string> allEquipments;

		protected PlayerInventory _playerInventory;

		public List<string> AllEquipments
		{
			get
			{
				if (allEquipments == null)
				{
					allEquipments = new List<string>();
				}
				return allEquipments;
			}
		}

		public EquipInventoryHandler(PlayerInventory playerInventory)
		{
			_playerInventory = playerInventory;
			allEquipments = new List<string>();
		}

		public void InitInventory(List<string> allEquips)
		{
			allEquipments = allEquips;
		}

		public void AddEquipment(string equipCode)
		{
			if (!AllEquipments.Contains(equipCode))
			{
				AllEquipments.Add(equipCode);
			}
		}

		public void ClearAllEquipments()
		{
			allEquipments.Clear();
		}

		public bool IsHaveEquip(string equipCode)
		{
			return AllEquipments.Contains(equipCode);
		}

		public void EquipEquipment(string preEquip, string currentEquip)
		{
			AllEquipments.Add(preEquip);
			AllEquipments.Remove(currentEquip);
		}

		public void RemoveEquipment(string equipCode)
		{
			AllEquipments.Remove(equipCode);
		}
	}

	private Player player;

	private Dictionary<EquipmentType, EquipInventoryHandler> allEquipmentTypeToHandlers;

	public int PlayerMoney { get; set; }

	public Dictionary<string, int> AllSpecialUsualCards { get; set; }

	public Dictionary<string, int> AllMainHandCards { get; set; }

	public Dictionary<string, int> AllSupHandCards { get; set; }

	public HashSet<string> AllNewEquipments { get; private set; }

	public List<string> AllSkills { get; set; }

	public HashSet<string> AllNewSkills { get; private set; }

	public HashSet<string> AllNewCards { get; private set; }

	public int AllEquipmentCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<EquipmentType, EquipInventoryHandler> allEquipmentTypeToHandler in allEquipmentTypeToHandlers)
			{
				num += allEquipmentTypeToHandler.Value.AllEquipments.Count;
			}
			return num;
		}
	}

	public List<string> AllShoes => allEquipmentTypeToHandlers[EquipmentType.Shoes].AllEquipments;

	public List<string> AllHelmets => allEquipmentTypeToHandlers[EquipmentType.Helmet].AllEquipments;

	public List<string> AllBreasplates => allEquipmentTypeToHandlers[EquipmentType.Breastplate].AllEquipments;

	public List<string> AllGloves => allEquipmentTypeToHandlers[EquipmentType.Glove].AllEquipments;

	public List<string> AllOrnaments => allEquipmentTypeToHandlers[EquipmentType.Ornament].AllEquipments;

	public List<string> AllMainHands => allEquipmentTypeToHandlers[EquipmentType.MainHandWeapon].AllEquipments;

	public List<string> AllSupHands => allEquipmentTypeToHandlers[EquipmentType.SupHandWeapon].AllEquipments;

	public PlayerInventory(Player player)
	{
		this.player = player;
		AllSpecialUsualCards = new Dictionary<string, int>();
		AllMainHandCards = new Dictionary<string, int>();
		AllSupHandCards = new Dictionary<string, int>();
		AllNewEquipments = new HashSet<string>();
		AllSkills = new List<string>();
		AllNewSkills = new HashSet<string>();
		AllNewCards = new HashSet<string>();
		allEquipmentTypeToHandlers = new Dictionary<EquipmentType, EquipInventoryHandler>(7)
		{
			{
				EquipmentType.Helmet,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.Shoes,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.Breastplate,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.Glove,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.Ornament,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.MainHandWeapon,
				new EquipInventoryHandler(this)
			},
			{
				EquipmentType.SupHandWeapon,
				new EquipInventoryHandler(this)
			}
		};
	}

	public void SetPlayerInitInventory(OccupationInitSetting initSetting, Dictionary<string, int> mainhandCards, Dictionary<string, int> suphandCards)
	{
		PlayerMoney = initSetting.InitMoney;
		AllMainHandCards = new Dictionary<string, int>(mainhandCards);
		AllSupHandCards = new Dictionary<string, int>(suphandCards);
	}

	public void LoadPlayerInventorySaveInfo(PlayerInventorySaveInfo saveInfo)
	{
		AllMainHandCards = saveInfo.allInventoryMainHandCards;
		AllSupHandCards = saveInfo.allInventorySupHandCards;
		AllSpecialUsualCards = saveInfo.allInventoryOccupationCards;
		AllSkills = saveInfo.allInventorySkills;
		PlayerMoney = saveInfo.money;
		AllNewCards = saveInfo.allNewCards;
		AllNewEquipments = saveInfo.allNewEquips;
		AllNewSkills = saveInfo.allNewSkills;
		LoadPlayerEquipmentInventorySaveInfo(saveInfo);
	}

	private void LoadPlayerEquipmentInventorySaveInfo(PlayerInventorySaveInfo saveInfo)
	{
		allEquipmentTypeToHandlers[EquipmentType.Helmet].InitInventory(saveInfo.AllHelmets);
		allEquipmentTypeToHandlers[EquipmentType.Breastplate].InitInventory(saveInfo.AllBreasplates);
		allEquipmentTypeToHandlers[EquipmentType.Glove].InitInventory(saveInfo.AllGloves);
		allEquipmentTypeToHandlers[EquipmentType.Shoes].InitInventory(saveInfo.AllShoes);
		allEquipmentTypeToHandlers[EquipmentType.Ornament].InitInventory(saveInfo.AllOrnaments);
		allEquipmentTypeToHandlers[EquipmentType.MainHandWeapon].InitInventory(saveInfo.AllMainHands);
		allEquipmentTypeToHandlers[EquipmentType.SupHandWeapon].InitInventory(saveInfo.AllSupHands);
	}

	public void EquipEquipment(EquipmentType type, string preEquip, string currentEquip)
	{
		allEquipmentTypeToHandlers[type].EquipEquipment(preEquip, currentEquip);
	}

	public void AddEquipment(string equipCode)
	{
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		allEquipmentTypeToHandlers[equipmentCardAttr.EquipmentType].AddEquipment(equipCode);
		EventManager.BroadcastEvent(EventEnum.E_PlayerAddEquipment, new SimpleEventData
		{
			stringValue = equipCode
		});
		AddNewEquipment(equipCode, equipmentCardAttr.EquipmentType);
		GameTempData.TryRemoveEquips(equipCode);
	}

	private void AddNewEquipment(string equipCode, EquipmentType equipmentType)
	{
		AllNewEquipments.Add(equipCode);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockEquipmentIllu(equipCode);
		AllRandomInventory.Instance.RemoveEquipment(equipCode, equipmentType);
	}

	public void ClearAllEquipments()
	{
		foreach (KeyValuePair<EquipmentType, EquipInventoryHandler> allEquipmentTypeToHandler in allEquipmentTypeToHandlers)
		{
			allEquipmentTypeToHandler.Value.ClearAllEquipments();
		}
		AllNewEquipments.Clear();
	}

	public bool RemoveNewEquipment(string equipCode)
	{
		return AllNewEquipments.Remove(equipCode);
	}

	public void RemoveEquipment(string equipCode)
	{
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		allEquipmentTypeToHandlers[equipmentCardAttr.EquipmentType].RemoveEquipment(equipCode);
		RemoveNewEquipment(equipCode);
		AllRandomInventory.Instance.AddEquipment(equipCode, equipmentCardAttr.EquipmentType);
	}

	public bool IsHaveEquipment(string equipCode)
	{
		foreach (KeyValuePair<EquipmentType, EquipInventoryHandler> allEquipmentTypeToHandler in allEquipmentTypeToHandlers)
		{
			if (allEquipmentTypeToHandler.Value.IsHaveEquip(equipCode))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsHaveEquipment(string equipCode, EquipmentType type)
	{
		return allEquipmentTypeToHandlers[type].IsHaveEquip(equipCode);
	}

	public void PlayerEarnMoney(int money)
	{
		PlayerMoney += money;
		EventManager.BroadcastEvent(EventEnum.E_PlayerCoinUpdate, null);
	}

	public bool PlayerComsumeMoney(int money)
	{
		if (PlayerMoney >= money)
		{
			PlayerMoney -= money;
			EventManager.BroadcastEvent(EventEnum.E_PlayerCoinUpdate, null);
			return true;
		}
		return false;
	}

	public void AddSpecialUsualCards(string cardCode, int amount, bool isNew)
	{
		if (AllSpecialUsualCards.TryGetValue(cardCode, out var value))
		{
			AllSpecialUsualCards[cardCode] = value + amount;
		}
		else
		{
			AllSpecialUsualCards[cardCode] = amount;
		}
		if (isNew)
		{
			AllNewCards.Add(cardCode);
			SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockSpecialUsualCardIllus(cardCode);
			if (DataManager.Instance.GetSpecialUsualCardAttr(cardCode).IsOnlyOne)
			{
				AllRandomInventory.Instance.RemoveSpecialUsualCard(cardCode);
			}
		}
	}

	public bool RemoveNewCard(string cardCode)
	{
		AllNewCards.Remove(cardCode);
		return AllNewCards.Count > 0;
	}

	public bool IsPlayerInventoryContainNewCard(string cardCode)
	{
		return AllNewCards.Contains(cardCode);
	}

	public bool ReduceSpecialUsualCards(string cardCode, int amount)
	{
		if (AllSpecialUsualCards.TryGetValue(cardCode, out var value))
		{
			if (value < amount)
			{
				return false;
			}
			AllSpecialUsualCards[cardCode] = value - amount;
			return true;
		}
		return false;
	}

	public void AddMainHandCards(string cardCode, int amount)
	{
		if (AllMainHandCards.TryGetValue(cardCode, out var value))
		{
			AllMainHandCards[cardCode] = value + amount;
		}
		else
		{
			AllMainHandCards[cardCode] = amount;
		}
	}

	public bool ReduceMainHandCards(string cardCode, int amount)
	{
		if (AllMainHandCards.TryGetValue(cardCode, out var value))
		{
			if (value < amount)
			{
				return false;
			}
			AllMainHandCards[cardCode] = value - amount;
			return true;
		}
		return false;
	}

	public void AddSupHandCards(string cardCode, int amount)
	{
		if (AllSupHandCards.TryGetValue(cardCode, out var value))
		{
			AllSupHandCards[cardCode] = value + amount;
		}
		else
		{
			AllSupHandCards[cardCode] = amount;
		}
	}

	public bool ReduceSupHandCards(string cardCode, int amount)
	{
		if (AllSupHandCards.TryGetValue(cardCode, out var value))
		{
			if (value < amount)
			{
				return false;
			}
			AllSupHandCards[cardCode] = value - amount;
			return true;
		}
		return false;
	}

	public void AddSkill(string skillCode, bool isNew)
	{
		if (!AllSkills.Contains(skillCode))
		{
			AllSkills.Add(skillCode);
			EventManager.BroadcastEvent(EventEnum.E_PlayerAddSkill, new SimpleEventData
			{
				stringValue = skillCode,
				boolValue = isNew
			});
			if (isNew)
			{
				AddNewSkill(skillCode);
			}
		}
	}

	public void ReleaseSkill(string skillCode)
	{
		AllSkills.Add(skillCode);
	}

	public void EquipSkill(string skillCode)
	{
		AllSkills.Remove(skillCode);
	}

	public void RemoveSkill(string skillCode)
	{
		if (AllSkills.Contains(skillCode))
		{
			AllSkills.Remove(skillCode);
			RemoveNewSkill(skillCode);
			AllRandomInventory.Instance.AddPlayerSkill(skillCode);
		}
	}

	private void AddNewSkill(string skillCode)
	{
		AllNewSkills.Add(skillCode);
		AllRandomInventory.Instance.RemovePlayerSkill(skillCode);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.TryUnlockSkillIllu(skillCode);
	}

	public bool RemoveNewSkill(string skillCode)
	{
		AllNewSkills.Remove(skillCode);
		return AllNewSkills.Count > 0;
	}

	public bool IsHaveSkill(string skillCode)
	{
		return AllSkills.Contains(skillCode);
	}
}
