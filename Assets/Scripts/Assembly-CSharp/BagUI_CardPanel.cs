using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BagUI_CardPanel : MonoBehaviour, IBagCardDrag
{
	private abstract class CardPanel_Handler
	{
		protected BagUI_CardPanel parentUI;

		protected Button btn;

		public CardPanel_Handler(BagUI_CardPanel parentUI, Button btn)
		{
			this.parentUI = parentUI;
			btn.onClick.AddListener(Show);
			this.btn = btn;
		}

		private void Show()
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("翻页_点击标签_通用");
			OnSetThisType();
		}

		public abstract void OnSetThisType();

		public void Hide()
		{
			btn.interactable = true;
			OnHide();
		}

		protected abstract void OnHide();
	}

	private class CardPanel_AllHandler : CardPanel_Handler
	{
		public CardPanel_AllHandler(BagUI_CardPanel parentUI, Button btn)
			: base(parentUI, btn)
		{
		}

		public override void OnSetThisType()
		{
			parentUI.SwtichHandler(this);
			btn.interactable = false;
			parentUI.ShowAllInventoryCards();
		}

		protected override void OnHide()
		{
		}
	}

	private class CardPanel_MainHandler : CardPanel_Handler
	{
		public CardPanel_MainHandler(BagUI_CardPanel parentUI, Button btn)
			: base(parentUI, btn)
		{
		}

		public override void OnSetThisType()
		{
			parentUI.SwtichHandler(this);
			btn.interactable = false;
			parentUI.ShowMainHandInventoryCards();
		}

		protected override void OnHide()
		{
		}
	}

	private class CardPanel_SupHand : CardPanel_Handler
	{
		public CardPanel_SupHand(BagUI_CardPanel parentUI, Button btn)
			: base(parentUI, btn)
		{
		}

		public override void OnSetThisType()
		{
			parentUI.SwtichHandler(this);
			btn.interactable = false;
			parentUI.ShowSupHandInventoryCards();
		}

		protected override void OnHide()
		{
		}
	}

	private class CardPanel_SpecialHandler : CardPanel_Handler
	{
		public CardPanel_SpecialHandler(BagUI_CardPanel parentUI, Button btn)
			: base(parentUI, btn)
		{
		}

		public override void OnSetThisType()
		{
			parentUI.SwtichHandler(this);
			btn.interactable = false;
			parentUI.ShowOccupationInventoryCards();
		}

		protected override void OnHide()
		{
		}
	}

	private const float CardScale = 0.95f;

	[Header("卡牌界面")]
	public Sprite HideCannotUseBtnActive;

	public Sprite HideCannotUseBtnNotActive;

	public Sprite cardBgNormalSprite;

	public Sprite cardBgHighlightSprite;

	public Sprite cardBgNormalFeatherSprite;

	public Sprite cardBgHighlightFeatherSprite;

	public Sprite cardAddHintSprite;

	public Sprite cardReduceHintSprite;

	public Sprite inventoryCardNormalSprite;

	public Sprite inventoruCardHighlightSprite;

	public Color addCardHintColor;

	public Color reduceCardHintColor;

	private const string CardAmountSatisfiedColor = "#ffffffff";

	private const string CardAmountNotSatisfiedColor = "#ff0000ff";

	private bool isHideCannnotUse;

	private Image hideCannotUseBtnImg;

	private Transform cardPanel;

	private Transform inventoryCardPoolRoot;

	private ScrollRect inventoryScrollRect;

	private Dictionary<string, BagCardCtrl> allShowingInventoryBagCards = new Dictionary<string, BagCardCtrl>();

	private Dictionary<string, BagCardSlotCtrl> allShowingInventoryBagCardSlots = new Dictionary<string, BagCardSlotCtrl>();

	private Transform mainHandCardRoot;

	private ScrollRect mainHandScrollRect;

	private Dictionary<string, BagCardCtrl> allShowingEquipedMainHandCards = new Dictionary<string, BagCardCtrl>();

	private Dictionary<string, BagCardSlotCtrl> allShowingEquipedMainHandCardSlots = new Dictionary<string, BagCardSlotCtrl>();

	private Transform supHandCardRoot;

	private ScrollRect supHandScrollRect;

	private Dictionary<string, BagCardCtrl> allShowingEquipedSupHandCards = new Dictionary<string, BagCardCtrl>();

	private Dictionary<string, BagCardSlotCtrl> allShowingEquipedSupHandCardSlots = new Dictionary<string, BagCardSlotCtrl>();

	private Text equipedMainCardAmountText;

	private int equipedMainCardAmount;

	private Text equipedSupCardAmountText;

	private int equipedSupCardAmount;

	private int currentMousePanel;

	private Scrollbar cardRootScrollbar;

	private Tween cardRootScrollbarTween;

	private Transform mainhandCardPanel;

	private Transform suphandCardPanel;

	private Image mainHandCardBg;

	private Image mainHandCardFeatherImg;

	private Image supHandCardBg;

	private Image supHandCardFeatherImg;

	private Image inventoryImg;

	private Image mainHandHintImg;

	private Transform mainHandHintPos;

	private Image supHandHintImg;

	private Transform supHandHintPos;

	private BagUI parentUI;

	private Tween mainhandHintImgHintTween;

	private Tween suphandHintImgHintTween;

	private Queue<BagCardCtrl> allBagCardsPool = new Queue<BagCardCtrl>();

	private Queue<BagCardSlotCtrl> allBagCardSlotsPool = new Queue<BagCardSlotCtrl>();

	private CardPanel_Handler currentHandler;

	private CardPanel_AllHandler _allHandler;

	private CardPanel_MainHandler _mainHandler;

	private CardPanel_SupHand _supHandler;

	private CardPanel_SpecialHandler _specialHandler;

	private GameObject dragCardObj;

	private UsualNoDesCardInfo cardInfo;

	private string currentDragCardCode;

	private bool currentDragCardIsEquiped;

	public bool IsDragingCard { get; set; }

	public void InitCardPanel(BagUI parentUI, GameObject dragCardObj, UsualNoDesCardInfo cardInfo)
	{
		InitDragCard(dragCardObj, cardInfo);
		this.parentUI = parentUI;
		cardPanel = base.transform;
		inventoryScrollRect = cardPanel.Find("CardRoot").GetComponent<ScrollRect>();
		inventoryCardPoolRoot = cardPanel.Find("CardRoot/Mask/Content");
		mainHandCardRoot = cardPanel.Find("EquipedCardBg/MainHandCards/Mask/Content");
		mainHandScrollRect = cardPanel.Find("EquipedCardBg/MainHandCards").GetComponent<ScrollRect>();
		supHandCardRoot = cardPanel.Find("EquipedCardBg/SupHandCards/Mask/Content");
		supHandScrollRect = cardPanel.Find("EquipedCardBg/SupHandCards").GetComponent<ScrollRect>();
		mainHandCardBg = cardPanel.Find("EquipedCardBg/MainHandCards/Bg").GetComponent<Image>();
		supHandCardBg = cardPanel.Find("EquipedCardBg/SupHandCards/Bg").GetComponent<Image>();
		mainHandCardFeatherImg = cardPanel.Find("EquipedCardBg/MainHandCards/Feather").GetComponent<Image>();
		supHandCardFeatherImg = cardPanel.Find("EquipedCardBg/SupHandCards/Feather").GetComponent<Image>();
		equipedMainCardAmountText = cardPanel.Find("EquipedCardBg/MainHandCards/Title/Amount").GetComponent<Text>();
		equipedSupCardAmountText = cardPanel.Find("EquipedCardBg/SupHandCards/Title/Amount").GetComponent<Text>();
		mainhandCardPanel = cardPanel.Find("EquipedCardBg/MainHandCards");
		suphandCardPanel = cardPanel.Find("EquipedCardBg/SupHandCards");
		cardPanel.Find("EquipedCardBg/MainHandCards/Mask").GetComponent<OnMouseEventCallback>().EnterEventTrigger.Event.AddListener(OnMouseEnterMainHandCardPanel);
		cardPanel.Find("EquipedCardBg/MainHandCards/Mask").GetComponent<OnMouseEventCallback>().ExitEventTrigger.Event.AddListener(OnMouseExitHandCardPanel);
		cardPanel.Find("EquipedCardBg/SupHandCards/Mask").GetComponent<OnMouseEventCallback>().EnterEventTrigger.Event.AddListener(OnMouseEnterSupHandCardPanel);
		cardPanel.Find("EquipedCardBg/SupHandCards/Mask").GetComponent<OnMouseEventCallback>().ExitEventTrigger.Event.AddListener(OnMouseExitHandCardPanel);
		hideCannotUseBtnImg = cardPanel.Find("HideCannotUseBtn").GetComponent<Image>();
		hideCannotUseBtnImg.GetComponent<Button>().onClick.AddListener(OnClickHideCannotUseBtnBtn);
		cardRootScrollbar = cardPanel.Find("CardRoot/Mask/Scrollbar").GetComponent<Scrollbar>();
		inventoryImg = cardPanel.Find("CardRoot/Bg").GetComponent<Image>();
		mainHandHintImg = cardPanel.Find("EquipedCardBg/MainHandCards/Title/HintImg").GetComponent<Image>();
		mainHandHintPos = cardPanel.Find("EquipedCardBg/MainHandCards/Title/HintPos");
		supHandHintImg = cardPanel.Find("EquipedCardBg/SupHandCards/Title/HintImg").GetComponent<Image>();
		supHandHintPos = cardPanel.Find("EquipedCardBg/SupHandCards/Title/HintPos");
		InitCardPanelHandler();
	}

	public void Show()
	{
		cardPanel.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("打开卡牌界面");
		parentUI.AddGuideTips(new List<string>(1) { "Code_9_2", "Code_9_3" });
		LoadMainHandCards();
		LoadSupHandCards();
		parentUI.uiAnim.StartCardPanel(allShowingEquipedMainHandCardSlots, allShowingEquipedSupHandCardSlots, allShowingEquipedMainHandCards, allShowingEquipedSupHandCards);
		ShowPanel();
		EventManager.BroadcastEvent(EventEnum.E_OnOpenBagCardPanel, null);
	}

	public void Hide()
	{
		dragCardObj.SetActive(value: false);
		cardPanel.gameObject.SetActive(value: false);
		RecycleShowingInventoryCard();
		RecycleEquipedHandCard();
		CancelHighlightAllBgCardsBg();
		currentHandler?.Hide();
	}

	private void SetInventoryImg(bool isHighlight)
	{
		inventoryImg.sprite = (isHighlight ? inventoruCardHighlightSprite : inventoryCardNormalSprite);
	}

	private void VarifyMainHandCardAnimHint(bool isAdd)
	{
		if (mainhandHintImgHintTween != null && mainhandHintImgHintTween.IsActive())
		{
			mainhandHintImgHintTween.Complete();
		}
		mainHandHintImg.sprite = (isAdd ? cardAddHintSprite : cardReduceHintSprite);
		mainhandHintImgHintTween = mainHandHintImg.ImageFlashHint();
		if (isAdd)
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("+1", addCardHintColor, Color.black, mainHandHintPos, isSetParent: false, Vector3.zero);
		}
		else
		{
			Singleton<GameHintManager>.Instance.AddFlowingDownText_WorldPos("-1", reduceCardHintColor, Color.black, mainHandHintPos, isSetParent: false, Vector3.zero);
		}
	}

	private void VarifySupHandCardAnimHint(bool isAdd)
	{
		if (suphandHintImgHintTween != null && suphandHintImgHintTween.IsActive())
		{
			suphandHintImgHintTween.Complete();
		}
		supHandHintImg.sprite = (isAdd ? cardAddHintSprite : cardReduceHintSprite);
		suphandHintImgHintTween = supHandHintImg.ImageFlashHint();
		if (isAdd)
		{
			Singleton<GameHintManager>.Instance.AddFlowingText_WorldPos("+1", addCardHintColor, Color.black, supHandHintPos, isSetParent: false, Vector3.zero);
		}
		else
		{
			Singleton<GameHintManager>.Instance.AddFlowingDownText_WorldPos("-1", reduceCardHintColor, Color.black, supHandHintPos, isSetParent: false, Vector3.zero);
		}
	}

	private void HighlightMainHandCardsBg()
	{
		mainHandCardBg.sprite = cardBgHighlightSprite;
		mainHandCardFeatherImg.sprite = cardBgHighlightFeatherSprite;
	}

	private void CancelHighlightAllBgCardsBg()
	{
		Sprite sprite3 = (supHandCardBg.sprite = (mainHandCardBg.sprite = cardBgNormalSprite));
		sprite3 = (mainHandCardFeatherImg.sprite = (supHandCardFeatherImg.sprite = cardBgNormalFeatherSprite));
	}

	private void HighlightSupHandCardsBg()
	{
		supHandCardBg.sprite = cardBgHighlightSprite;
		supHandCardFeatherImg.sprite = cardBgHighlightFeatherSprite;
	}

	public string CloseBagCardPanelCheck()
	{
		if (equipedMainCardAmount < 12)
		{
			return "EquipedMainHandCardNotSatisfied".LocalizeText();
		}
		if (equipedSupCardAmount < 12)
		{
			return "EquipedSupHandCardNotSatisfied".LocalizeText();
		}
		return string.Empty;
	}

	public void OnClickHideCannotUseBtnBtn()
	{
		if (isHideCannnotUse)
		{
			isHideCannnotUse = false;
			hideCannotUseBtnImg.sprite = HideCannotUseBtnNotActive;
			foreach (KeyValuePair<string, BagCardSlotCtrl> allShowingInventoryBagCardSlot in allShowingInventoryBagCardSlots)
			{
				if (allShowingInventoryBagCardSlot.Value.CardAmount == 0)
				{
					allShowingInventoryBagCardSlot.Value.gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			isHideCannnotUse = true;
			hideCannotUseBtnImg.sprite = HideCannotUseBtnActive;
			foreach (KeyValuePair<string, BagCardSlotCtrl> allShowingInventoryBagCardSlot2 in allShowingInventoryBagCardSlots)
			{
				if (allShowingInventoryBagCardSlot2.Value.CardAmount == 0)
				{
					allShowingInventoryBagCardSlot2.Value.gameObject.SetActive(value: false);
				}
			}
		}
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
	}

	private BagCardCtrl GetBagCard(Transform root)
	{
		if (allBagCardsPool.Count > 0)
		{
			BagCardCtrl bagCardCtrl = allBagCardsPool.Dequeue();
			bagCardCtrl.CanvasGroup.alpha = 1f;
			bagCardCtrl.gameObject.SetActive(value: true);
			return bagCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BagCard", "Prefabs", root).GetComponent<BagCardCtrl>();
	}

	private BagCardSlotCtrl GetBagCardSlot(Transform root)
	{
		if (allBagCardSlotsPool.Count > 0)
		{
			BagCardSlotCtrl bagCardSlotCtrl = allBagCardSlotsPool.Dequeue();
			bagCardSlotCtrl.CanvasGroup.alpha = 1f;
			bagCardSlotCtrl.gameObject.SetActive(value: true);
			return bagCardSlotCtrl;
		}
		GameObject obj = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BagCardSlot", "Prefabs", root);
		obj.transform.localScale = Vector3.one * 0.95f;
		return obj.GetComponent<BagCardSlotCtrl>();
	}

	private void ShowAllInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadMainHandCardInventory();
		LoadSupHandCardInventory();
		LoadOccupationCardInventory();
		parentUI.uiAnim.ResetCardSlotAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	private void ShowMainHandInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadMainHandCardInventory();
		parentUI.uiAnim.ResetCardSlotAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	private void ShowSupHandInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadSupHandCardInventory();
		parentUI.uiAnim.ResetCardSlotAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	private void ShowOccupationInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadOccupationCardInventory();
		parentUI.uiAnim.ResetCardSlotAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	public void TryEquipCard(string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (currentMousePanel == 0)
		{
			if (usualCardAttr.HandFlag != 0 && usualCardAttr.HandFlag != HandFlag.BothHand)
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
				AddCardToInventory(cardCode);
				return;
			}
			if (allShowingInventoryBagCardSlots.TryGetValue(cardCode, out var value) && value.CardAmount == 0 && isHideCannnotUse)
			{
				value.gameObject.SetActive(value: false);
			}
			parentUI.SetChanged(isChanged: true);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipMainHandCard(cardCode, 1);
			AddHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards[cardCode], isMain: true);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		}
		else if (currentMousePanel == 1)
		{
			if (usualCardAttr.HandFlag != HandFlag.SupHand && usualCardAttr.HandFlag != HandFlag.BothHand)
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
				AddCardToInventory(cardCode);
				return;
			}
			if (allShowingInventoryBagCardSlots.TryGetValue(cardCode, out var value2) && value2.CardAmount == 0 && isHideCannnotUse)
			{
				value2.gameObject.SetActive(value: false);
			}
			parentUI.SetChanged(isChanged: true);
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipSupHandCard(cardCode, 1);
			AddHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards[cardCode], isMain: false);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		}
		else
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
			AddCardToInventory(cardCode);
		}
	}

	public void RemoveNewCard(string cardCode)
	{
		if (!Singleton<GameManager>.Instance.Player.PlayerInventory.RemoveNewCard(cardCode))
		{
			parentUI.CancelBagNewCardImg();
		}
	}

	private void EquipCard(string cardCode, bool isMain)
	{
		if (isMain)
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipMainHandCard(cardCode, 1);
			AddHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards[cardCode], isMain: true);
		}
		else
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.EquipSupHandCard(cardCode, 1);
			AddHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards[cardCode], isMain: false);
		}
	}

	private void AddHandCard(string cardCode, int finalAmount, bool isMain)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (isMain)
		{
			if (allShowingEquipedMainHandCardSlots.TryGetValue(cardCode, out var value))
			{
				value.UpdateAmount(finalAmount);
			}
			else
			{
				BagCardSlotCtrl bagCardSlot = GetBagCardSlot(mainHandCardRoot);
				bagCardSlot.transform.SetParent(mainHandCardRoot);
				BagCardCtrl bagCard = GetBagCard(mainHandCardRoot);
				bagCard.LoadCard(cardCode, this, isEquiped: true, isNew: false, isMain: true, 1, mainHandScrollRect);
				bagCardSlot.SetCard(bagCard, "MainHand".LocalizeText(), 1, isEquiped: true);
				allShowingEquipedMainHandCards.Add(cardCode, bagCard);
				allShowingEquipedMainHandCardSlots.Add(cardCode, bagCardSlot);
			}
			if (!usualCardAttr.IsComsumeableCard)
			{
				equipedMainCardAmount++;
				VarifyMainHandCardAnimHint(isAdd: true);
			}
			SetMainHandCardAmountText();
		}
		else
		{
			if (allShowingEquipedSupHandCardSlots.TryGetValue(cardCode, out var value2))
			{
				value2.UpdateAmount(finalAmount);
			}
			else
			{
				BagCardSlotCtrl bagCardSlot2 = GetBagCardSlot(supHandCardRoot);
				bagCardSlot2.transform.SetParent(supHandCardRoot);
				BagCardCtrl bagCard2 = GetBagCard(supHandCardRoot);
				bagCard2.LoadCard(cardCode, this, isEquiped: true, isNew: false, isMain: false, 1, supHandScrollRect);
				bagCardSlot2.SetCard(bagCard2, "SupHand".LocalizeText(), 1, isEquiped: true);
				allShowingEquipedSupHandCards.Add(cardCode, bagCard2);
				allShowingEquipedSupHandCardSlots.Add(cardCode, bagCardSlot2);
			}
			if (!usualCardAttr.IsComsumeableCard)
			{
				equipedSupCardAmount++;
				VarifySupHandCardAnimHint(isAdd: true);
			}
			SetSupHandCardAmountText();
		}
	}

	private void RemoveHandCard(string cardCode, int finalAmount, bool isMain)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (isMain)
		{
			allShowingEquipedMainHandCardSlots[cardCode].UpdateAmount(finalAmount);
			if (!usualCardAttr.IsComsumeableCard)
			{
				equipedMainCardAmount--;
			}
			SetMainHandCardAmountText();
		}
		else
		{
			allShowingEquipedSupHandCardSlots[cardCode].UpdateAmount(finalAmount);
			if (!usualCardAttr.IsComsumeableCard)
			{
				equipedSupCardAmount--;
			}
			SetSupHandCardAmountText();
		}
	}

	private void SetMainHandCardAmountText()
	{
		equipedMainCardAmountText.text = "<color=" + ((equipedMainCardAmount >= 12) ? "#ffffffff" : "#ff0000ff") + ">" + equipedMainCardAmount + "</color>";
	}

	private void SetSupHandCardAmountText()
	{
		equipedSupCardAmountText.text = "<color=" + ((equipedSupCardAmount >= 12) ? "#ffffffff" : "#ff0000ff") + ">" + equipedSupCardAmount + "</color>";
	}

	public void ReleaseCard(string cardCode, bool isMain)
	{
		if (isMain && allShowingEquipedMainHandCards.ContainsKey(cardCode))
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseMainHandCard(cardCode, 1);
			RemoveHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards.ContainsKey(cardCode) ? Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards[cardCode] : 0, isMain: true);
		}
		else if (!isMain && allShowingEquipedSupHandCards.ContainsKey(cardCode))
		{
			Singleton<GameManager>.Instance.Player.PlayerBattleInfo.ReleaseSupHandCard(cardCode, 1);
			RemoveHandCard(cardCode, Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards.ContainsKey(cardCode) ? Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards[cardCode] : 0, isMain: false);
		}
	}

	private void TryRemoveHandCard(string cardCode, bool isMain)
	{
		if (isMain && !Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards.ContainsKey(cardCode))
		{
			BagCardCtrl bagCardCtrl = allShowingEquipedMainHandCards[cardCode];
			BagCardSlotCtrl bagCardSlotCtrl = allShowingEquipedMainHandCardSlots[cardCode];
			bagCardCtrl.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(bagCardCtrl);
			allShowingEquipedMainHandCards.Remove(cardCode);
			bagCardSlotCtrl.gameObject.SetActive(value: false);
			allBagCardSlotsPool.Enqueue(bagCardSlotCtrl);
			allShowingEquipedMainHandCardSlots.Remove(cardCode);
		}
		else if (!isMain && !Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards.ContainsKey(cardCode))
		{
			BagCardCtrl bagCardCtrl2 = allShowingEquipedSupHandCards[cardCode];
			BagCardSlotCtrl bagCardSlotCtrl2 = allShowingEquipedSupHandCardSlots[cardCode];
			bagCardCtrl2.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(bagCardCtrl2);
			allShowingEquipedSupHandCards.Remove(cardCode);
			bagCardSlotCtrl2.gameObject.SetActive(value: false);
			allBagCardSlotsPool.Enqueue(bagCardSlotCtrl2);
			allShowingEquipedSupHandCardSlots.Remove(cardCode);
		}
	}

	public void TryAddCardToInventory(string cardCode, bool isMain)
	{
		if (currentMousePanel != -1)
		{
			if (!TrySwapCardPanel(cardCode, isMain))
			{
				SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
				EquipCard(cardCode, isMain);
			}
		}
		else
		{
			ForceAddCardToInventory(cardCode, isMain);
		}
	}

	public void ForceAddCardToInventory(string cardCode, bool isMain)
	{
		parentUI.SetChanged(isChanged: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		TryRemoveHandCard(cardCode, isMain);
		AddCardToInventory(cardCode);
		if (!DataManager.Instance.GetUsualCardAttr(cardCode).IsComsumeableCard)
		{
			if (isMain)
			{
				VarifyMainHandCardAnimHint(isAdd: false);
			}
			else
			{
				VarifySupHandCardAnimHint(isAdd: false);
			}
		}
	}

	private bool TrySwapCardPanel(string cardCode, bool isMain)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		if (usualCardAttr.HandFlag != HandFlag.BothHand)
		{
			return false;
		}
		if ((isMain && currentMousePanel == 1) || (!isMain && currentMousePanel == 0))
		{
			TryRemoveHandCard(cardCode, isMain);
			EquipCard(cardCode, !isMain);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
			if (!usualCardAttr.IsComsumeableCard)
			{
				if (isMain)
				{
					VarifyMainHandCardAnimHint(isAdd: false);
				}
				else
				{
					VarifySupHandCardAnimHint(isAdd: false);
				}
			}
			return true;
		}
		return false;
	}

	private void AddCardToInventory(string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		int num = 0;
		switch (usualCardAttr.HandFlag)
		{
		case HandFlag.BothHand:
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSpecialUsualCards(cardCode, 1, isNew: false);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSpecialUsualCards[cardCode];
			break;
		case HandFlag.MainHand:
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddMainHandCards(cardCode, 1);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllMainHandCards[cardCode];
			break;
		default:
			Singleton<GameManager>.Instance.Player.PlayerInventory.AddSupHandCards(cardCode, 1);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSupHandCards[cardCode];
			break;
		}
		if (!allShowingInventoryBagCardSlots.TryGetValue(cardCode, out var value))
		{
			return;
		}
		value.UpdateAmount(num);
		if (num > 0)
		{
			allShowingInventoryBagCards[cardCode].SetCardAmountMoreThanZeroColor();
			if (!value.gameObject.activeSelf)
			{
				value.gameObject.SetActive(value: true);
			}
		}
		else
		{
			allShowingInventoryBagCards[cardCode].SetCardAmountEqualZeroColor();
		}
	}

	public bool RemoveCardFromInventory(string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		int num = 0;
		bool flag = false;
		switch (usualCardAttr.HandFlag)
		{
		case HandFlag.BothHand:
			flag = Singleton<GameManager>.Instance.Player.PlayerInventory.ReduceSpecialUsualCards(cardCode, 1);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSpecialUsualCards[cardCode];
			break;
		case HandFlag.MainHand:
			flag = Singleton<GameManager>.Instance.Player.PlayerInventory.ReduceMainHandCards(cardCode, 1);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllMainHandCards[cardCode];
			break;
		default:
			flag = Singleton<GameManager>.Instance.Player.PlayerInventory.ReduceSupHandCards(cardCode, 1);
			num = Singleton<GameManager>.Instance.Player.PlayerInventory.AllSupHandCards[cardCode];
			break;
		}
		if (!flag)
		{
			return false;
		}
		if (allShowingInventoryBagCardSlots.TryGetValue(cardCode, out var value))
		{
			value.UpdateAmount(num);
			if (num > 0)
			{
				allShowingInventoryBagCards[cardCode].SetCardAmountMoreThanZeroColor();
			}
			else
			{
				allShowingInventoryBagCards[cardCode].SetCardAmountEqualZeroColor();
			}
		}
		return true;
	}

	private void LoadMainHandCards()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> allEquipedMainHandCard in Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards)
		{
			LoadSingleBagCard(mainHandCardRoot, allEquipedMainHandCard.Key, isEquiped: true, isNew: false, isMain: true, allEquipedMainHandCard.Value, mainHandScrollRect, string.Empty, ref allShowingEquipedMainHandCards, ref allShowingEquipedMainHandCardSlots);
			if (!DataManager.Instance.GetUsualCardAttr(allEquipedMainHandCard.Key).IsComsumeableCard)
			{
				num += allEquipedMainHandCard.Value;
			}
		}
		equipedMainCardAmount = num;
		equipedMainCardAmountText.text = num.ToString();
	}

	private void LoadSupHandCards()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> allEquipedSupHandCard in Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards)
		{
			LoadSingleBagCard(supHandCardRoot, allEquipedSupHandCard.Key, isEquiped: true, isNew: false, isMain: false, allEquipedSupHandCard.Value, supHandScrollRect, string.Empty, ref allShowingEquipedSupHandCards, ref allShowingEquipedSupHandCardSlots);
			if (!DataManager.Instance.GetUsualCardAttr(allEquipedSupHandCard.Key).IsComsumeableCard)
			{
				num += allEquipedSupHandCard.Value;
			}
		}
		equipedSupCardAmount = num;
		equipedSupCardAmountText.text = num.ToString();
	}

	private void RecycleEquipedHandCard()
	{
		foreach (KeyValuePair<string, BagCardCtrl> allShowingEquipedMainHandCard in allShowingEquipedMainHandCards)
		{
			allShowingEquipedMainHandCard.Value.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(allShowingEquipedMainHandCard.Value);
		}
		allShowingEquipedMainHandCards.Clear();
		foreach (KeyValuePair<string, BagCardCtrl> allShowingEquipedSupHandCard in allShowingEquipedSupHandCards)
		{
			allShowingEquipedSupHandCard.Value.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(allShowingEquipedSupHandCard.Value);
		}
		allShowingEquipedSupHandCards.Clear();
		foreach (KeyValuePair<string, BagCardSlotCtrl> allShowingEquipedMainHandCardSlot in allShowingEquipedMainHandCardSlots)
		{
			allShowingEquipedMainHandCardSlot.Value.gameObject.SetActive(value: false);
			allBagCardSlotsPool.Enqueue(allShowingEquipedMainHandCardSlot.Value);
		}
		allShowingEquipedMainHandCardSlots.Clear();
		foreach (KeyValuePair<string, BagCardSlotCtrl> allShowingEquipedSupHandCardSlot in allShowingEquipedSupHandCardSlots)
		{
			allShowingEquipedSupHandCardSlot.Value.gameObject.SetActive(value: false);
			allBagCardSlotsPool.Enqueue(allShowingEquipedSupHandCardSlot.Value);
		}
		allShowingEquipedSupHandCardSlots.Clear();
	}

	private void LoadOccupationCardInventory()
	{
		foreach (KeyValuePair<string, int> allSpecialUsualCard in Singleton<GameManager>.Instance.Player.PlayerInventory.AllSpecialUsualCards)
		{
			BagCardSlotCtrl bagCardSlotCtrl = LoadSingleBagCard(inventoryCardPoolRoot, allSpecialUsualCard.Key, isEquiped: false, Singleton<GameManager>.Instance.Player.PlayerInventory.IsPlayerInventoryContainNewCard(allSpecialUsualCard.Key), isMain: false, allSpecialUsualCard.Value, inventoryScrollRect, "OccupationHand".LocalizeText(), ref allShowingInventoryBagCards, ref allShowingInventoryBagCardSlots);
			if (isHideCannnotUse && allSpecialUsualCard.Value == 0)
			{
				bagCardSlotCtrl.gameObject.SetActive(value: false);
			}
		}
	}

	private void LoadMainHandCardInventory()
	{
		foreach (KeyValuePair<string, int> allMainHandCard in Singleton<GameManager>.Instance.Player.PlayerInventory.AllMainHandCards)
		{
			BagCardSlotCtrl bagCardSlotCtrl = LoadSingleBagCard(inventoryCardPoolRoot, allMainHandCard.Key, isEquiped: false, isNew: false, isMain: false, allMainHandCard.Value, inventoryScrollRect, "MainHand".LocalizeText(), ref allShowingInventoryBagCards, ref allShowingInventoryBagCardSlots);
			if (isHideCannnotUse && allMainHandCard.Value == 0)
			{
				bagCardSlotCtrl.gameObject.SetActive(value: false);
			}
		}
	}

	private void LoadSupHandCardInventory()
	{
		foreach (KeyValuePair<string, int> allSupHandCard in Singleton<GameManager>.Instance.Player.PlayerInventory.AllSupHandCards)
		{
			BagCardSlotCtrl bagCardSlotCtrl = LoadSingleBagCard(inventoryCardPoolRoot, allSupHandCard.Key, isEquiped: false, isNew: false, isMain: false, allSupHandCard.Value, inventoryScrollRect, "SupHand".LocalizeText(), ref allShowingInventoryBagCards, ref allShowingInventoryBagCardSlots);
			if (isHideCannnotUse && allSupHandCard.Value == 0)
			{
				bagCardSlotCtrl.gameObject.SetActive(value: false);
			}
		}
	}

	private BagCardSlotCtrl LoadSingleBagCard(Transform root, string cardCode, bool isEquiped, bool isNew, bool isMain, int amount, ScrollRect rect, string handFlag, ref Dictionary<string, BagCardCtrl> showingCards, ref Dictionary<string, BagCardSlotCtrl> showingSlots)
	{
		BagCardSlotCtrl bagCardSlot = GetBagCardSlot(root);
		bagCardSlot.transform.SetParent(root);
		bagCardSlot.transform.SetSiblingIndex(showingSlots.Count);
		BagCardCtrl bagCard = GetBagCard(root);
		bagCard.LoadCard(cardCode, this, isEquiped, isNew, isMain, amount, rect);
		bagCardSlot.SetCard(bagCard, handFlag, amount, isEquiped);
		showingCards.Add(cardCode, bagCard);
		showingSlots.Add(cardCode, bagCardSlot);
		return bagCardSlot;
	}

	private void RecycleShowingInventoryCard()
	{
		foreach (KeyValuePair<string, BagCardCtrl> allShowingInventoryBagCard in allShowingInventoryBagCards)
		{
			allShowingInventoryBagCard.Value.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(allShowingInventoryBagCard.Value);
		}
		allShowingInventoryBagCards.Clear();
		foreach (KeyValuePair<string, BagCardSlotCtrl> allShowingInventoryBagCardSlot in allShowingInventoryBagCardSlots)
		{
			allShowingInventoryBagCardSlot.Value.gameObject.SetActive(value: false);
			allBagCardSlotsPool.Enqueue(allShowingInventoryBagCardSlot.Value);
		}
		allShowingInventoryBagCardSlots.Clear();
	}

	public void OnMouseEnterMainHandCardPanel()
	{
		currentMousePanel = 0;
	}

	public void OnMouseEnterSupHandCardPanel()
	{
		currentMousePanel = 1;
	}

	public void OnMouseExitHandCardPanel()
	{
		currentMousePanel = -1;
	}

	private void InitCardPanelHandler()
	{
		_allHandler = new CardPanel_AllHandler(this, base.transform.Find("CardBtn/ShowAllCardBtn").GetComponent<Button>());
		_mainHandler = new CardPanel_MainHandler(this, base.transform.Find("CardBtn/ShowMainHandCardBtn").GetComponent<Button>());
		_supHandler = new CardPanel_SupHand(this, base.transform.Find("CardBtn/ShowSupHandCardBtn").GetComponent<Button>());
		_specialHandler = new CardPanel_SpecialHandler(this, base.transform.Find("CardBtn/ShowOccupationCardBtn").GetComponent<Button>());
	}

	private void ShowPanel()
	{
		_allHandler.OnSetThisType();
	}

	private void SwtichHandler(CardPanel_Handler handler)
	{
		currentHandler?.Hide();
		currentHandler = handler;
	}

	private void InitDragCard(GameObject dragCardObj, UsualNoDesCardInfo cardInfo)
	{
		this.dragCardObj = dragCardObj;
		this.cardInfo = cardInfo;
	}

	public GameObject StartDragCard(string cardCode, bool isEquiped)
	{
		currentDragCardIsEquiped = isEquiped;
		currentDragCardCode = cardCode;
		dragCardObj.SetActive(value: true);
		cardInfo.LoadCard(cardCode);
		IsDragingCard = true;
		switch (DataManager.Instance.GetUsualCardAttr(cardCode).HandFlag)
		{
		case HandFlag.MainHand:
			HighlightMainHandCardsBg();
			break;
		case HandFlag.SupHand:
			HighlightSupHandCardsBg();
			break;
		case HandFlag.BothHand:
			HighlightMainHandCardsBg();
			HighlightSupHandCardsBg();
			break;
		}
		if (isEquiped)
		{
			SetInventoryImg(isHighlight: true);
		}
		return dragCardObj;
	}

	public void EndDragCard()
	{
		dragCardObj.SetActive(value: false);
		IsDragingCard = false;
		if (!currentDragCardCode.IsNullOrEmpty())
		{
			CancelHighlightAllBgCardsBg();
		}
		SetInventoryImg(isHighlight: false);
		currentDragCardCode = string.Empty;
	}
}
