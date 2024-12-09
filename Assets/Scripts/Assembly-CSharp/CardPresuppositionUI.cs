using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardPresuppositionUI : EscCloseUIView, IBagCardDrag
{
	private abstract class CardPanel_Handler
	{
		protected CardPresuppositionUI parentUI;

		protected Button btn;

		public CardPanel_Handler(CardPresuppositionUI parentUI, Button btn)
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
		public CardPanel_AllHandler(CardPresuppositionUI parentUI, Button btn)
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
		public CardPanel_MainHandler(CardPresuppositionUI parentUI, Button btn)
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
		public CardPanel_SupHand(CardPresuppositionUI parentUI, Button btn)
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

	private const float CardScale = 0.95f;

	private Button saveBtn;

	private Button resetBtn;

	private UIAnim_CardPresupposition anim;

	private bool currentPresuppositionChanged;

	public Sprite presuppositionNormal;

	public Sprite presuppositionChosen;

	public Sprite presuppositionHighlight;

	private Queue<BagCardCtrl> allBagCardsPool = new Queue<BagCardCtrl>();

	private Queue<BagCardSlotCtrl> allBagCardSlotsPool = new Queue<BagCardSlotCtrl>();

	private const string CannotEmptyKey = "CannotEmptyKey";

	private const string SamePresuppositionNameKey = "SamePresuppositionNameKey";

	private const int MaxPresuppositionAmount = 5;

	private const PlayerOccupation DefaultShowOccupation = PlayerOccupation.Archer;

	private Transform presuppositionListRoot;

	private Dictionary<string, SinglePresuppositionCtrl> allShowingPresuppostions = new Dictionary<string, SinglePresuppositionCtrl>(5);

	private Queue<SinglePresuppositionCtrl> allPresuppositionPool = new Queue<SinglePresuppositionCtrl>();

	private PlayerOccupation currentShowingOccupation;

	private CardPresuppositionStruct currentShowingTmpPresupposition;

	private OccupationSingleSign currentShowingOccuSign;

	private Dictionary<PlayerOccupation, OccupationSingleSign> allSigns;

	private Transform otherOccupationPanel;

	private bool isOtherOccupationPanelShowing;

	private Transform opePanel;

	private Button deleteBtn;

	private Button varifyBtn;

	private GameObject dragCardObj;

	private UsualNoDesCardInfo cardInfo;

	private string currentDragCardCode;

	private bool currentDragCardIsEquiped;

	[Header("卡牌界面")]
	public Sprite cardTypeHighlight;

	public Sprite cardTypeNormal;

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

	private Tween mainhandHintImgHintTween;

	private Tween suphandHintImgHintTween;

	private CardPanel_Handler currentHandler;

	private CardPanel_AllHandler _allHandler;

	private CardPanel_MainHandler _mainHandler;

	private CardPanel_SupHand _supHandler;

	public override string UIViewName => "CardPresuppositionUI";

	public override string UILayerName => "NormalLayer";

	public bool IsDragingCard { get; set; }

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		OpenBagCardPanel();
		anim.StartAnim(allShowingEquipedMainHandCardSlots, allShowingEquipedSupHandCardSlots, allShowingEquipedMainHandCards, allShowingEquipedSupHandCards);
		anim.SetSlotsAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickReturnBtn();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		currentHandler?.Hide();
		currentHandler = null;
		dragCardObj.SetActive(value: false);
		RecycleShowingInventoryCard();
		RecycleEquipedHandCard();
		RecycleAllPresuppositionCtrls();
		HideOtherOccupationPanel();
		CancelHighlightAllBgCardsBg();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		InitPresuppositionList();
		InitCardsPanel();
		InitDragCard();
		InitCardPanelHandler();
		base.transform.Find("Bg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
		saveBtn = base.transform.Find("Bg/SaveBtn").GetComponent<Button>();
		saveBtn.onClick.AddListener(OnClickSaveBtn);
		resetBtn = base.transform.Find("Bg/ResetBtn").GetComponent<Button>();
		resetBtn.onClick.AddListener(OnClickResetBtn);
		anim = GetComponent<UIAnim_CardPresupposition>();
		anim.Init();
	}

	private void OnClickSaveBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("保存预设按钮");
		SingletonDontDestroy<Game>.Instance.CurrentUserData.SavePresupposition(currentShowingOccupation, currentShowingTmpPresupposition);
		GameSave.SaveUserData();
		ResetCurrentPresuppositionChanged();
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowOneChosenSystemHint("SaveSuccess".LocalizeText(), null);
	}

	private void OnClickResetBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("重置预设按钮");
		ShowPresuppositionContent(SingletonDontDestroy<Game>.Instance.CurrentUserData.GetCardPresuppositionByIndex(currentShowingOccupation, currentShowingTmpPresupposition.index));
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		string text = PresuppositionCheck();
		if (!text.IsNullOrEmpty())
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowOneChosenSystemHint(text, null);
		}
		else if (currentPresuppositionChanged)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("PresuppositionNotSaveQuestion".LocalizeText(), delegate
			{
				SingletonDontDestroy<UIManager>.Instance.HideView(this);
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
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

	private void InitPresuppositionList()
	{
		presuppositionListRoot = base.transform.Find("Bg/PresuppositionList");
		currentShowingTmpPresupposition = null;
		InitOpePanel();
		InitOccupationSignPanel();
	}

	private void InitOccupationSignPanel()
	{
		currentShowingOccuSign = base.transform.Find("Bg/OccupationSingleSign").GetComponent<OccupationSingleSign>();
		allSigns = new Dictionary<PlayerOccupation, OccupationSingleSign>();
		otherOccupationPanel = base.transform.Find("Bg/OtherOccupationPanel");
		for (int i = 0; i < otherOccupationPanel.childCount; i++)
		{
			OccupationSingleSign component = otherOccupationPanel.GetChild(i).GetComponent<OccupationSingleSign>();
			allSigns.Add(component.PlayerOccupation, component);
		}
		base.transform.Find("Bg/OccupationSingleSign").GetComponent<Button>().onClick.AddListener(ShowOrHidingOtherOccupationPanel);
	}

	private void ShowOrHidingOtherOccupationPanel()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("切换角色母按钮");
		if (isOtherOccupationPanelShowing)
		{
			HideOtherOccupationPanel();
		}
		else
		{
			ShowOtherOccupationPanel();
		}
	}

	private void ShowOtherOccupationPanel()
	{
		isOtherOccupationPanelShowing = true;
		otherOccupationPanel.gameObject.SetActive(value: true);
		foreach (KeyValuePair<PlayerOccupation, OccupationSingleSign> allSign in allSigns)
		{
			if (allSign.Key == currentShowingOccupation)
			{
				allSign.Value.SetChosen();
			}
			else
			{
				allSign.Value.SetNotChosen();
			}
		}
	}

	private void HideOtherOccupationPanel()
	{
		isOtherOccupationPanelShowing = false;
		otherOccupationPanel.gameObject.SetActive(value: false);
	}

	public void SwitchToOtherOccupation(PlayerOccupation playerOccupation)
	{
		HideOtherOccupationPanel();
		if (playerOccupation != 0 && currentShowingOccuSign.PlayerOccupation != playerOccupation)
		{
			SetCurrentOccupation(playerOccupation);
			LoadNewOccupationPresupposition(playerOccupation);
		}
	}

	public void SetCurrentOccupation(PlayerOccupation playerOccupation)
	{
		currentShowingOccupation = playerOccupation;
		currentShowingOccuSign.PlayerOccupation = playerOccupation;
		OccupationData occupationData = DataManager.Instance.GetOccupationData(playerOccupation);
		currentShowingOccuSign.SetOccupation(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.CardPresuppositionSignSpriteName, occupationData.DefaultSpritePath), occupationData.OccupationNameKey.LocalizeText());
	}

	private void LoadDefaultOccupationPresuppositions()
	{
		SetCurrentOccupation(PlayerOccupation.Archer);
		LoadNewOccupationPresupposition(currentShowingOccupation);
	}

	private void LoadNewOccupationPresupposition(PlayerOccupation playerOccupation)
	{
		List<CardPresuppositionStruct> list = SingletonDontDestroy<Game>.Instance.CurrentUserData.AllInitCardPresupposition[playerOccupation];
		RecycleAllPresuppositionCtrls();
		for (int i = 0; i < list.Count; i++)
		{
			AddNewSinglePresuppositionCtrl(list[i], i);
		}
		if (allShowingPresuppostions.Count < 5)
		{
			AddNewSinglePresuppositionCtrl(null);
		}
		ShowPresuppositionContent(list[0]);
	}

	private void ShowPresuppositionContent(CardPresuppositionStruct presuppositionStruct)
	{
		currentShowingTmpPresupposition = new CardPresuppositionStruct(presuppositionStruct);
		LoadPresuppositionCards(SingletonDontDestroy<Game>.Instance.CurrentUserData, currentShowingOccupation, currentShowingTmpPresupposition);
		allShowingPresuppostions[presuppositionStruct.Name].SetChosen();
		ShowPresuppositionOpePanel(presuppositionStruct.index);
	}

	private void AddNewSinglePresuppositionCtrl(CardPresuppositionStruct cardPresuppositionStruct, int sibliIndex = -1)
	{
		SinglePresuppositionCtrl singlePresupposition = GetSinglePresupposition();
		singlePresupposition.LoadPresupposition(this, cardPresuppositionStruct);
		singlePresupposition.transform.SetSiblingIndex((sibliIndex == -1) ? allShowingPresuppostions.Count : sibliIndex);
		allShowingPresuppostions.Add((cardPresuppositionStruct == null) ? string.Empty : cardPresuppositionStruct.Name, singlePresupposition);
	}

	private SinglePresuppositionCtrl GetSinglePresupposition()
	{
		if (allPresuppositionPool.Count > 0)
		{
			SinglePresuppositionCtrl singlePresuppositionCtrl = allPresuppositionPool.Dequeue();
			singlePresuppositionCtrl.gameObject.SetActive(value: true);
			return singlePresuppositionCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SinglePresupposition", "Prefabs", presuppositionListRoot).GetComponent<SinglePresuppositionCtrl>();
	}

	private void RecycleAllPresuppositionCtrls()
	{
		if (allShowingPresuppostions.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, SinglePresuppositionCtrl> allShowingPresuppostion in allShowingPresuppostions)
		{
			allShowingPresuppostion.Value.gameObject.SetActive(value: false);
			allPresuppositionPool.Enqueue(allShowingPresuppostion.Value);
		}
		allShowingPresuppostions.Clear();
	}

	private void RecyclePresuppositionCtrl(string presuppositionName)
	{
		if (allShowingPresuppostions.TryGetValue(presuppositionName, out var value))
		{
			value.gameObject.SetActive(value: false);
			allPresuppositionPool.Enqueue(value);
			value.transform.SetAsLastSibling();
			allShowingPresuppostions.Remove(presuppositionName);
		}
	}

	public void TryAddNewPresupposition()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("CreateNewPresuppositionUI", string.Empty) as CreateNewPresuppositionUI).ShowCreateNewPresuppositionPanel(CheckPresuppositionLeague, OnConfirmCreateNewPresupposition);
	}

	public void OnConfirmCreateNewPresupposition(string presuppositionName)
	{
		CardPresuppositionStruct cardPresuppositionStruct = SingletonDontDestroy<Game>.Instance.CurrentUserData.AddNewPresupposition(currentShowingOccupation, presuppositionName);
		if (allShowingPresuppostions.Count == 5)
		{
			SinglePresuppositionCtrl singlePresuppositionCtrl = allShowingPresuppostions[string.Empty];
			allShowingPresuppostions.Remove(string.Empty);
			singlePresuppositionCtrl.LoadPresupposition(this, cardPresuppositionStruct);
			allShowingPresuppostions.Add(presuppositionName, singlePresuppositionCtrl);
		}
		else
		{
			AddNewSinglePresuppositionCtrl(cardPresuppositionStruct, allShowingPresuppostions.Count);
		}
		GameSave.SaveUserData();
	}

	public void TryShowChosenPresupposition(int index)
	{
		if (currentPresuppositionChanged)
		{
			SystemHintUI systemHintUI = SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI;
			string text = PresuppositionCheck();
			if (!text.IsNullOrEmpty())
			{
				systemHintUI.ShowOneChosenSystemHint(text, null);
				return;
			}
			systemHintUI.ShowTwoChosenSystemHint("PresuppositionNotSaveQuestion".LocalizeText(), delegate
			{
				ConfirmShowChosenPresupposition(index);
			});
		}
		else
		{
			ConfirmShowChosenPresupposition(index);
		}
	}

	private void ConfirmShowChosenPresupposition(int index)
	{
		allShowingPresuppostions[currentShowingTmpPresupposition.Name].SetNormal();
		CardPresuppositionStruct cardPresuppositionByIndex = SingletonDontDestroy<Game>.Instance.CurrentUserData.GetCardPresuppositionByIndex(currentShowingOccupation, index);
		ShowPresuppositionContent(cardPresuppositionByIndex);
	}

	private void ConfirmDeleteCurrentShowingPresupposition()
	{
		SingletonDontDestroy<Game>.Instance.CurrentUserData.DeletePresupposition(currentShowingOccupation, currentShowingTmpPresupposition.index);
		GameSave.SaveUserData();
		RecyclePresuppositionCtrl(currentShowingTmpPresupposition.Name);
		ShowPresuppositionContent(SingletonDontDestroy<Game>.Instance.CurrentUserData.GetCardPresuppositionByIndex(currentShowingOccupation, 0));
	}

	public void ConfirmVarifyCurrentShowingPresuppositionName(string newName)
	{
		SingletonDontDestroy<Game>.Instance.CurrentUserData.VarifyPresuppositionName(currentShowingOccupation, currentShowingTmpPresupposition.index, newName);
		GameSave.SaveUserData();
		SinglePresuppositionCtrl value = allShowingPresuppostions[currentShowingTmpPresupposition.Name];
		allShowingPresuppostions.Remove(currentShowingTmpPresupposition.Name);
		currentShowingTmpPresupposition.Name = newName;
		allShowingPresuppostions.Add(newName, value);
	}

	public string CheckPresuppositionLeague(string newName)
	{
		if (newName.IsNullOrEmpty())
		{
			return "CannotEmptyKey".LocalizeText();
		}
		if (allShowingPresuppostions.ContainsKey(newName))
		{
			return "SamePresuppositionNameKey".LocalizeText();
		}
		return string.Empty;
	}

	private void InitOpePanel()
	{
		opePanel = base.transform.Find("Bg/PresuppositionList/OpePanel");
		deleteBtn = opePanel.Find("DeleteBtn").GetComponent<Button>();
		deleteBtn.onClick.AddListener(OnOnClickDeletePresuppositionBtn);
		varifyBtn = opePanel.Find("VarifyBtn").GetComponent<Button>();
		varifyBtn.onClick.AddListener(OnClickVarifyPresuppositionBtn);
	}

	private void OnOnClickDeletePresuppositionBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("删除按钮");
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("DeletePresuppositionQuestion".LocalizeText(), ConfirmDeleteCurrentShowingPresupposition);
	}

	private void OnClickVarifyPresuppositionBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("修改按钮");
		allShowingPresuppostions[currentShowingTmpPresupposition.Name].EditPresuppositionName();
	}

	private void ShowPresuppositionOpePanel(int preIndex)
	{
		opePanel.transform.SetSiblingIndex(preIndex + 1);
		if (preIndex == 0)
		{
			deleteBtn.interactable = false;
			varifyBtn.interactable = false;
		}
		else
		{
			deleteBtn.interactable = true;
			varifyBtn.interactable = true;
		}
	}

	private void InitDragCard()
	{
		dragCardObj = base.transform.Find("Bg/BagCard").gameObject;
		cardInfo = dragCardObj.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
	}

	public GameObject StartDragCard(string cardCode, bool isEquiped)
	{
		IsDragingCard = true;
		currentDragCardIsEquiped = isEquiped;
		currentDragCardCode = cardCode;
		dragCardObj.SetActive(value: true);
		cardInfo.LoadCard(cardCode);
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

	private void InitCardsPanel()
	{
		Transform transform = base.transform.Find("Bg");
		inventoryScrollRect = transform.Find("CardRoot").GetComponent<ScrollRect>();
		inventoryCardPoolRoot = transform.Find("CardRoot/Mask/Content");
		mainHandCardRoot = transform.Find("EquipedCardBg/MainHandCards/Mask/Content");
		mainHandScrollRect = transform.Find("EquipedCardBg/MainHandCards").GetComponent<ScrollRect>();
		supHandCardRoot = transform.Find("EquipedCardBg/SupHandCards/Mask/Content");
		supHandScrollRect = transform.Find("EquipedCardBg/SupHandCards").GetComponent<ScrollRect>();
		mainHandCardBg = transform.Find("EquipedCardBg/MainHandCards/Bg").GetComponent<Image>();
		mainHandCardFeatherImg = transform.Find("EquipedCardBg/MainHandCards/Feather").GetComponent<Image>();
		supHandCardBg = transform.Find("EquipedCardBg/SupHandCards/Bg").GetComponent<Image>();
		supHandCardFeatherImg = transform.Find("EquipedCardBg/SupHandCards/Feather").GetComponent<Image>();
		equipedMainCardAmountText = transform.Find("EquipedCardBg/MainHandCards/Title/Amount").GetComponent<Text>();
		equipedSupCardAmountText = transform.Find("EquipedCardBg/SupHandCards/Title/Amount").GetComponent<Text>();
		mainhandCardPanel = transform.Find("EquipedCardBg/MainHandCards");
		suphandCardPanel = transform.Find("EquipedCardBg/SupHandCards");
		transform.Find("EquipedCardBg/MainHandCards/Mask").GetComponent<OnMouseEventCallback>().EnterEventTrigger.Event.AddListener(OnMouseEnterMainHandCardPanel);
		transform.Find("EquipedCardBg/MainHandCards/Mask").GetComponent<OnMouseEventCallback>().ExitEventTrigger.Event.AddListener(OnMouseExitHandCardPanel);
		transform.Find("EquipedCardBg/SupHandCards/Mask").GetComponent<OnMouseEventCallback>().EnterEventTrigger.Event.AddListener(OnMouseEnterSupHandCardPanel);
		transform.Find("EquipedCardBg/SupHandCards/Mask").GetComponent<OnMouseEventCallback>().ExitEventTrigger.Event.AddListener(OnMouseExitHandCardPanel);
		hideCannotUseBtnImg = transform.Find("HideCannotUseBtn").GetComponent<Image>();
		hideCannotUseBtnImg.GetComponent<Button>().onClick.AddListener(OnClickHideCannotUseBtnBtn);
		cardRootScrollbar = transform.Find("CardRoot/Mask/Scrollbar").GetComponent<Scrollbar>();
		inventoryImg = base.transform.Find("Bg/CardRoot/Bg").GetComponent<Image>();
		mainHandHintImg = base.transform.Find("Bg/EquipedCardBg/MainHandCards/Title/HintImg").GetComponent<Image>();
		mainHandHintPos = base.transform.Find("Bg/EquipedCardBg/MainHandCards/Title/HintPos");
		supHandHintImg = base.transform.Find("Bg/EquipedCardBg/SupHandCards/Title/HintImg").GetComponent<Image>();
		supHandHintPos = base.transform.Find("Bg/EquipedCardBg/SupHandCards/Title/HintPos");
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

	private void OpenBagCardPanel()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("打开卡牌界面");
		LoadDefaultOccupationPresuppositions();
	}

	private void LoadPresuppositionCards(UserData userData, PlayerOccupation occupation, CardPresuppositionStruct presuppositionStruct)
	{
		ResetCurrentPresuppositionChanged();
		RecycleShowingInventoryCard();
		RecycleEquipedHandCard();
		LoadMainHandCards(presuppositionStruct.MainHandcards);
		LoadSupHandCards(presuppositionStruct.SupHandCards);
		ShowPanel();
	}

	private void ResetCurrentPresuppositionChanged()
	{
		currentPresuppositionChanged = false;
		saveBtn.interactable = false;
		resetBtn.interactable = false;
	}

	private void AnyChangeOccureToCurrentPresupposition()
	{
		currentPresuppositionChanged = true;
		saveBtn.interactable = equipedMainCardAmount >= 12 && equipedSupCardAmount >= 12;
		resetBtn.interactable = true;
	}

	private void HighlightMainHandCardsBg()
	{
		mainHandCardBg.sprite = cardBgHighlightSprite;
		mainHandCardFeatherImg.sprite = cardBgHighlightFeatherSprite;
	}

	private void CancelHighlightAllBgCardsBg()
	{
		Sprite sprite3 = (supHandCardBg.sprite = (mainHandCardBg.sprite = cardBgNormalSprite));
		sprite3 = (supHandCardFeatherImg.sprite = (mainHandCardFeatherImg.sprite = cardBgNormalFeatherSprite));
	}

	private void HighlightSupHandCardsBg()
	{
		supHandCardBg.sprite = cardBgHighlightSprite;
		supHandCardFeatherImg.sprite = cardBgHighlightFeatherSprite;
	}

	private string PresuppositionCheck()
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

	private void ShowAllInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadMainHandCardInventory(SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedMainHandCards[currentShowingOccupation], currentShowingTmpPresupposition.MainHandcards);
		LoadSupHandCardInventory(SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSupHandCards[currentShowingOccupation], currentShowingTmpPresupposition.SupHandCards);
		anim.SetSlotsAnim(allShowingInventoryBagCardSlots, allShowingInventoryBagCards);
	}

	private void ShowMainHandInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadMainHandCardInventory(SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedMainHandCards[currentShowingOccupation], currentShowingTmpPresupposition.MainHandcards);
	}

	private void ShowSupHandInventoryCards()
	{
		RecycleShowingInventoryCard();
		LoadSupHandCardInventory(SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSupHandCards[currentShowingOccupation], currentShowingTmpPresupposition.SupHandCards);
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
			currentShowingTmpPresupposition.EquipMainHandCard(cardCode, 1);
			AddHandCard(cardCode, currentShowingTmpPresupposition.GetMainHandCardAmount(cardCode), isMain: true);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
			AnyChangeOccureToCurrentPresupposition();
			VarifyMainHandCardAnimHint(isAdd: true);
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
			currentShowingTmpPresupposition.EquipSupHandCard(cardCode, 1);
			AddHandCard(cardCode, currentShowingTmpPresupposition.GetSupHandCardAmount(cardCode), isMain: false);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
			AnyChangeOccureToCurrentPresupposition();
			VarifySupHandCardAnimHint(isAdd: true);
		}
		else
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
			AddCardToInventory(cardCode);
		}
	}

	public void RemoveNewCard(string cardCode)
	{
	}

	private void EquipCard(string cardCode, bool isMain)
	{
		if (isMain)
		{
			currentShowingTmpPresupposition.EquipMainHandCard(cardCode, 1);
			AddHandCard(cardCode, currentShowingTmpPresupposition.GetMainHandCardAmount(cardCode), isMain: true);
		}
		else
		{
			currentShowingTmpPresupposition.EquipSupHandCard(cardCode, 1);
			AddHandCard(cardCode, currentShowingTmpPresupposition.GetSupHandCardAmount(cardCode), isMain: false);
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
			currentShowingTmpPresupposition.ReleaseMainHandCard(cardCode, 1);
			RemoveHandCard(cardCode, currentShowingTmpPresupposition.MainHandcards.ContainsKey(cardCode) ? currentShowingTmpPresupposition.MainHandcards[cardCode] : 0, isMain: true);
		}
		else if (!isMain && allShowingEquipedSupHandCards.ContainsKey(cardCode))
		{
			currentShowingTmpPresupposition.RelaseSupHandCard(cardCode, 1);
			RemoveHandCard(cardCode, currentShowingTmpPresupposition.SupHandCards.ContainsKey(cardCode) ? currentShowingTmpPresupposition.SupHandCards[cardCode] : 0, isMain: false);
		}
	}

	private void TryRemoveHandCard(string cardCode, bool isMain)
	{
		if (isMain && !currentShowingTmpPresupposition.MainHandcards.ContainsKey(cardCode))
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
		else if (!isMain && !currentShowingTmpPresupposition.SupHandCards.ContainsKey(cardCode))
		{
			BagCardCtrl bagCardCtrl2 = allShowingEquipedSupHandCards[cardCode];
			BagCardSlotCtrl bagCardSlotCtrl2 = allShowingEquipedSupHandCardSlots[cardCode];
			bagCardCtrl2.gameObject.SetActive(value: false);
			allBagCardsPool.Enqueue(bagCardCtrl2);
			bagCardCtrl2.CanvasGroup.alpha = 1f;
			allShowingEquipedSupHandCards.Remove(cardCode);
			bagCardSlotCtrl2.gameObject.SetActive(value: false);
			bagCardSlotCtrl2.CanvasGroup.alpha = 1f;
			allBagCardSlotsPool.Enqueue(bagCardSlotCtrl2);
			allShowingEquipedSupHandCardSlots.Remove(cardCode);
		}
	}

	public void TryAddCardToInventory(string cardCode, bool isMain)
	{
		if (currentMousePanel != -1)
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
			EquipCard(cardCode, isMain);
		}
		else
		{
			ForceAddCardToInventory(cardCode, isMain);
		}
	}

	public void ForceAddCardToInventory(string cardCode, bool isMain)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		TryRemoveHandCard(cardCode, isMain);
		AddCardToInventory(cardCode);
		AnyChangeOccureToCurrentPresupposition();
		if (isMain)
		{
			VarifyMainHandCardAnimHint(isAdd: false);
		}
		else
		{
			VarifySupHandCardAnimHint(isAdd: false);
		}
	}

	private void AddCardToInventory(string cardCode)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		int num = 0;
		num = ((usualCardAttr.HandFlag != 0) ? (SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSupHandCards[currentShowingOccupation][cardCode] - currentShowingTmpPresupposition.GetSupHandCardAmount(cardCode)) : (SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedMainHandCards[currentShowingOccupation][cardCode] - currentShowingTmpPresupposition.GetMainHandCardAmount(cardCode)));
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
		if (usualCardAttr.HandFlag == HandFlag.MainHand)
		{
			int num2 = SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedMainHandCards[currentShowingOccupation][cardCode] - currentShowingTmpPresupposition.GetMainHandCardAmount(cardCode);
			flag = num2 > 0;
			num = num2 - 1;
		}
		else
		{
			int num3 = SingletonDontDestroy<Game>.Instance.CurrentUserData.AllUnlockedSupHandCards[currentShowingOccupation][cardCode] - currentShowingTmpPresupposition.GetSupHandCardAmount(cardCode);
			flag = num3 > 0;
			num = num3 - 1;
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

	private void LoadMainHandCards(Dictionary<string, int> mainCards)
	{
		int num = 0;
		foreach (KeyValuePair<string, int> mainCard in mainCards)
		{
			LoadSingleBagCard(mainHandCardRoot, mainCard.Key, isEquiped: true, isMain: true, mainCard.Value, mainHandScrollRect, string.Empty, ref allShowingEquipedMainHandCards, ref allShowingEquipedMainHandCardSlots);
			if (!DataManager.Instance.GetUsualCardAttr(mainCard.Key).IsComsumeableCard)
			{
				num += mainCard.Value;
			}
		}
		equipedMainCardAmount = num;
		equipedMainCardAmountText.text = num.ToString();
	}

	private void LoadSupHandCards(Dictionary<string, int> supCards)
	{
		int num = 0;
		foreach (KeyValuePair<string, int> supCard in supCards)
		{
			LoadSingleBagCard(supHandCardRoot, supCard.Key, isEquiped: true, isMain: false, supCard.Value, supHandScrollRect, string.Empty, ref allShowingEquipedSupHandCards, ref allShowingEquipedSupHandCardSlots);
			if (!DataManager.Instance.GetUsualCardAttr(supCard.Key).IsComsumeableCard)
			{
				num += supCard.Value;
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

	private void LoadMainHandCardInventory(Dictionary<string, int> allUnlockedCards, Dictionary<string, int> mainhandCards)
	{
		foreach (KeyValuePair<string, int> allUnlockedCard in allUnlockedCards)
		{
			int num = allUnlockedCard.Value;
			if (mainhandCards.TryGetValue(allUnlockedCard.Key, out var value))
			{
				num -= value;
			}
			BagCardSlotCtrl bagCardSlotCtrl = LoadSingleBagCard(inventoryCardPoolRoot, allUnlockedCard.Key, isEquiped: false, isMain: false, num, inventoryScrollRect, "MainHand".LocalizeText(), ref allShowingInventoryBagCards, ref allShowingInventoryBagCardSlots);
			if (isHideCannnotUse && num == 0)
			{
				bagCardSlotCtrl.gameObject.SetActive(value: false);
			}
		}
	}

	private void LoadSupHandCardInventory(Dictionary<string, int> allUnlockedCards, Dictionary<string, int> suphandCards)
	{
		foreach (KeyValuePair<string, int> allUnlockedCard in allUnlockedCards)
		{
			int num = allUnlockedCard.Value;
			if (suphandCards.TryGetValue(allUnlockedCard.Key, out var value))
			{
				num -= value;
			}
			BagCardSlotCtrl bagCardSlotCtrl = LoadSingleBagCard(inventoryCardPoolRoot, allUnlockedCard.Key, isEquiped: false, isMain: false, num, inventoryScrollRect, "SupHand".LocalizeText(), ref allShowingInventoryBagCards, ref allShowingInventoryBagCardSlots);
			if (isHideCannnotUse && num == 0)
			{
				bagCardSlotCtrl.gameObject.SetActive(value: false);
			}
		}
	}

	private BagCardSlotCtrl LoadSingleBagCard(Transform root, string cardCode, bool isEquiped, bool isMain, int amount, ScrollRect rect, string handFlag, ref Dictionary<string, BagCardCtrl> showingCards, ref Dictionary<string, BagCardSlotCtrl> showingSlots)
	{
		BagCardSlotCtrl bagCardSlot = GetBagCardSlot(root);
		bagCardSlot.transform.SetParent(root);
		bagCardSlot.transform.SetSiblingIndex(showingSlots.Count);
		BagCardCtrl bagCard = GetBagCard(root);
		bagCard.LoadCard(cardCode, this, isEquiped, isNew: false, isMain, amount, rect);
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
		_allHandler = new CardPanel_AllHandler(this, base.transform.Find("Bg/CardBtn/ShowAllCardBtn").GetComponent<Button>());
		_mainHandler = new CardPanel_MainHandler(this, base.transform.Find("Bg/CardBtn/ShowMainHandCardBtn").GetComponent<Button>());
		_supHandler = new CardPanel_SupHand(this, base.transform.Find("Bg/CardBtn/ShowSupHandCardBtn").GetComponent<Button>());
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
}
