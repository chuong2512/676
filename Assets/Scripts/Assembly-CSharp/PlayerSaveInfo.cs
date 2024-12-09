using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSaveInfo
{
	public PlayerOccupation PlayerOccupation;

	public PlayerAttrSaveInfo PlayerAttrSaveInfo;

	public PlayerBattleSaveInfo PlayerBattleSaveInfo;

	public PlayerEquipmentSaveInfo PlayerEquipmentSaveInfo;

	public PlayerInventorySaveInfo PlayerInventorySaveInfo;

	public List<string> AllProphesyCodes;

	public int allTimePiecesAmount;

	public PlayerSaveInfo(PlayerOccupation playerOccupation, PlayerAttrSaveInfo attrSaveInfo, PlayerBattleSaveInfo battleSaveInfo, PlayerEquipmentSaveInfo equipmentSaveInfo, PlayerInventorySaveInfo inventorySaveInfo, List<string> prophesyCodes, int timePiecesAmount)
	{
		PlayerOccupation = playerOccupation;
		PlayerAttrSaveInfo = attrSaveInfo;
		PlayerBattleSaveInfo = battleSaveInfo;
		PlayerEquipmentSaveInfo = equipmentSaveInfo;
		PlayerInventorySaveInfo = inventorySaveInfo;
		AllProphesyCodes = prophesyCodes;
		allTimePiecesAmount = timePiecesAmount;
	}
}
