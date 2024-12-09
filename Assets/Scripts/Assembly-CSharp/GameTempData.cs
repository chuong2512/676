public static class GameTempData
{
	public static ShopData ShopData;

	public static void ClearGameTempData()
	{
		ShopData = null;
	}

	public static void TryRemoveEquips(string itemCode)
	{
		if (ShopData != null && ShopData.isEquipShop)
		{
			ShopData.allItemCodes.Remove(itemCode);
		}
	}
}
