using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIView
{
	private const int NormalShopItemAmount = 6;

	private UIAnim_ShopUI anim;

	private ShopUISkeletonAnimCtrl _shopUiSkeletonAnimCtrl;

	private static readonly string[] ShopEnterBubbleKeys = new string[4] { "SHOPENTERBUBBLEKEY0", "SHOPENTERBUBBLEKEY1", "SHOPENTERBUBBLEKEY2", "SHOPENTERBUBBLEKEY3" };

	private static readonly string[] ShopPurchesBubbleKeys = new string[3] { "SHOPPURCHESBUBBLEKEY0", "SHOPPURCHESBUBBLEKEY1", "SHOPPURCHESBUBBLEKEY2" };

	private Transform normalShopPanel;

	private Queue<ShopItemSlotCtrl> shopItemSlotPool = new Queue<ShopItemSlotCtrl>();

	private Dictionary<string, ShopItemSlotCtrl> allShowingItems = new Dictionary<string, ShopItemSlotCtrl>();

	private int currentMapLayer;

	private int currentMapLevel;

	private ShopItemSlotCtrl currentItemSlot;

	private Text coinAmountText;

	private bool isPlayerPurchedItem;

	private Transform bubblePoint;

	public override string UIViewName => "ShopUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		CancelCurrentItemHighlight();
		currentItemSlot = null;
		GameTempData.ShopData = GetShopData();
		SingletonDontDestroy<UIManager>.Instance.HideView("BubbleTalkUI");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Shop UI...");
		EventManager.UnregisterEvent(EventEnum.E_PlayerAddEquipment, OnPlayerGetEquipment);
	}

	public override void OnSpawnUI()
	{
		_shopUiSkeletonAnimCtrl = base.transform.Find("Bg/Anim").GetComponent<ShopUISkeletonAnimCtrl>();
		Transform transform = base.transform.Find("Bg");
		normalShopPanel = transform.Find("ItemRoot");
		coinAmountText = transform.Find("CoinAmount").GetComponent<Text>();
		bubblePoint = base.transform.Find("Bg/BubblePoint");
		transform.Find("BackBtn").GetComponent<Button>().onClick.AddListener(OnClickBackBtn);
		EventManager.RegisterEvent(EventEnum.E_PlayerAddEquipment, OnPlayerGetEquipment);
		anim = GetComponent<UIAnim_ShopUI>();
	}

	public override void ReInitUI()
	{
		base.ReInitUI();
		EventManager.RegisterEvent(EventEnum.E_PlayerAddEquipment, OnPlayerGetEquipment);
	}

	private void OnPlayerGetEquipment(EventData data)
	{
		string stringValue = ((SimpleEventData)data).stringValue;
		if (allShowingItems.Count > 0 && allShowingItems.TryGetValue(stringValue, out var value))
		{
			value.gameObject.SetActive(value: false);
			shopItemSlotPool.Enqueue(value);
			allShowingItems.Remove(stringValue);
		}
	}

	public void OnClickPurchaseBtn()
	{
		if (currentItemSlot != null && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(currentItemSlot.ItemPrice))
		{
			int itemPrice = currentItemSlot.ItemPrice;
			currentItemSlot.gameObject.SetActive(value: false);
			shopItemSlotPool.Enqueue(currentItemSlot);
			allShowingItems.Remove(currentItemSlot.CurrentItemCode);
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddEquipment(currentItemSlot.CurrentItemCode);
			currentItemSlot = null;
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-" + itemPrice, UIView.CoinColor, Color.black, coinAmountText.transform, isSetParent: false, Vector3.zero);
			coinAmountText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("购买商品");
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("VoiceAct/商店大叔_买东西");
			isPlayerPurchedItem = true;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowShopBubble(ShopPurchesBubbleKeys[Random.Range(0, ShopPurchesBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
			_shopUiSkeletonAnimCtrl.PlayPurchasedAnim();
		}
	}

	private void OnClickBackBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("离开商店");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound(isPlayerPurchedItem ? "VoiceAct/商店大叔_买东西离开" : "VoiceAct/商店大叔_没买东西离开");
		SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
		SingletonDontDestroy<UIManager>.Instance.HideView("ShopUI");
		if (isPlayerPurchedItem)
		{
			GameSave.SaveGame();
		}
	}

	public void SetCurrentChooseItem(ShopItemSlotCtrl itemSlotCtrl)
	{
		currentItemSlot = itemSlotCtrl;
	}

	private void CancelCurrentItemHighlight()
	{
		if (currentItemSlot != null)
		{
			currentItemSlot = null;
		}
	}

	public void ShowShop(int mapLevel, int mapLayer, int roomSeed)
	{
		_shopUiSkeletonAnimCtrl.PlayIdle();
		normalShopPanel.gameObject.SetActive(value: true);
		LoadShopInfo(mapLevel, mapLayer, roomSeed);
		coinAmountText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入商店");
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("卡牌_商店界面_背景音乐", isReplaceMainBgm: false);
		isPlayerPurchedItem = false;
		int num = Random.Range(0, ShopEnterBubbleKeys.Length);
		(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowShopBubble(ShopEnterBubbleKeys[num].LocalizeText(), bubblePoint.position);
		List<Transform> list = new List<Transform>();
		foreach (KeyValuePair<string, ShopItemSlotCtrl> allShowingItem in allShowingItems)
		{
			list.Add(allShowingItem.Value.GetComponent<Transform>());
		}
		anim.SetItems(list);
		anim.StartAnim();
	}

	private void LoadShopInfo(int mapLevel, int mapLayer, int roomSeed)
	{
		RecycleAllItem();
		currentMapLayer = mapLayer;
		currentMapLevel = mapLevel;
		if (GameTempData.ShopData != null && mapLayer == GameTempData.ShopData.currentLayer && mapLevel == GameTempData.ShopData.currentLevel && GameTempData.ShopData.isEquipShop)
		{
			List<string> allItemCodes = GameTempData.ShopData.allItemCodes;
			for (int i = 0; i < allItemCodes.Count; i++)
			{
				ShopItemSlotCtrl shopItem = GetShopItem(normalShopPanel);
				shopItem.LoadItem(allItemCodes[i], i, this);
				allShowingItems.Add(allItemCodes[i], shopItem);
				if (allItemCodes[i] == GameTempData.ShopData.saledItemCode)
				{
					shopItem.SetItemSale();
				}
			}
		}
		else
		{
			Random.InitState(roomSeed);
			List<string> shopNewItemList = GetShopNewItemList();
			for (int j = 0; j < shopNewItemList.Count; j++)
			{
				ShopItemSlotCtrl shopItem2 = GetShopItem(normalShopPanel);
				shopItem2.LoadItem(shopNewItemList[j], j, this);
				allShowingItems.Add(shopNewItemList[j], shopItem2);
			}
			allShowingItems[shopNewItemList[Random.Range(0, allShowingItems.Count)]].SetItemSale();
		}
	}

	private List<string> GetShopNewItemList()
	{
		Dictionary<int, List<string>> allEquips = AllRandomInventory.Instance.AllSatisfiedEquipsWithMapLevel(4);
		List<string> list = new List<string>(6);
		switch (Singleton<GameManager>.Instance.CurrentMapLevel)
		{
		case 1:
			GetEquipsByMapLevel(list, 1, 6, allEquips);
			break;
		case 2:
			GetEquipsByMapLevel(list, 1, 2, allEquips);
			GetEquipsByMapLevel(list, 2, 4, allEquips);
			break;
		case 3:
		{
			List<string> list2 = AllRandomInventory.Instance.AllSatisfiedEpicSuitEquips(Singleton<GameManager>.Instance.Player.PlayerOccupation, EquipmentType.Ornament);
			if (list2.Count > 0)
			{
				GetEquipsByMapLevel(list, 2, 2, allEquips);
				GetEquipsByMapLevel(list, 3, 3, allEquips);
				list.Add(list2[Random.Range(0, list2.Count)]);
			}
			else
			{
				GetEquipsByMapLevel(list, 2, 3, allEquips);
				GetEquipsByMapLevel(list, 3, 3, allEquips);
			}
			break;
		}
		}
		return list;
	}

	private void GetEquipsByMapLevel(List<string> showEquips, int maplevel, int amount, Dictionary<int, List<string>> allEquips)
	{
		if (maplevel > 3)
		{
			return;
		}
		int i;
		for (i = 0; i < amount; i++)
		{
			if (allEquips[maplevel].Count <= 0)
			{
				break;
			}
			int index = Random.Range(0, allEquips[maplevel].Count);
			showEquips.Add(allEquips[maplevel][index]);
			allEquips[maplevel].RemoveAt(index);
		}
		if (i != amount)
		{
			GetEquipsByMapLevel(showEquips, maplevel + 1, amount - i, allEquips);
		}
	}

	private ShopItemSlotCtrl GetShopItem(Transform root)
	{
		if (shopItemSlotPool.Count > 0)
		{
			ShopItemSlotCtrl shopItemSlotCtrl = shopItemSlotPool.Dequeue();
			shopItemSlotCtrl.gameObject.SetActive(value: true);
			shopItemSlotCtrl.transform.SetParent(root);
			return shopItemSlotCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("ItemSlot", "Prefabs", root).GetComponent<ShopItemSlotCtrl>();
	}

	private void RecycleAllItem()
	{
		foreach (KeyValuePair<string, ShopItemSlotCtrl> allShowingItem in allShowingItems)
		{
			allShowingItem.Value.gameObject.SetActive(value: false);
			allShowingItem.Value.OnRecycle();
			shopItemSlotPool.Enqueue(allShowingItem.Value);
		}
		allShowingItems.Clear();
	}

	public ShopData GetShopData()
	{
		if (currentMapLevel == Singleton<GameManager>.Instance.CurrentMapLevel && currentMapLayer == Singleton<GameManager>.Instance.CurrentMapLayer)
		{
			List<string> list = new List<string>();
			string saledItemCode = string.Empty;
			foreach (KeyValuePair<string, ShopItemSlotCtrl> allShowingItem in allShowingItems)
			{
				list.Add(allShowingItem.Key);
				if (allShowingItem.Value.IsSaled)
				{
					saledItemCode = allShowingItem.Key;
				}
			}
			return new ShopData
			{
				currentLayer = currentMapLayer,
				currentLevel = currentMapLevel,
				isEquipShop = true,
				allItemCodes = list,
				saledItemCode = saledItemCode
			};
		}
		return null;
	}
}
