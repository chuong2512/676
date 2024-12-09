public class PlayerEquipment
{
	public EquipmentCard Helmet;

	public EquipmentCard Glove;

	public EquipmentCard Breastplate;

	public EquipmentCard Shoes;

	public EquipmentCard MainHandWeapon;

	public EquipmentCard SupHandWeapon;

	public EquipmentCard Ornament;

	private Player player;

	private SuitHandler suitHandler;

	public SuitHandler SuitHandler => suitHandler;

	public void LoadPlayerEquipmentSaveInfo(PlayerEquipmentSaveInfo saveInfo)
	{
		EquipHelmet(FactoryManager.GetEquipmentCard(saveInfo.equipedHelmet));
		EquipBreastplate(FactoryManager.GetEquipmentCard(saveInfo.equipedBreastplate));
		EquipGlove(FactoryManager.GetEquipmentCard(saveInfo.equipedGlove));
		EquipOrnament(FactoryManager.GetEquipmentCard(saveInfo.equipedOrnament));
		EquipShoes(FactoryManager.GetEquipmentCard(saveInfo.equipedShoes));
		EquipMainHandWeapon(FactoryManager.GetEquipmentCard(saveInfo.equipedMainHand));
		EquipSupHandWeapon(FactoryManager.GetEquipmentCard(saveInfo.equipedSupHand));
	}

	public void SetPlayerInitEquipment(OccupationInitSetting initSetting)
	{
		EquipHelmet(FactoryManager.GetEquipmentCard(initSetting.InitHelmet));
		EquipGlove(FactoryManager.GetEquipmentCard(initSetting.InitGlove));
		EquipBreastplate(FactoryManager.GetEquipmentCard(initSetting.InitBreastplate));
		EquipShoes(FactoryManager.GetEquipmentCard(initSetting.InitShoes));
		EquipMainHandWeapon(FactoryManager.GetEquipmentCard(initSetting.InitMainhand));
		EquipSupHandWeapon(FactoryManager.GetEquipmentCard(initSetting.InitSuphand));
		EquipOrnament(FactoryManager.GetEquipmentCard(initSetting.InitTrinket));
	}

	public PlayerEquipment(Player player)
	{
		this.player = player;
		suitHandler = new SuitHandler(player, this);
	}

	public bool IsPlayerEquiped(string equipCode)
	{
		if (!(Helmet.CardCode == equipCode) && !(Glove.CardCode == equipCode) && !(Breastplate.CardCode == equipCode) && !(Shoes.CardCode == equipCode) && !(MainHandWeapon.CardCode == equipCode) && !(SupHandWeapon.CardCode == equipCode))
		{
			return Ornament.CardCode == equipCode;
		}
		return true;
	}

	private void AddSuit(SuitType suitType, string equipCode)
	{
		suitHandler.AddSuit(suitType, equipCode);
	}

	private void RemoveSuit(SuitType suitType, string equipCode)
	{
		suitHandler.RemoveSuit(suitType, equipCode);
	}

	public void EquipHelmet(EquipmentCard card)
	{
		if (Helmet != null)
		{
			Helmet.Release(player);
			if (Helmet.SuitType != 0)
			{
				RemoveSuit(Helmet.SuitType, Helmet.CardCode);
			}
		}
		card.Equip(player);
		Helmet = card;
		if (Helmet.SuitType != 0)
		{
			AddSuit(Helmet.SuitType, Helmet.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipHelmet(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipGlove(EquipmentCard card)
	{
		if (Glove != null)
		{
			Glove.Release(player);
			if (Glove.SuitType != 0)
			{
				RemoveSuit(Glove.SuitType, Glove.CardCode);
			}
		}
		card.Equip(player);
		Glove = card;
		if (Glove.SuitType != 0)
		{
			AddSuit(Glove.SuitType, Glove.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipGlove(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipBreastplate(EquipmentCard card)
	{
		if (Breastplate != null)
		{
			Breastplate.Release(player);
			if (Breastplate.SuitType != 0)
			{
				RemoveSuit(Breastplate.SuitType, Breastplate.CardCode);
			}
		}
		card.Equip(player);
		Breastplate = card;
		if (Breastplate.SuitType != 0)
		{
			AddSuit(Breastplate.SuitType, Breastplate.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipBreastplate(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipShoes(EquipmentCard card)
	{
		if (Shoes != null)
		{
			Shoes.Release(player);
			if (Shoes.SuitType != 0)
			{
				RemoveSuit(Shoes.SuitType, Shoes.CardCode);
			}
		}
		card.Equip(player);
		Shoes = card;
		if (Shoes.SuitType != 0)
		{
			AddSuit(Shoes.SuitType, Shoes.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipShoes(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipMainHandWeapon(EquipmentCard card)
	{
		if (MainHandWeapon != null)
		{
			MainHandWeapon.Release(player);
			if (MainHandWeapon.SuitType != 0)
			{
				RemoveSuit(MainHandWeapon.SuitType, MainHandWeapon.CardCode);
			}
		}
		card.Equip(player);
		MainHandWeapon = card;
		if (MainHandWeapon.SuitType != 0)
		{
			AddSuit(MainHandWeapon.SuitType, MainHandWeapon.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipMainHand(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipSupHandWeapon(EquipmentCard card)
	{
		if (SupHandWeapon != null)
		{
			SupHandWeapon.Release(player);
			if (SupHandWeapon.SuitType != 0)
			{
				RemoveSuit(SupHandWeapon.SuitType, SupHandWeapon.CardCode);
			}
		}
		card.Equip(player);
		SupHandWeapon = card;
		if (SupHandWeapon.SuitType != 0)
		{
			AddSuit(SupHandWeapon.SuitType, SupHandWeapon.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipSupHand(card.CardCode, isNeedHint: true);
		}
	}

	public void EquipOrnament(EquipmentCard card)
	{
		if (Ornament != null)
		{
			Ornament.Release(player);
			if (Ornament.SuitType != 0)
			{
				RemoveSuit(Ornament.SuitType, Ornament.CardCode);
			}
		}
		card.Equip(player);
		Ornament = card;
		if (Ornament.SuitType != 0)
		{
			AddSuit(Ornament.SuitType, Ornament.CardCode);
		}
		CharacterInfoUI characterInfoUI = SingletonDontDestroy<UIManager>.Instance.GetView("CharacterInfoUI") as CharacterInfoUI;
		if (characterInfoUI != null)
		{
			characterInfoUI.EquipOrnament(card.CardCode, isNeedHint: true);
		}
	}
}
