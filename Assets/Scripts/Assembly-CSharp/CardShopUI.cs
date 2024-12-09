using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShopUI : UIView
{
	private const int MaxItemAmount = 6;

	private UIAnim_ShopUI anim;

	private ShopUISkeletonAnimCtrl _shopUiSkeletonAnimCtrl;

	private static readonly string[] ShopEnterBubbleKeys = new string[4] { "SHOPENTERBUBBLEKEY0", "SHOPENTERBUBBLEKEY1", "SHOPENTERBUBBLEKEY2", "SHOPENTERBUBBLEKEY3" };

	private static readonly string[] ShopPurchesBubbleKeys = new string[3] { "SHOPPURCHESBUBBLEKEY0", "SHOPPURCHESBUBBLEKEY1", "SHOPPURCHESBUBBLEKEY2" };

	private Text playerMoneyText;

	private Transform cardItemRoot;

	private Queue<ShopCardItemCtrl> shopItemSlotPool = new Queue<ShopCardItemCtrl>();

	private Dictionary<string, ShopCardItemCtrl> allShowingItems = new Dictionary<string, ShopCardItemCtrl>();

	private int currentMapLayer;

	private int currentMapLevel;

	private ShopCardItemCtrl currentItemSlot;

	private bool isPlayerPurchedItem;

	private Transform bubblePoint;

	private Transform keyRoot;

	private RectTransform rectKeyRoot;

	private Queue<KeyCtrl> allKeysPool = new Queue<KeyCtrl>();

	private List<KeyCtrl> allShowingKeys = new List<KeyCtrl>();

	public override string UIViewName => "CardShopUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		currentItemSlot = null;
		GameTempData.ShopData = GetShopData();
		SingletonDontDestroy<UIManager>.Instance.HideView("BubbleTalkUI");
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		_shopUiSkeletonAnimCtrl = base.transform.Find("Bg/Anim").GetComponent<ShopUISkeletonAnimCtrl>();
		playerMoneyText = base.transform.Find("Bg/PlayerMoney").GetComponent<Text>();
		cardItemRoot = base.transform.Find("Bg/CardItemRoot");
		bubblePoint = base.transform.Find("Bg/BubblePoint");
		base.transform.Find("Bg/QuitBtn").GetComponent<Button>().onClick.AddListener(OnClickQuitBtn);
		InitKeyRoot();
		anim = GetComponent<UIAnim_ShopUI>();
	}

	public void ShowShop(int mapLevel, int mapLayer, int roomseed)
	{
		_shopUiSkeletonAnimCtrl.PlayIdle();
		LoadShopInfo(mapLevel, mapLayer, roomseed);
		playerMoneyText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("进入商店");
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGM("卡牌_商店界面_背景音乐", isReplaceMainBgm: false);
		isPlayerPurchedItem = false;
		(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowShopBubble(ShopEnterBubbleKeys[Random.Range(0, ShopEnterBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
		List<Transform> list = new List<Transform>();
		foreach (KeyValuePair<string, ShopCardItemCtrl> allShowingItem in allShowingItems)
		{
			list.Add(allShowingItem.Value.GetComponent<Transform>());
		}
		anim.SetItems(list);
		anim.StartAnim();
	}

	private void LoadShopInfo(int mapLevel, int mapLayer, int RoomSeed)
	{
		RecycleAllItem();
		if (GameTempData.ShopData != null && mapLayer == GameTempData.ShopData.currentLayer && mapLevel == GameTempData.ShopData.currentLevel && !GameTempData.ShopData.isEquipShop)
		{
			List<string> allItemCodes = GameTempData.ShopData.allItemCodes;
			for (int i = 0; i < allItemCodes.Count; i++)
			{
				ShopCardItemCtrl shopItem = GetShopItem();
				shopItem.LoadCard(allItemCodes[i], this);
				allShowingItems.Add(allItemCodes[i], shopItem);
				if (allItemCodes[i] == GameTempData.ShopData.saledItemCode)
				{
					shopItem.SetSale();
				}
			}
			return;
		}
		currentMapLayer = mapLayer;
		currentMapLevel = mapLevel;
		Random.InitState(RoomSeed);
		List<string> list = AllRandomInventory.Instance.AllStatisfiedSpecialUsualCards(2);
		int num = 0;
		List<string> list2 = new List<string>(6);
		while (list.Count > 0 && num < 6)
		{
			ShopCardItemCtrl shopItem2 = GetShopItem();
			int index = Random.Range(0, list.Count);
			shopItem2.LoadCard(list[index], this);
			allShowingItems.Add(list[index], shopItem2);
			list2.Add(list[index]);
			list.RemoveAt(index);
			num++;
		}
		allShowingItems[list2[Random.Range(0, allShowingItems.Count)]].SetSale();
	}

	private ShopCardItemCtrl GetShopItem()
	{
		if (shopItemSlotPool.Count > 0)
		{
			ShopCardItemCtrl shopCardItemCtrl = shopItemSlotPool.Dequeue();
			shopCardItemCtrl.gameObject.SetActive(value: true);
			return shopCardItemCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("ShopCardItem", "Prefabs", cardItemRoot).GetComponent<ShopCardItemCtrl>();
	}

	public void OnClickPurchaesBtn()
	{
		if (currentItemSlot != null && Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerComsumeMoney(currentItemSlot.Price))
		{
			int price = currentItemSlot.Price;
			currentItemSlot.gameObject.SetActive(value: false);
			shopItemSlotPool.Enqueue(currentItemSlot);
			allShowingItems.Remove(currentItemSlot.CurrentCardCode);
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(currentItemSlot.CurrentCardCode, 1, isNew: true);
			currentItemSlot = null;
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("-" + price, UIView.CoinColor, Color.black, playerMoneyText.transform, isSetParent: false, Vector3.zero);
			playerMoneyText.text = Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney + "G";
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("购买商品");
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("VoiceAct/商店大叔_买东西");
			isPlayerPurchedItem = true;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowShopBubble(ShopPurchesBubbleKeys[Random.Range(0, ShopPurchesBubbleKeys.Length)].LocalizeText(), bubblePoint.position);
			_shopUiSkeletonAnimCtrl.PlayPurchasedAnim();
		}
	}

	private void OnClickQuitBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("离开商店");
		SingletonDontDestroy<AudioManager>.Instance.PlaySound(isPlayerPurchedItem ? "VoiceAct/商店大叔_买东西离开" : "VoiceAct/商店大叔_没买东西离开");
		SingletonDontDestroy<AudioManager>.Instance.RecoveryToMainBGM();
		SingletonDontDestroy<UIManager>.Instance.HideView("CardShopUI");
		if (isPlayerPurchedItem)
		{
			GameSave.SaveGame();
		}
	}

	public void SetCurrentChooseItem(ShopCardItemCtrl itemSlotCtrl)
	{
		currentItemSlot = itemSlotCtrl;
	}

	private void RecycleAllItem()
	{
		if (allShowingItems.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, ShopCardItemCtrl> allShowingItem in allShowingItems)
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
			foreach (KeyValuePair<string, ShopCardItemCtrl> allShowingItem in allShowingItems)
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
				isEquipShop = false,
				allItemCodes = list,
				saledItemCode = saledItemCode
			};
		}
		return null;
	}

	private void InitKeyRoot()
	{
		keyRoot = base.transform.Find("Bg/KeyRoot");
		rectKeyRoot = keyRoot.GetComponent<RectTransform>();
	}

	public void ShowKeys(Transform root, List<KeyValuePair> allKeys)
	{
		if (allKeys != null && allKeys.Count > 0)
		{
			keyRoot.position = root.position;
			for (int i = 0; i < allKeys.Count; i++)
			{
				KeyCtrl keyCtrl = GetKeyCtrl();
				keyCtrl.LoadKey(allKeys[i].key.LocalizeText(), allKeys[i].value.LocalizeText());
				allShowingKeys.Add(keyCtrl);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(rectKeyRoot);
		}
	}

	public void HideKeys()
	{
		if (allShowingKeys.Count <= 0)
		{
			return;
		}
		foreach (KeyCtrl allShowingKey in allShowingKeys)
		{
			allShowingKey.gameObject.SetActive(value: false);
			allKeysPool.Enqueue(allShowingKey);
		}
		allKeysPool.Clear();
	}

	private KeyCtrl GetKeyCtrl()
	{
		if (allKeysPool.Count > 0)
		{
			KeyCtrl keyCtrl = allKeysPool.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", keyRoot).GetComponent<KeyCtrl>();
	}
}
