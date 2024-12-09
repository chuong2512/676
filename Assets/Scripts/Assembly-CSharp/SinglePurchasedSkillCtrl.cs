using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SinglePurchasedSkillCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private Image iconImg;

	private Text nameText;

	private Image maskImg;

	private Text costText;

	private bool isPurchased;

	private string currentSkillCode;

	private PlayerOccupation playerOccupation;

	private int price;

	private PurchasedItemUI rootPanel;

	private Image lockIconImg;

	private bool isRegisterEvent;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		maskImg = base.transform.Find("LockMask").GetComponent<Image>();
		costText = base.transform.Find("LockMask/CostText").GetComponent<Text>();
		lockIconImg = base.transform.Find("LockMask/LockIcon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		UnregisterEvent();
	}

	public void LoadSkill(PurchasedItemUI rootPanel, PlayerOccupation playerOccupation, ItemPurchasedData data, bool isPurchased)
	{
		this.rootPanel = rootPanel;
		this.playerOccupation = playerOccupation;
		currentSkillCode = data.ItemCode;
		this.isPurchased = isPurchased;
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, data.ItemCode);
		nameText.text = skillCardAttr.NameKey.LocalizeText();
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCardAttr.IllustrationName, "Sprites/SkillIcon");
		if (isPurchased)
		{
			maskImg.gameObject.SetActive(value: false);
			return;
		}
		maskImg.gameObject.SetActive(value: true);
		costText.text = data.Price.ToString();
		price = data.Price;
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
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkillCode, playerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: false));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!isPurchased)
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("技能详情");
			bool isInteractive = SingletonDontDestroy<Game>.Instance.CurrentUserData.GameCoin >= price;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SkillDescriptionUI") as SkillDescriptionUI).ShowSkillDescription(playerOccupation, currentSkillCode, isOnBattle: false, "purchase".LocalizeText(), OnClickComfirmPurchase, isInteractive);
		}
	}

	private void OnClickComfirmPurchase()
	{
		rootPanel.ShowPurchasedVfx(lockIconImg.transform, 0.8f);
		lockIconImg.gameObject.SetActive(value: false);
		UnregisterEvent();
		SingletonDontDestroy<Game>.Instance.CurrentUserData.ComsumeCoin(price, isAutoSave: false);
		SingletonDontDestroy<Game>.Instance.CurrentUserData.PurchaseSkill(currentSkillCode, isAutoSave: false);
		GameSave.SaveUserData();
		isPurchased = true;
		maskImg.gameObject.SetActive(value: false);
		SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
	}
}
