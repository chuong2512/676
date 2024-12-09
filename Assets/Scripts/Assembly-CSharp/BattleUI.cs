using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : UIView
{
	public enum KeyDesType
	{
		Left,
		Right
	}

	private CanvasGroup _canvasGroup;

	private UIAnim_BattleUI anim;

	public const float CardPanelRectY = 0.34f;

	private const string UsualCardPrefabName = "UsualCard";

	private const string CardPrefabPath = "Prefabs";

	public const float CardBaseSizeX = 140f;

	private Transform cardPanel;

	private Transform supHandCardRoot;

	private Transform mainHandCardRoot;

	private Transform readyCastCardRoot;

	private Image cardBgImg;

	private Queue<UsualCardController> allCardsObjPool = new Queue<UsualCardController>();

	private List<UsualCardController> allShowingSupHandCards = new List<UsualCardController>();

	private List<UsualCardController> allShowingMainHandCards = new List<UsualCardController>();

	private bool isShowingMainHandCard;

	private bool isMovingCard;

	private UsualCardController cardPlayerReadyCast;

	private Transform supDisPoint;

	private Transform supMovePoint;

	private Transform mainDisPoint;

	private Transform mainMovePoint;

	private Image mainhandDiscardImg;

	private Image mainhandCardPileImg;

	private Text mainhandDiscardText;

	private Text mainhandCardPileText;

	private Image suphandDiscardImg;

	private Image suphandCardPileImg;

	private Text suphandDiscardText;

	private Text suphandCardPileText;

	private static bool isAddingSupCard;

	private Queue<string> supCardsAddingQueue = new Queue<string>();

	private Action supcardDrawAnimOverAction;

	private static bool isAddingMainCard;

	private Queue<string> mainCardsAddingQueue = new Queue<string>();

	private Action maincardDrawAnimOverAction;

	private static bool isShuffleMainHandCard;

	private static bool isShuffleSupHandCard;

	private Transform systemPanel;

	private Transform playerInfoDetailBtnBg;

	private Transform buttonPanel;

	private Button storingForceBtn;

	private Image storingForceHighlightImg;

	private Button nextRoundBtn;

	private Image nextRoundHighlightImg;

	[Header("玩家信息的参数")]
	public Sprite apSpriteHighlight;

	public Sprite apLackSprite;

	private Image playerHeadProtraitImg;

	private UsualHealthBarCtrl _healthCtrl;

	private Transform playerInfoPanel;

	private Text atkDmgText;

	private Image atkDmgImg;

	private Text playerApAmountRemain;

	private Image playerApImg;

	private Text playerSpecialAttrRemain;

	private Image playerSpecialImg;

	private MessageHint playerSpecialAttrHint;

	private Image armorBg;

	private Text armorAmountText;

	private Image playerSelfHighlightImg;

	private Transform defenceCtrlRoot;

	private Tween playerArmorHintTween;

	private Tween playerApAmountHintTween;

	private Tween playerSpecialAttrHintTween;

	private Transform skillRoot;

	private Transform skillDesPoint;

	private Queue<BattleSkillSlotCtrl> battleSkillSlotPool = new Queue<BattleSkillSlotCtrl>();

	private Dictionary<string, BattleSkillSlotCtrl> allShowingBattleSkill = new Dictionary<string, BattleSkillSlotCtrl>();

	private List<UsualCardController> mainHighlightCards = new List<UsualCardController>();

	private List<UsualCardController> supHighlightCards = new List<UsualCardController>();

	private Transform buffPanel;

	private Queue<BuffIconCtrl> allBuffIconPool = new Queue<BuffIconCtrl>();

	private Dictionary<BuffType, BuffIconCtrl> allShowingBuffIcons = new Dictionary<BuffType, BuffIconCtrl>();

	private Transform equipEffectPanel;

	private Queue<EquipEffectIconCtrl> allEquipEffectIconPools = new Queue<EquipEffectIconCtrl>();

	private List<EquipEffectIconCtrl> allShowingEquipEffectIcon = new List<EquipEffectIconCtrl>();

	private Transform keyPanelLeftRoot;

	private RectTransform keyPanelLeftRectTrans;

	private Transform keyPanelRightRoot;

	private RectTransform keyPanelRightRectTrans;

	private Queue<KeyCtrl> allShowingKeyCtrls = new Queue<KeyCtrl>();

	private Queue<KeyCtrl> allKeyCtrlPool = new Queue<KeyCtrl>();

	private Coroutine keyCor;

	public override string UILayerName => "NormalLayer";

	public override string UIViewName => "BattleUI";

	public Transform bubbleHintPoint { get; private set; }

	public static bool IsDrawingMainCard
	{
		get
		{
			if (!isAddingMainCard)
			{
				return isShuffleMainHandCard;
			}
			return true;
		}
	}

	public static bool isDrawingSupCard
	{
		get
		{
			if (!isAddingSupCard)
			{
				return isShuffleSupHandCard;
			}
			return true;
		}
	}

	public Transform MainHandCardRoot => mainHandCardRoot;

	public Transform ReadyCastLeftPoint { get; private set; }

	public Transform ReadyCastRightPoint { get; private set; }

	public Button StoringForceBtn => storingForceBtn;

	public Button NextRoundBtn => nextRoundBtn;

	public Transform PlayerHeadProtraitTrans => playerHeadProtraitImg.transform;

	public Transform PlayerHealthTransform => _healthCtrl.transform;

	public Transform PlayerApImgTrans => playerApImg.transform;

	public Transform PlayerSpecialAttrImgTrans => playerSpecialImg.transform;

	public Transform ArmorTransform => armorBg.transform;

	public PlayerDefenceAttrCtrl PlayerDefenceAttrCtrl { get; private set; }

	public Transform SkillDesPoint => skillDesPoint;

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		ForceRecycleAllCards();
		RecycleAllBattleSkillSlot();
		RecycleAllEquipEffectIcons();
		RecycleAllBuffIcon();
		EventManager.UnregisterEvent(EventEnum.E_UpdatePlayerHealth, UpdatePlayerHealth);
		EventManager.UnregisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
		storingForceHighlightImg.gameObject.SetActive(value: false);
		nextRoundHighlightImg.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Battle UI");
	}

	public override void OnSpawnUI()
	{
		_canvasGroup = base.transform.Find("Bg").GetComponent<CanvasGroup>();
		bubbleHintPoint = base.transform.Find("Bg/BubbleHintPoint");
		anim = GetComponent<UIAnim_BattleUI>();
		InitCardPanel();
		InitSettingPanel();
		InitButtonPanel();
		InitPlayerInfoPanel();
		InitSkillPanel();
		InitBuffPanel();
		InitEquipEffectPanel();
		InitKeyPanel();
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SetCardPanelBg((string)objs[0]);
		SetNextRoundBtnHighlight(isHighlight: false);
		SetNextRoundBtnInteractive(isInteractive: true);
		SetStoringForceBtnHighlight(isHighlight: false);
		SetStoringForceBtnInteractive(isInteractive: true);
		SetApSprite(isHighlight: false);
		SetSpecialAttrSprite(isHighlight: false);
		CardPanelOnShow();
		EventManager.RegisterEvent(EventEnum.E_UpdatePlayerHealth, UpdatePlayerHealth);
		EventManager.RegisterEvent(EventEnum.E_OnBattleEnd, OnBattleEnd);
	}

	public void BlockUIRaycast()
	{
		_canvasGroup.blocksRaycasts = false;
	}

	public void RecieveUIRaycast()
	{
		_canvasGroup.blocksRaycasts = true;
	}

	private void OnBattleEnd(EventData data)
	{
		RecycleAllBuffIcon();
	}

	private void InitCardPanel()
	{
		cardBgImg = base.transform.Find("Bg/CardBg").GetComponent<Image>();
		cardPanel = base.transform.Find("Bg/CardPanel");
		readyCastCardRoot = cardPanel.Find("ReadyCastRoot");
		mainHandCardRoot = cardPanel.Find("MainHandCards/CardRoot");
		supHandCardRoot = cardPanel.Find("SupHandCards/CardRoot");
		ReadyCastLeftPoint = cardPanel.Find("ReadyCastRoot/LeftPoint");
		ReadyCastRightPoint = cardPanel.Find("ReadyCastRoot/RightPoint");
		supDisPoint = cardPanel.Find("SupHandCards/SupDisPoint");
		supMovePoint = cardPanel.Find("SupHandCards/SupMovePoint");
		mainDisPoint = cardPanel.Find("MainHandCards/MainDisPoint");
		mainMovePoint = cardPanel.Find("MainHandCards/MainMovePoint");
		Transform transform = cardPanel.Find("MainPile");
		mainhandDiscardImg = transform.Find("MainHandDiscard").GetComponent<Image>();
		mainhandDiscardImg.GetComponent<Button>().onClick.AddListener(ShowMainHandCardHeap);
		mainhandCardPileImg = transform.Find("MainHand").GetComponent<Image>();
		mainhandCardPileImg.GetComponent<Button>().onClick.AddListener(ShowMainHandCardHeap);
		mainhandDiscardText = transform.Find("MainHandDiscard/MainDiscardAmount").GetComponent<Text>();
		mainhandCardPileText = transform.Find("MainHand/MainAmount").GetComponent<Text>();
		Transform transform2 = cardPanel.Find("SupPile");
		suphandDiscardImg = transform2.Find("SupHandDiscard").GetComponent<Image>();
		suphandDiscardImg.GetComponent<Button>().onClick.AddListener(ShowSupHandCardHeap);
		suphandCardPileImg = transform2.Find("SupHand").GetComponent<Image>();
		suphandCardPileImg.GetComponent<Button>().onClick.AddListener(ShowSupHandCardHeap);
		suphandDiscardText = transform2.Find("SupHandDiscard/SupDiscardAmount").GetComponent<Text>();
		suphandCardPileText = transform2.Find("SupHand/SupAmount").GetComponent<Text>();
	}

	private void SetCardPanelBg(string spriteName)
	{
		cardBgImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(spriteName, "Sprites/BattleBg");
	}

	private void CardPanelOnShow()
	{
		Player player = Singleton<GameManager>.Instance.Player;
		mainhandDiscardImg.gameObject.SetActive(value: false);
		suphandDiscardImg.gameObject.SetActive(value: false);
		mainhandCardPileText.text = player.PlayerBattleInfo.EquipedMainHandCardAmount.ToString();
		suphandCardPileText.text = player.PlayerBattleInfo.EquipedSupHandCardAmount.ToString();
	}

	private void ShowMainHandCardHeap()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("PlayerCardHeapUI") as PlayerCardHeapUI).ShowCardHeap(Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetMainHandRemainCardsWithAmount(), Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetMainHandDiscardCardsWithAmount(), isMain: true);
	}

	private void ShowSupHandCardHeap()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("PlayerCardHeapUI") as PlayerCardHeapUI).ShowCardHeap(Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetSupHandRemainCardsWithAmount(), Singleton<GameManager>.Instance.Player.PlayerBattleInfo.GetSupHandDiscardCardsWithAmount(), isMain: false);
	}

	public int GetMainSiblingIndex(UsualCardController cardController)
	{
		for (int i = 0; i < allShowingMainHandCards.Count; i++)
		{
			if (allShowingMainHandCards[i] == cardController)
			{
				return i;
			}
		}
		return 0;
	}

	public int GetSupSiblingIndex(UsualCardController cardController)
	{
		int num = allShowingSupHandCards.Count - 1;
		for (int i = 0; i < allShowingSupHandCards.Count; i++)
		{
			if (allShowingSupHandCards[i] == cardController)
			{
				return num - i;
			}
		}
		return 0;
	}

	public void AddSupHandCard(List<string> cardCodes, Action supcardDrawAnimOverAction)
	{
		this.supcardDrawAnimOverAction = supcardDrawAnimOverAction;
		for (int i = 0; i < cardCodes.Count; i++)
		{
			supCardsAddingQueue.Enqueue(cardCodes[i]);
		}
		if (!isAddingSupCard)
		{
			StartCoroutine(AddSupHandCard_IE());
		}
	}

	private IEnumerator AddSupHandCard_IE()
	{
		isAddingSupCard = true;
		while (supCardsAddingQueue.Count > 0)
		{
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_DrawSupHand_EffectConfig", null, null, null);
			string cardCode = supCardsAddingQueue.Dequeue();
			UsualCardController card = GetCard();
			card.LoadCard(this, cardCode, isMainHand: false);
			card.transform.SetParent(supHandCardRoot);
			card.transform.SetSiblingIndex(0);
			card.transform.position = suphandCardPileImg.transform.position;
			card.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			card.SetFixedSize(0.309f);
			ReAdjustSupHandCard(allShowingSupHandCards.Count, allShowingSupHandCards.Count + 1);
			card.LocalMoveCard(new Vector3(0f - GetCardAdjustPosX(allShowingSupHandCards.Count, allShowingSupHandCards.Count + 1), 0f, 0f), 0.3f);
			card.MoveRotateCard(Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 0f), 0.3f);
			card.TurnToMiniSize();
			allShowingSupHandCards.Add(card);
			UpdateSupHandCardPileAmount(supCardsAddingQueue.Count);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("发牌", UnityEngine.Random.Range(0.9f, 1.1f));
			yield return new WaitForSeconds(0.3f);
		}
		isAddingSupCard = false;
		supcardDrawAnimOverAction?.Invoke();
	}

	private void UpdateSupHandCardPileAmount(int queueAmount)
	{
		int num = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandCardsPileAmount + queueAmount;
		if (num == 0)
		{
			suphandCardPileImg.gameObject.SetActive(value: false);
			return;
		}
		if (!suphandCardPileImg.gameObject.activeSelf)
		{
			suphandCardPileImg.gameObject.SetActive(value: true);
		}
		suphandCardPileText.text = num.ToString();
	}

	private void ReAdjustSupHandCard(int amount, int maxAmount)
	{
		for (int i = 0; i < amount; i++)
		{
			if (!allShowingSupHandCards[i].IsThisCardCasting)
			{
				allShowingSupHandCards[i].LocalMoveCard(new Vector3(0f - GetCardAdjustPosX(i, maxAmount), 0f, 0f), 0.2f);
			}
		}
	}

	public void AddMainHandCard(List<string> cardCodes, Action maincardDrawAnimOverAction)
	{
		this.maincardDrawAnimOverAction = maincardDrawAnimOverAction;
		for (int i = 0; i < cardCodes.Count; i++)
		{
			mainCardsAddingQueue.Enqueue(cardCodes[i]);
		}
		if (!isAddingMainCard)
		{
			StartCoroutine(AddMainHandCard_IE());
		}
	}

	private IEnumerator AddMainHandCard_IE()
	{
		isAddingMainCard = true;
		while (mainCardsAddingQueue.Count > 0)
		{
			Singleton<BattleEffectManager>.Instance.HandleUsualEffectConfig("Usual_DrawMainHand_EffectConfig", null, null, null);
			string cardCode = mainCardsAddingQueue.Dequeue();
			UsualCardController card = GetCard();
			card.LoadCard(this, cardCode, isMainHand: true);
			card.transform.SetParent(mainHandCardRoot);
			card.transform.SetAsLastSibling();
			card.transform.position = mainhandCardPileImg.transform.position;
			card.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			card.SetFixedSize(0.309f);
			ReAdjustMainHandCard(allShowingMainHandCards.Count, allShowingMainHandCards.Count + 1);
			card.LocalMoveCard(new Vector3(GetCardAdjustPosX(allShowingMainHandCards.Count, allShowingMainHandCards.Count + 1), 0f, 0f), 0.3f);
			card.MoveRotateCard(Quaternion.Euler(0f, 0f, 90f), Quaternion.Euler(0f, 0f, 0f), 0.3f);
			card.TurnToMiniSize();
			allShowingMainHandCards.Add(card);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("发牌", UnityEngine.Random.Range(0.9f, 1.1f));
			EventManager.BroadcastEvent(EventEnum.E_OnPlayerDrawMainHandCard, new SimpleEventData
			{
				intValue = 1
			});
			UpdateMainHandCardPileAmount(mainCardsAddingQueue.Count);
			yield return new WaitForSeconds(0.3f);
		}
		isAddingMainCard = false;
		maincardDrawAnimOverAction?.Invoke();
	}

	private void UpdateMainHandCardPileAmount(int queueAmount)
	{
		int num = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandCardsPileAmount + queueAmount;
		if (num == 0)
		{
			mainhandCardPileImg.gameObject.SetActive(value: false);
			return;
		}
		if (!mainhandCardPileImg.gameObject.activeSelf)
		{
			mainhandCardPileImg.gameObject.SetActive(value: true);
		}
		mainhandCardPileText.text = num.ToString();
	}

	private void ReAdjustMainHandCard(int amount, int maxAmount)
	{
		for (int i = 0; i < amount; i++)
		{
			if (!allShowingMainHandCards[i].IsThisCardCasting)
			{
				allShowingMainHandCards[i].LocalMoveCard(new Vector3(GetCardAdjustPosX(i, maxAmount), 0f, 0f), 0.2f);
			}
		}
	}

	public void ShuffleMainHandCard(Action animOverAction)
	{
		isShuffleMainHandCard = true;
		mainhandCardPileImg.gameObject.SetActive(value: true);
		int discardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.MainHandDiscardPileAmount;
		DOTween.To(() => discardAmount, delegate(int x)
		{
			discardAmount = x;
		}, 0, 1f).OnUpdate(delegate
		{
			mainhandDiscardText.text = discardAmount.ToString();
		}).OnComplete(delegate
		{
			isShuffleMainHandCard = false;
			mainhandDiscardImg.gameObject.SetActive(value: false);
			animOverAction?.Invoke();
		});
		int cardAmount = 0;
		DOTween.To(() => cardAmount, delegate(int x)
		{
			cardAmount = x;
		}, discardAmount, 1f).OnUpdate(delegate
		{
			mainhandCardPileText.text = cardAmount.ToString();
		});
		anim.ShuffleCard_Main(discardAmount);
	}

	public void ShuffleSupHandCard(Action animOverAction)
	{
		isShuffleSupHandCard = true;
		suphandCardPileImg.gameObject.SetActive(value: true);
		int discardAmount = Singleton<GameManager>.Instance.Player.PlayerBattleInfo.SupHandDiscardPileAmount;
		DOTween.To(() => discardAmount, delegate(int x)
		{
			discardAmount = x;
		}, 0, 1f).OnUpdate(delegate
		{
			suphandDiscardText.text = discardAmount.ToString();
		}).OnComplete(delegate
		{
			isShuffleSupHandCard = false;
			suphandDiscardImg.gameObject.SetActive(value: false);
			animOverAction?.Invoke();
		});
		int cardAmount = 0;
		DOTween.To(() => cardAmount, delegate(int x)
		{
			cardAmount = x;
		}, discardAmount, 1f).OnUpdate(delegate
		{
			suphandCardPileText.text = cardAmount.ToString();
		});
		anim.ShuffleCard_Sup(discardAmount);
	}

	public static float GetCardAdjustPosX(int index, int maxAmount)
	{
		if ((float)(maxAmount - 1) * 140f <= 490f)
		{
			return (float)index * 140f;
		}
		float num = 490f / (float)(maxAmount - 1);
		return (float)index * num;
	}

	private UsualCardController GetCard()
	{
		if (allCardsObjPool.Count > 0)
		{
			UsualCardController usualCardController = allCardsObjPool.Dequeue();
			usualCardController.gameObject.SetActive(value: true);
			return usualCardController;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("UsualCard", "Prefabs", supHandCardRoot).GetComponent<UsualCardController>();
	}

	private void ForceRecycleAllCards()
	{
		if (allShowingMainHandCards.Count > 0)
		{
			for (int i = 0; i < allShowingMainHandCards.Count; i++)
			{
				UsualCardController usualCardController = allShowingMainHandCards[i];
				usualCardController.RecycleCard(isNeedAnim: false, isDrop: true, null);
				allCardsObjPool.Enqueue(usualCardController);
				usualCardController.transform.SetParent(readyCastCardRoot);
			}
			allShowingMainHandCards.Clear();
		}
		if (allShowingSupHandCards.Count > 0)
		{
			for (int j = 0; j < allShowingSupHandCards.Count; j++)
			{
				UsualCardController usualCardController2 = allShowingSupHandCards[j];
				usualCardController2.RecycleCard(isNeedAnim: false, isDrop: true, null);
				allCardsObjPool.Enqueue(usualCardController2);
				usualCardController2.transform.SetParent(readyCastCardRoot);
			}
			allShowingSupHandCards.Clear();
		}
	}

	private void RemoveMainHandCard(UsualCardController card, bool isDrop, bool isRemoveFromShowing = true)
	{
		if (isRemoveFromShowing)
		{
			allShowingMainHandCards.Remove(card);
		}
		ReAdjustMainHandCard(allShowingMainHandCards.Count, allShowingMainHandCards.Count);
		RecycleHandCardController(card, isMain: true, isDrop);
	}

	public void RemoveMainHandCard(string cardCode, int amount, bool isDrop)
	{
		int num = 0;
		for (int i = 0; i < allShowingMainHandCards.Count; i++)
		{
			if (allShowingMainHandCards[i].CurrentCard.CardCode == cardCode)
			{
				RecycleHandCardController(allShowingMainHandCards[i], isMain: true, isDrop, i);
				i--;
				num++;
				if (num == amount)
				{
					break;
				}
			}
		}
		ReAdjustMainHandCard(allShowingMainHandCards.Count, allShowingMainHandCards.Count);
	}

	private void RemoveSupHandCard(UsualCardController card, bool isDrop, bool isRemoveFromShowing = true)
	{
		if (isRemoveFromShowing)
		{
			allShowingSupHandCards.Remove(card);
		}
		ReAdjustSupHandCard(allShowingSupHandCards.Count, allShowingSupHandCards.Count);
		RecycleHandCardController(card, isMain: false, isDrop);
	}

	public void RemoveSupHandCard(string cardCode, int amount, bool isDrop)
	{
		int num = 0;
		for (int i = 0; i < allShowingSupHandCards.Count; i++)
		{
			if (allShowingSupHandCards[i].CurrentCard.CardCode == cardCode)
			{
				RecycleHandCardController(allShowingSupHandCards[i], isMain: false, isDrop, i);
				i--;
				num++;
				if (num == amount)
				{
					break;
				}
			}
		}
		ReAdjustSupHandCard(allShowingSupHandCards.Count, allShowingSupHandCards.Count);
	}

	public void PlayerChooseACardReadyCast(UsualCardController cardController)
	{
		if (!(cardController == cardPlayerReadyCast))
		{
			cardPlayerReadyCast = cardController;
			cardController.transform.SetParent(readyCastCardRoot);
			cardController.OnReadyCastCard();
		}
	}

	public void PlayerUseCard(UsualCardController cardController, Action callback)
	{
		ConfirmCastingUsualCard(cardController, callback);
	}

	private void ConfirmCastingUsualCard(UsualCardController cardController, Action callback)
	{
		if (cardController.IsMainHandCard)
		{
			allShowingMainHandCards.Remove(cardController);
		}
		else
		{
			allShowingSupHandCards.Remove(cardController);
		}
		Singleton<GameManager>.Instance.Player.PlayerUseAUsualCard(cardController.CurrentCard, cardController.IsMainHandCard, delegate
		{
			callback?.Invoke();
			OnEndUsualCardCast(cardController);
		});
	}

	private void OnEndUsualCardCast(UsualCardController cardController)
	{
		if (cardController.IsMainHandCard)
		{
			RemoveMainHandCard(cardController, isDrop: false);
		}
		else
		{
			RemoveSupHandCard(cardController, isDrop: false);
		}
		cardPlayerReadyCast = null;
	}

	public void PlayerDropACard(string cardCode, bool isMain)
	{
		if (isMain)
		{
			for (int i = 0; i < allShowingMainHandCards.Count; i++)
			{
				if (allShowingMainHandCards[i].CurrentCard.CardCode == cardCode)
				{
					RemoveMainHandCard(allShowingMainHandCards[i], isDrop: true);
					break;
				}
			}
			return;
		}
		for (int j = 0; j < allShowingSupHandCards.Count; j++)
		{
			if (allShowingSupHandCards[j].CurrentCard.CardCode == cardCode)
			{
				RemoveSupHandCard(allShowingSupHandCards[j], isDrop: true);
				break;
			}
		}
	}

	public void PlayerCancelCardCast(UsualCardController cardController)
	{
		cardPlayerReadyCast = null;
		cardController.transform.SetParent(allShowingMainHandCards.Contains(cardController) ? mainHandCardRoot : supHandCardRoot);
		cardController.OnCancelCastCard();
	}

	public void RecycleAllMainHandCardControllers(List<string> cardsKept, HashSet<string> cardKeptType)
	{
		RecycleAllMainHandCardControllers(GetCardsKeptsUsualCardControllerListFromShowingMainCards(cardsKept), cardKeptType);
		ReAdjustMainHandCard(allShowingMainHandCards.Count, allShowingMainHandCards.Count);
	}

	private void RecycleAllMainHandCardControllers(List<UsualCardController> keepCards, HashSet<string> cardKeptType)
	{
		if ((keepCards == null || keepCards.Count <= 0) && (cardKeptType == null || cardKeptType.Count <= 0))
		{
			RecycleAllMainHandCardControllers();
			return;
		}
		for (int i = 0; i < allShowingMainHandCards.Count; i++)
		{
			if ((cardKeptType == null || !cardKeptType.Contains(allShowingMainHandCards[i].CurrentCard.CardCode)) && (keepCards == null || !keepCards.Contains(allShowingMainHandCards[i])))
			{
				RecycleHandCardController(allShowingMainHandCards[i], isMain: true, isDrop: true, i);
				i--;
			}
		}
	}

	private void RecycleAllMainHandCardControllers()
	{
		if (allShowingMainHandCards.Count > 0)
		{
			for (int i = 0; i < allShowingMainHandCards.Count; i++)
			{
				RecycleHandCardController(allShowingMainHandCards[i], isMain: true, isDrop: true);
			}
			allShowingMainHandCards.Clear();
		}
	}

	public void RecycleAllSupHandCardControllers(List<string> cardsKept, HashSet<string> cardKeptType)
	{
		RecycleAllSupHandCardControllers(GetCardsKeptsUsualCardControllerListFromShowingSupCards(cardsKept), cardKeptType);
		ReAdjustSupHandCard(allShowingSupHandCards.Count, allShowingSupHandCards.Count);
	}

	private void RecycleAllSupHandCardControllers(List<UsualCardController> keepCards, HashSet<string> cardKeptType)
	{
		if ((keepCards == null || keepCards.Count <= 0) && (cardKeptType == null || cardKeptType.Count <= 0))
		{
			RecycleAllSupHandCardControllers();
			return;
		}
		for (int i = 0; i < allShowingSupHandCards.Count; i++)
		{
			if ((cardKeptType == null || !cardKeptType.Contains(allShowingSupHandCards[i].CurrentCard.CardCode)) && (keepCards == null || !keepCards.Contains(allShowingSupHandCards[i])))
			{
				RecycleHandCardController(allShowingSupHandCards[i], isMain: false, isDrop: true, i);
				i--;
			}
		}
	}

	private void RecycleAllSupHandCardControllers()
	{
		if (allShowingSupHandCards.Count > 0)
		{
			for (int i = 0; i < allShowingSupHandCards.Count; i++)
			{
				RecycleHandCardController(allShowingSupHandCards[i], isMain: false, isDrop: true);
			}
			allShowingSupHandCards.Clear();
		}
	}

	private List<UsualCardController> GetCardsKeptsUsualCardControllerListFromShowingMainCards(List<string> cardsKept)
	{
		List<UsualCardController> list = new List<UsualCardController>();
		for (int i = 0; i < allShowingMainHandCards.Count; i++)
		{
			if (cardsKept != null && cardsKept.Contains(allShowingMainHandCards[i].CurrentCard.CardCode))
			{
				cardsKept.Remove(allShowingMainHandCards[i].CurrentCard.CardCode);
				list.Add(allShowingMainHandCards[i]);
			}
		}
		return list;
	}

	private List<UsualCardController> GetCardsKeptsUsualCardControllerListFromShowingSupCards(List<string> cardsKept)
	{
		List<UsualCardController> list = new List<UsualCardController>();
		for (int i = 0; i < allShowingSupHandCards.Count; i++)
		{
			if (cardsKept != null && cardsKept.Contains(allShowingSupHandCards[i].CurrentCard.CardCode))
			{
				cardsKept.Remove(allShowingSupHandCards[i].CurrentCard.CardCode);
				list.Add(allShowingSupHandCards[i]);
			}
		}
		return list;
	}

	private void RecycleHandCardController(UsualCardController cardController, bool isMain, bool isDrop, int index = -1)
	{
		if (isMain)
		{
			RecycleMainHandCardController(cardController, isDrop, index);
		}
		else
		{
			RecycleSupHandCardController(cardController, isDrop, index);
		}
	}

	private void RecycleMainHandCardController(UsualCardController cardController, bool isDrop, int index = -1)
	{
		RecycleACard(cardController, isDrop);
		if (index >= 0)
		{
			allShowingMainHandCards.RemoveAt(index);
		}
		UpdateMainHandDiscardAmount();
	}

	private void UpdateMainHandDiscardAmount()
	{
		PlayerBattleInfo playerBattleInfo = Singleton<GameManager>.Instance.Player.PlayerBattleInfo;
		if (!mainhandDiscardImg.gameObject.activeSelf && playerBattleInfo.MainHandDiscardPileAmount > 0)
		{
			mainhandDiscardImg.gameObject.SetActive(value: true);
		}
		mainhandDiscardText.text = ((playerBattleInfo.MainHandDiscardPileAmount > 0) ? playerBattleInfo.MainHandDiscardPileAmount.ToString() : string.Empty);
	}

	private void RecycleSupHandCardController(UsualCardController cardController, bool isDrop, int index = -1)
	{
		RecycleACard(cardController, isDrop);
		if (index >= 0)
		{
			allShowingSupHandCards.RemoveAt(index);
		}
		UpdateSupHandDiscardAmount();
	}

	private void UpdateSupHandDiscardAmount()
	{
		if (!suphandDiscardImg.gameObject.activeSelf)
		{
			suphandDiscardImg.gameObject.SetActive(value: true);
		}
		PlayerBattleInfo playerBattleInfo = Singleton<GameManager>.Instance.Player.PlayerBattleInfo;
		suphandDiscardText.text = playerBattleInfo.SupHandDiscardPileAmount.ToString();
	}

	private void RecycleACard(UsualCardController cardController, bool isDrop)
	{
		cardController.transform.rotation = Quaternion.identity;
		cardController.RecycleCard(isNeedAnim: true, isDrop, delegate
		{
			cardController.transform.SetParent(readyCastCardRoot);
			allCardsObjPool.Enqueue(cardController);
		});
	}

	private void InitSettingPanel()
	{
		systemPanel = base.transform.Find("Bg/SystemPanel");
		systemPanel.Find("SettingBg/SettingBtn").GetComponent<Button>().onClick.AddListener(OnClickSettingBtn);
		systemPanel.Find("PlayerInfoDetailBtnBg/PlayerInfoDetailBtn").GetComponent<Button>().onClick.AddListener(OnClickPlayerDetailInfoBtn);
		playerInfoDetailBtnBg = systemPanel.Find("PlayerInfoDetailBtnBg");
	}

	public void SetPlayerInfoDetailBtnBgActive(bool isActive)
	{
		playerInfoDetailBtnBg.gameObject.SetActive(isActive);
	}

	private void OnClickSettingBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.ShowView("SettingUI", false);
	}

	private void OnClickPlayerDetailInfoBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("PlayerDetailInfoUI");
	}

	private void InitButtonPanel()
	{
		buttonPanel = base.transform.Find("Bg/ButtonPanel");
		nextRoundBtn = buttonPanel.Find("NextRoundBtn").GetComponent<Button>();
		nextRoundBtn.onClick.AddListener(OnClickNextRoundBtn);
		storingForceBtn = buttonPanel.Find("StoringForceBtn").GetComponent<Button>();
		storingForceBtn.onClick.AddListener(OnClickStoringForceBtn);
		storingForceHighlightImg = buttonPanel.Find("StoringForceBtn/HighlightImg").GetComponent<Image>();
		nextRoundHighlightImg = buttonPanel.Find("NextRoundBtn/HighlightImg").GetComponent<Image>();
	}

	public void LockNextRoundBtn()
	{
		nextRoundBtn.gameObject.SetActive(value: false);
	}

	public void UnlockNextRoundBtn()
	{
		nextRoundBtn.gameObject.SetActive(value: true);
	}

	public void LockSotringForceBtn()
	{
		storingForceBtn.gameObject.SetActive(value: false);
	}

	public void UnlockStoringForceBtn()
	{
		storingForceBtn.gameObject.SetActive(value: true);
	}

	private void OnClickNextRoundBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("蓄势_结束回合按钮");
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRound == Round.PlayerRound)
		{
			Singleton<GameManager>.Instance.BattleSystem.EndPlayerRound();
			SetNextRoundBtnInteractive(isInteractive: false);
			SetNextRoundBtnHighlight(isHighlight: false);
			SetStoringForceBtnHighlight(isHighlight: false);
			SetStoringForceBtnInteractive(isInteractive: false);
		}
	}

	private void OnClickStoringForceBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("蓄势_结束回合按钮");
		if (Singleton<GameManager>.Instance.BattleSystem.BattleRound == Round.PlayerRound)
		{
			Singleton<GameManager>.Instance.Player.StoringForce();
		}
	}

	public void SetStoringForceBtnInteractive(bool isInteractive)
	{
		storingForceBtn.interactable = isInteractive;
	}

	public void SetStoringForceBtnHighlight(bool isHighlight)
	{
		storingForceHighlightImg.gameObject.SetActive(isHighlight);
	}

	public void SetNextRoundBtnInteractive(bool isInteractive)
	{
		nextRoundBtn.interactable = isInteractive;
	}

	public void SetNextRoundBtnHighlight(bool isHighlight)
	{
		nextRoundHighlightImg.gameObject.SetActive(isHighlight);
	}

	private void InitPlayerInfoPanel()
	{
		playerInfoPanel = base.transform.Find("Bg/PlayerInfo");
		playerHeadProtraitImg = playerInfoPanel.Find("HeadPortraitBg/HeadPortrait").GetComponent<Image>();
		_healthCtrl = playerInfoPanel.Find("HealthBar").GetComponent<UsualHealthBarCtrl>();
		atkDmgText = playerInfoPanel.Find("WeaponDmg").GetComponent<Text>();
		atkDmgImg = playerInfoPanel.Find("WeaponDmg/Icon").GetComponent<Image>();
		playerApImg = playerInfoPanel.Find("ApRemainImg").GetComponent<Image>();
		playerSpecialImg = playerInfoPanel.Find("FaithRemainImg").GetComponent<Image>();
		playerSpecialAttrHint = playerSpecialImg.GetComponent<MessageHint>();
		playerApAmountRemain = playerInfoPanel.Find("ApRemainImg/Amount").GetComponent<Text>();
		playerSpecialAttrRemain = playerInfoPanel.Find("FaithRemainImg/Amount").GetComponent<Text>();
		armorBg = playerInfoPanel.Find("HealthBar/ArmorBg").GetComponent<Image>();
		armorAmountText = playerInfoPanel.Find("HealthBar/ArmorBg/Amount").GetComponent<Text>();
		playerSelfHighlightImg = playerInfoPanel.Find("SelfHighlight").GetComponent<Image>();
		defenceCtrlRoot = playerInfoPanel.Find("DefenceRoot");
	}

	public void LoadPlayerInfo()
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		SetPlayerHeadportrait(occupationData);
		PlayerAttr playerAttr = Singleton<GameManager>.Instance.Player.PlayerAttr;
		_healthCtrl.LoadHealth(playerAttr.Health, playerAttr.MaxHealth);
		UpdatePlayerArmor(playerAttr.Armor);
		UpdatePlayerApAmount();
		SetPlayerAtkDmg(playerAttr.AtkDmg, 0);
		SetPlayerAtkDmgIcon(occupationData);
		UpdatePlayerSpecialAttr(playerAttr.SpecialAttrShowStr());
		SetPlayerSpecialAttrImgAndHint(occupationData);
		if (PlayerDefenceAttrCtrl != null && PlayerDefenceAttrCtrl.PlayerOccupation != Singleton<GameManager>.Instance.Player.PlayerOccupation)
		{
			UnityEngine.Object.Destroy(PlayerDefenceAttrCtrl.gameObject);
			PlayerDefenceAttrCtrl = null;
		}
		if (PlayerDefenceAttrCtrl == null)
		{
			PlayerDefenceAttrCtrl = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(occupationData.BattleUIDefenceAttrCtrlPrefab, occupationData.DefaultPrefabPath, defenceCtrlRoot).GetComponent<PlayerDefenceAttrCtrl>();
		}
		PlayerDefenceAttrCtrl.LoadPlayerInfo();
	}

	private void SetPlayerSpecialAttrImgAndHint(OccupationData data)
	{
		playerSpecialImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.BattleUISpecialAttrSprite, data.DefaultSpritePath);
		playerSpecialAttrHint.Key = data.OccupationSpecialAttrDes.key;
		playerSpecialAttrHint.extraKey = data.OccupationSpecialAttrDes.value;
	}

	private void SetPlayerAtkDmgIcon(OccupationData data)
	{
		atkDmgImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.AtkIconSpriteName, data.DefaultSpritePath);
	}

	public void SetPlayerAtkDmg(int baseDmg, int extraDmg)
	{
		if (extraDmg > 0)
		{
			atkDmgText.text = baseDmg + "+<size=20>" + extraDmg + "</size>";
		}
		else
		{
			atkDmgText.text = baseDmg.ToString();
		}
	}

	public void LockPlayerFaithForGuideSystem()
	{
		Singleton<GameManager>.Instance.Player.PlayerAttr.IsLockingSpecialAttr = true;
		playerSpecialImg.gameObject.SetActive(value: false);
	}

	public void UnlockPlayerFaithForGuideSystem()
	{
		playerSpecialImg.gameObject.SetActive(value: true);
	}

	public void SetPlayerSpecialPossHeadPortrait()
	{
		playerHeadProtraitImg.material.DOFloat(1f, "_AToDPara", 0.5f);
	}

	public void SetPlayerNormalHeapPortrait()
	{
		playerHeadProtraitImg.material.DOFloat(0f, "_AToDPara", 0.5f);
	}

	private void SetPlayerHeadportrait(OccupationData data)
	{
		playerHeadProtraitImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.BattleUIHeadportraitSpriteName, data.DefaultSpritePath);
		playerHeadProtraitImg.material = SingletonDontDestroy<ResourceManager>.Instance.LoadMaterial(data.BattleUIHeadportraitMat, data.DefaultMaterialPath);
	}

	public void HighlightSelfCast()
	{
		playerSelfHighlightImg.gameObject.SetActive(value: true);
	}

	public void CancelHighlighSelfCast()
	{
		playerSelfHighlightImg.gameObject.SetActive(value: false);
	}

	private void UpdatePlayerHealth(EventData eventData)
	{
		UpdatePlayerHealth(Singleton<GameManager>.Instance.Player.PlayerAttr.Health, Singleton<GameManager>.Instance.Player.PlayerAttr.MaxHealth);
	}

	private void UpdatePlayerHealth(int health, int maxHealth)
	{
		_healthCtrl.UpdateHealth(health, maxHealth);
	}

	public void UpdatePlayerArmor(int armor)
	{
		if (!playerArmorHintTween.IsNull() && playerArmorHintTween.IsActive())
		{
			playerArmorHintTween.Complete();
		}
		playerArmorHintTween = armorAmountText.transform.TransformHint();
		armorBg.enabled = armor > 0;
		armorAmountText.text = ((armor > 0) ? armor.ToString() : string.Empty);
	}

	public void UpdatePlayerApAmount()
	{
		if (!playerApAmountHintTween.IsNull() && playerApAmountHintTween.IsActive())
		{
			playerApAmountHintTween.Complete();
		}
		playerApAmountHintTween = playerApAmountRemain.transform.TransformHint();
		playerApAmountRemain.text = $"× {((Singleton<GameManager>.Instance.Player.PlayerAttr.ApAmount <= 99) ? Singleton<GameManager>.Instance.Player.PlayerAttr.ApAmount : 99)}";
	}

	public void UpdatePlayerSpecialAttr(string specialAttrDes)
	{
		if (!playerSpecialAttrHintTween.IsNull() && playerSpecialAttrHintTween.IsActive())
		{
			playerSpecialAttrHintTween.Complete();
		}
		playerSpecialAttrHintTween = playerSpecialAttrRemain.transform.TransformHint();
		playerSpecialAttrRemain.text = specialAttrDes;
	}

	public void SetApSprite(bool isHighlight)
	{
		playerApImg.sprite = (isHighlight ? apSpriteHighlight : apLackSprite);
	}

	public void SetSpecialAttrSprite(bool isHighlight)
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		playerSpecialImg.sprite = (isHighlight ? SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.BattleUISpecialAttrSprite, occupationData.DefaultSpritePath) : SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.BattleUISpecialAttrLackSprite, occupationData.DefaultSpritePath));
	}

	private void InitSkillPanel()
	{
		skillRoot = base.transform.Find("Bg/SkillPanel");
		skillDesPoint = base.transform.Find("Bg/SkillDesPoint");
	}

	public void LoadSkill(List<string> playerEquipedSkill)
	{
		for (int i = 0; i < playerEquipedSkill.Count; i++)
		{
			if (!playerEquipedSkill[i].IsNullOrEmpty())
			{
				BattleSkillSlotCtrl battleSkillSlot = GetBattleSkillSlot();
				battleSkillSlot.LoadSkill(playerEquipedSkill[i], this);
				battleSkillSlot.transform.SetSiblingIndex(i);
				battleSkillSlot.transform.SetSiblingIndex(allShowingBattleSkill.Count);
				allShowingBattleSkill.Add(playerEquipedSkill[i], battleSkillSlot);
			}
		}
	}

	public void LockSkills()
	{
		BattleSkillSlotCtrl.isLocked = true;
	}

	public void UnlockSkills()
	{
		BattleSkillSlotCtrl.isLocked = false;
		foreach (KeyValuePair<string, BattleSkillSlotCtrl> item in allShowingBattleSkill)
		{
			item.Value.UnlockSkill(isNeedAnim: true);
		}
	}

	private BattleSkillSlotCtrl GetBattleSkillSlot()
	{
		if (battleSkillSlotPool.Count > 0)
		{
			BattleSkillSlotCtrl battleSkillSlotCtrl = battleSkillSlotPool.Dequeue();
			battleSkillSlotCtrl.gameObject.SetActive(value: true);
			return battleSkillSlotCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BattleSkillSlot", "Prefabs", skillRoot).GetComponent<BattleSkillSlotCtrl>();
	}

	public void RecycleAllBattleSkillSlot()
	{
		foreach (KeyValuePair<string, BattleSkillSlotCtrl> item in allShowingBattleSkill)
		{
			item.Value.RecycleSkillSlot();
			item.Value.gameObject.SetActive(value: false);
			battleSkillSlotPool.Enqueue(item.Value);
		}
		allShowingBattleSkill.Clear();
	}

	public void HighlightSkillCards(string skillCode)
	{
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode);
		for (int i = 0; i < allShowingMainHandCards.Count; i++)
		{
			if (allShowingMainHandCards[i].CurrentCard.CardCode == skillCardAttr.MainHandCardCode)
			{
				allShowingMainHandCards[i].SetSkillHighLight();
				mainHighlightCards.Add(allShowingMainHandCards[i]);
				if (mainHighlightCards.Count == skillCardAttr.MainHandCardConsumeAmount)
				{
					break;
				}
			}
		}
		for (int j = 0; j < allShowingSupHandCards.Count; j++)
		{
			if (allShowingSupHandCards[j].CurrentCard.CardCode == skillCardAttr.SupHandCardCode)
			{
				allShowingSupHandCards[j].SetSkillHighLight();
				supHighlightCards.Add(allShowingSupHandCards[j]);
				if (supHighlightCards.Count == skillCardAttr.SupHandCardConsumeAmount)
				{
					break;
				}
			}
		}
	}

	public void EndSkillDescription()
	{
		if (mainHighlightCards.Count > 0)
		{
			for (int i = 0; i < mainHighlightCards.Count; i++)
			{
				mainHighlightCards[i].SetSkillNormal();
			}
			mainHighlightCards.Clear();
		}
		if (supHighlightCards.Count > 0)
		{
			for (int j = 0; j < supHighlightCards.Count; j++)
			{
				supHighlightCards[j].SetSkillNormal();
			}
			supHighlightCards.Clear();
		}
	}

	private void InitBuffPanel()
	{
		buffPanel = base.transform.Find("Bg/BuffPanel");
	}

	public void AddBuff(BaseBuff buff)
	{
		if (!allShowingBuffIcons.ContainsKey(buff.BuffType))
		{
			BuffIconCtrl buffIcon = GetBuffIcon();
			buffIcon.transform.DOComplete();
			buffIcon.LoadBuff(buff, isScreen: true);
			buffIcon.transform.SetAsLastSibling();
			allShowingBuffIcons.Add(buff.BuffType, buffIcon);
		}
	}

	public BuffIconCtrl GetBuffIconCtrl(BuffType buffType)
	{
		if (!allShowingBuffIcons.TryGetValue(buffType, out var value))
		{
			return null;
		}
		return value;
	}

	public void RemoveBuff(BaseBuff buff)
	{
		RemoveBuff(buff.BuffType);
	}

	public void RemoveBuff(BuffType buffType)
	{
		if (allShowingBuffIcons.TryGetValue(buffType, out var value))
		{
			value.gameObject.SetActive(value: false);
			allShowingBuffIcons.Remove(buffType);
			allBuffIconPool.Enqueue(value);
		}
	}

	public void UpdateBuff(BaseBuff buff)
	{
		UpdateBuff(buff.BuffType);
	}

	public void UpdateBuff(BuffType buffType)
	{
		if (allShowingBuffIcons.TryGetValue(buffType, out var value))
		{
			value.UpdateBuff();
		}
	}

	private BuffIconCtrl GetBuffIcon()
	{
		if (allBuffIconPool.Count > 0)
		{
			BuffIconCtrl buffIconCtrl = allBuffIconPool.Dequeue();
			buffIconCtrl.gameObject.SetActive(value: true);
			return buffIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BuffIcon", "Prefabs", buffPanel).GetComponent<BuffIconCtrl>();
	}

	private void RecycleAllBuffIcon()
	{
		if (allShowingBuffIcons.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<BuffType, BuffIconCtrl> allShowingBuffIcon in allShowingBuffIcons)
		{
			allShowingBuffIcon.Value.gameObject.SetActive(value: false);
			allBuffIconPool.Enqueue(allShowingBuffIcon.Value);
		}
		allShowingBuffIcons.Clear();
	}

	private void InitEquipEffectPanel()
	{
		equipEffectPanel = base.transform.Find("Bg/EquipEffectPanel");
	}

	public EquipEffectIconCtrl AddEquipEffect(EquipmentCard equip, string initContent, Func<string> func)
	{
		EquipEffectIconCtrl equipEffectIcon = GetEquipEffectIcon();
		equipEffectIcon.LoadEquip(equip, initContent, func);
		allShowingEquipEffectIcon.Add(equipEffectIcon);
		return equipEffectIcon;
	}

	public EquipEffectIconCtrl AddSuitEffet(SuitType suitType, string hintContent, Func<string> func)
	{
		EquipEffectIconCtrl equipEffectIcon = GetEquipEffectIcon();
		equipEffectIcon.LoadSuit(suitType, hintContent, func);
		allShowingEquipEffectIcon.Add(equipEffectIcon);
		return equipEffectIcon;
	}

	private EquipEffectIconCtrl GetEquipEffectIcon()
	{
		if (allEquipEffectIconPools.Count > 0)
		{
			EquipEffectIconCtrl equipEffectIconCtrl = allEquipEffectIconPools.Dequeue();
			equipEffectIconCtrl.gameObject.SetActive(value: true);
			return equipEffectIconCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EquipEffectIcon", "Prefabs", equipEffectPanel).GetComponent<EquipEffectIconCtrl>();
	}

	private void RecycleAllEquipEffectIcons()
	{
		if (allShowingEquipEffectIcon.Count > 0)
		{
			for (int i = 0; i < allShowingEquipEffectIcon.Count; i++)
			{
				allShowingEquipEffectIcon[i].gameObject.SetActive(value: false);
				allEquipEffectIconPools.Enqueue(allShowingEquipEffectIcon[i]);
			}
			allShowingEquipEffectIcon.Clear();
		}
	}

	private void InitKeyPanel()
	{
		keyPanelLeftRoot = base.transform.Find("Bg/LeftKeyPanel");
		keyPanelLeftRectTrans = keyPanelLeftRoot.GetComponent<RectTransform>();
		keyPanelRightRoot = base.transform.Find("Bg/RightKeyPanel");
		keyPanelRightRectTrans = keyPanelRightRoot.GetComponent<RectTransform>();
	}

	public void AddKeyDescription(List<KeyValuePair> allKeys, KeyDesType type, Transform target)
	{
		if (allKeys.IsNull())
		{
			return;
		}
		Transform transform = null;
		RectTransform layoutRoot = null;
		switch (type)
		{
		case KeyDesType.Left:
			transform = keyPanelLeftRoot;
			layoutRoot = keyPanelLeftRectTrans;
			break;
		case KeyDesType.Right:
			transform = keyPanelRightRoot;
			layoutRoot = keyPanelRightRectTrans;
			break;
		}
		foreach (KeyValuePair allKey in allKeys)
		{
			KeyCtrl keyCtrl = GetKeyCtrl(transform);
			keyCtrl.LoadKey(allKey.key.LocalizeText(), allKey.value.LocalizeText());
			allShowingKeyCtrls.Enqueue(keyCtrl);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
		keyCor = StartCoroutine(Key_IE(transform, target));
	}

	private IEnumerator Key_IE(Transform keyRoot, Transform target)
	{
		while (true)
		{
			keyRoot.position = target.position;
			yield return null;
		}
	}

	public void AddKeyDescription(string _name, string _des, RectTransform root, bool isScreen)
	{
		KeyCtrl keyCtrl = GetKeyCtrl(base.transform);
		keyCtrl.LoadKey(_name, _des);
		Vector2 vector = new Vector2(keyCtrl.HalfSizeX + root.sizeDelta.x, keyCtrl.HalfSizeY);
		if (isScreen)
		{
			keyCtrl.transform.position = root.transform.position + (Vector3)vector;
		}
		else
		{
			keyCtrl.transform.position = SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToScreenPoint(root.transform.position) + (Vector3)vector;
		}
		allShowingKeyCtrls.Enqueue(keyCtrl);
	}

	private KeyCtrl GetKeyCtrl(Transform root)
	{
		if (allKeyCtrlPool.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyCtrlPool.Dequeue();
			keyCtrl.transform.SetParent(root);
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", root).GetComponent<KeyCtrl>();
	}

	public void HideAllKeyCtrl()
	{
		while (allShowingKeyCtrls.Count > 0)
		{
			KeyCtrl keyCtrl = allShowingKeyCtrls.Dequeue();
			keyCtrl.gameObject.SetActive(value: false);
			allKeyCtrlPool.Enqueue(keyCtrl);
		}
		if (keyCor != null)
		{
			StopCoroutine(keyCor);
			keyCor = null;
		}
	}
}
