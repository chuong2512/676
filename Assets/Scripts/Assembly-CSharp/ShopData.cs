using System;
using System.Collections.Generic;

[Serializable]
public class ShopData
{
	public int currentLayer;

	public int currentLevel;

	public bool isEquipShop;

	public List<string> allItemCodes;

	public string saledItemCode;
}
