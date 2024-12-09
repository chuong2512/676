using System;

[Serializable]
public class PlayerEquipmentSaveInfo
{
	public string equipedHelmet;

	public string equipedOrnament;

	public string equipedGlove;

	public string equipedShoes;

	public string equipedBreastplate;

	public string equipedMainHand;

	public string equipedSupHand;

	public PlayerEquipmentSaveInfo(Player player)
	{
		equipedHelmet = player.PlayerEquipment.Helmet.CardCode;
		equipedOrnament = player.PlayerEquipment.Ornament.CardCode;
		equipedGlove = player.PlayerEquipment.Glove.CardCode;
		equipedShoes = player.PlayerEquipment.Shoes.CardCode;
		equipedBreastplate = player.PlayerEquipment.Breastplate.CardCode;
		equipedMainHand = player.PlayerEquipment.MainHandWeapon.CardCode;
		equipedSupHand = player.PlayerEquipment.SupHandWeapon.CardCode;
	}
}
