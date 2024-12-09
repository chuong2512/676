using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInventorySaveInfo
{
	public Dictionary<string, int> allInventoryMainHandCards;

	public Dictionary<string, int> allInventorySupHandCards;

	public Dictionary<string, int> allInventoryOccupationCards;

	public List<string> AllHelmets;

	public List<string> AllBreasplates;

	public List<string> AllGloves;

	public List<string> AllOrnaments;

	public List<string> AllShoes;

	public List<string> AllMainHands;

	public List<string> AllSupHands;

	public List<string> allInventorySkills;

	public HashSet<string> allNewCards;

	public HashSet<string> allNewEquips;

	public HashSet<string> allNewSkills;

	public int money;

	public PlayerInventorySaveInfo(Player player)
	{
		allInventoryMainHandCards = player.PlayerInventory.AllMainHandCards;
		allInventorySupHandCards = player.PlayerInventory.AllSupHandCards;
		allInventoryOccupationCards = player.PlayerInventory.AllSpecialUsualCards;
		AllHelmets = player.PlayerInventory.AllHelmets;
		AllGloves = player.PlayerInventory.AllGloves;
		AllBreasplates = player.PlayerInventory.AllBreasplates;
		AllShoes = player.PlayerInventory.AllShoes;
		AllOrnaments = player.PlayerInventory.AllOrnaments;
		AllMainHands = player.PlayerInventory.AllMainHands;
		AllSupHands = player.PlayerInventory.AllSupHands;
		allInventorySkills = player.PlayerInventory.AllSkills;
		allNewCards = player.PlayerInventory.AllNewCards;
		allNewEquips = player.PlayerInventory.AllNewEquipments;
		allNewSkills = player.PlayerInventory.AllNewSkills;
		money = player.PlayerInventory.PlayerMoney;
	}
}
