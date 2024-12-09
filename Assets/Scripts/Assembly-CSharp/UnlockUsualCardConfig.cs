using System;
using System.Collections.Generic;

[Serializable]
public struct UnlockUsualCardConfig
{
	public string PlayerOccupation;

	public List<CardSpaceTimeAmountStruct> AllCards;

	public List<CardToUnlockInfo> AllCardUnlockInfos;
}
