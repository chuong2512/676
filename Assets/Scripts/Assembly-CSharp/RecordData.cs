using System;
using System.Collections.Generic;

[Serializable]
public class RecordData
{
	public Dictionary<string, int> MainHandCards = new Dictionary<string, int>();

	public Dictionary<string, int> SupHandCards = new Dictionary<string, int>();

	public List<string> AllEquipedSkills = new List<string>();

	public List<string> allClearBossHeapIDList = new List<string>();

	public string MainHandWeapon;

	public string SupHandWeapon;

	public string Ornament;

	public string Helmet;

	public string Breasplate;

	public string Glove;

	public string Shoes;

	public bool isGamePass;

	public int timeUsed;

	public int mapLevel;

	public int mapLayer;

	public int MemoryAmount;

	public int DrawCardAmount;

	public int ActionPointAmount;

	public int ArmorAmount;

	public int AtkDmg;

	public int DefenceAttr;

	public int Year;

	public int Month;

	public int Day;

	public int Hour;

	public int Minute;

	public PlayerOccupation PlayerOccupation;

	public RecordData(PlayerOccupation playerOccupation)
	{
		PlayerOccupation = playerOccupation;
		DateTime now = DateTime.Now;
		Year = now.Year;
		Month = now.Month;
		Day = now.Day;
		Hour = now.Hour;
		Minute = now.Minute;
	}

	public void AddMainHandCard(string card, int amount)
	{
		MainHandCards.Add(card, amount);
	}

	public void AddSupHandCard(string card, int amount)
	{
		SupHandCards.Add(card, amount);
	}

	public void AddSkill(string skill)
	{
		AllEquipedSkills.Add(skill);
	}
}
