using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinglePurchasedCardCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private UsualNoDesCardInfo _cardInfo;

	private Image maskImg;

	private Text costText;

	private string currentCardCode;

	private bool isPurchased;

	private int price;

	private Image lockIconImg;

	private bool isRegisterEvent;

	private PurchasedItemUI rootPanel;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		_cardInfo = base.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
		maskImg = base.transform.Find("CardMask").GetComponent<Image>();
		costText = base.transform.Find("CardMask/CostText").GetComponent<Text>();
		lockIconImg = base.transform.Find("CardMask/LockIcon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		UnregisterEvent();
	}

	public void LoadCard(PurchasedItemUI purchasedItemUi, ItemPurchasedData data, bool isPurchased)
	{
		rootPanel = purchasedItemUi;
		currentCardCode = data.ItemCode;
		this.isPurchased = isPurchased;
		_cardInfo.LoadCard(data.ItemCode);
		if (isPurchased)
		{
			maskImg.gameObject.SetActive(value: false);
			return;
		}
		price = data.Price;
		maskImg.gameObject.SetActive(value: true);
		costText.text = data.Price.ToString();
		CheckAndSetLockStat();
		RegisterEvent();
	}

	private void RegisterEvent()
	{
		if (!isRegisterEvent)
		{
			isRegisterEvent = true;
			EventManager.RegisterEvent(EventEnum.E_DarkCrystalChanged, OnDarkCrystalChanged);
		}
	}

	private void UnregisterEvent()
	{
		if (isRegisterEvent)
		{
			isRegisterEvent = false;
			EventManager.RegisterEvent(EventEnum.E_DarkCrystalChanged, OnDarkCrystalChanged);
		}
	}

	private void CheckAndSetLockStat()
	{
		lockIconImg.gameObject.SetActive(value: true);
		lockIconImg.sprite = ((SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin >= price) ? rootPanel.lockMask_CanPurchased : rootPanel.lockMask_CannotPurchased);
	}

	private void OnDarkCrystalChanged(EventData data)
	{
		if (!isPurchased)
		{
			CheckAndSetLockStat();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, currentCardCode, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!isPurchased)
		{
			bool isInteractive = SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin >= price;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI).ShowBigCard(currentCardCode, "purchase".LocalizeText(), OnClickConfirmPurchase, isInteractive);
		}
	}

	private void OnClickConfirmPurchase()
	{
		rootPanel.ShowPurchasedVfx(lockIconImg.transform, 1f);
		lockIconImg.gameObject.SetActive(value: false);
		UnregisterEvent();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.ComsumeCoin(price, isAutoSave: false);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.PurchaseSpecialUsualCard(currentCardCode, isAutoSave: false);
		GameSave.SaveUserData();
		isPurchased = true;
		maskImg.gameObject.SetActive(value: false);
		SingletonDontDestroy<UIManager>.Instance.HideView("BagCardDesUI");
	}
}
