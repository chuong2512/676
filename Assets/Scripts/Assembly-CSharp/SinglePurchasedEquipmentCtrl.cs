using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinglePurchasedEquipmentCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private Image iconImg;

	private Text nameText;

	private Image maskImg;

	private Text costText;

	private bool isPurchased;

	private string currentShowingEquipCode;

	private int price;

	private Image occuIcon;

	private Image lockIconImg;

	private bool isRegisterEvent;

	private PurchasedItemUI rootPanel;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		maskImg = base.transform.Find("Mask").GetComponent<Image>();
		costText = base.transform.Find("Mask/CostText").GetComponent<Text>();
		occuIcon = base.transform.Find("PlayerOccuIcon").GetComponent<Image>();
		lockIconImg = base.transform.Find("Mask/LockIcon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		UnregisterEvent();
	}

	public void LoadEquipment(string equipCode, EquipmentPurchasedPanel parentPanel, EquipmentType equipmentType, bool isPurchased)
	{
		rootPanel = parentPanel.ParentPanel;
		this.isPurchased = isPurchased;
		currentShowingEquipCode = equipCode;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
		nameText.text = equipmentCardAttr.NameKey.LocalizeText();
		if (isPurchased)
		{
			maskImg.gameObject.SetActive(value: false);
		}
		else
		{
			maskImg.gameObject.SetActive(value: true);
			ItemPurchasedData equipmentPutchasedData = DataManager.Instance.GetEquipmentPutchasedData(equipmentType, equipCode);
			costText.text = equipmentPutchasedData.Price.ToString();
			price = equipmentPutchasedData.Price;
			CheckAndSetLockStat();
			RegisterEvent();
		}
		if (equipmentCardAttr.Occupation == PlayerOccupation.None)
		{
			occuIcon.gameObject.SetActive(value: false);
			return;
		}
		occuIcon.gameObject.SetActive(value: true);
		occuIcon.sprite = parentPanel.OccupationIcon[equipmentCardAttr.Occupation];
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
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, currentShowingEquipCode, isCheckSuit: false, isUnlocked: true, isPurchased: true));
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
			(SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI).LoadEquipment(currentShowingEquipCode, 0, "purchase".LocalizeText(), isEquiped: false, OnClickConfirmPurchased, isInteractive);
		}
	}

	private void OnClickConfirmPurchased()
	{
		rootPanel.ShowPurchasedVfx(lockIconImg.transform, 1f);
		lockIconImg.gameObject.SetActive(value: false);
		UnregisterEvent();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.ComsumeCoin(price, isAutoSave: false);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.PurchaseEquipment(currentShowingEquipCode, isAutoSave: false);
		GameSave.SaveUserData();
		isPurchased = true;
		maskImg.gameObject.SetActive(value: false);
	}
}
