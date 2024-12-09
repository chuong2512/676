using System;
using System.Collections.Generic;

[Serializable]
public struct CardToUnlockInfo
{
	public string CardCode;

	public List<string> UnlockConditions;

	public List<string> Skills;
}
