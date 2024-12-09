using System;
using System.Collections.Generic;

[Serializable]
public class PlayerBattleSaveInfo
{
	public Dictionary<string, int> allEquipedMainHandCards;

	public Dictionary<string, int> allEquipedSupHandCards;

	public List<string> allEquipedSkills;

	public List<int> allGift;

	public PlayerBattleSaveInfo(Player player)
	{
		allEquipedMainHandCards = player.PlayerBattleInfo.AllEquipedMainHandCards;
		allEquipedSupHandCards = player.PlayerBattleInfo.AllEquipedSupHandCards;
		allEquipedSkills = player.PlayerBattleInfo.CurrentSkillList;
		allGift = player.PlayerBattleInfo.GetGiftList();
	}
}
