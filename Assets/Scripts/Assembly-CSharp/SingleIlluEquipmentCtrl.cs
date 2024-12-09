using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleIlluEquipmentCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string NotUnlockedName = "???";

	private Image iconImg;

	private Text nameText;

	private string currentShowingEquipCode;

	private Image maskImg;

	private bool isUnlocked;

	private bool isPurchased;

	private Image occuIcon;

	private Tween scaleTween;

	private float originalScale;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		maskImg = base.transform.Find("Mask").GetComponent<Image>();
		occuIcon = base.transform.Find("PlayerOccuIcon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadEquipment(EquipmentCardAttr attrData, EquipmentIlluBookPanel parentPanel, Material material, bool isUnlocked, bool isPurchased)
	{
		currentShowingEquipCode = attrData.CardCode;
		this.isUnlocked = isUnlocked;
		this.isPurchased = isPurchased;
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(attrData.ImageName, "Sprites/Equipment");
		nameText.text = (isUnlocked ? attrData.NameKey.LocalizeText() : "???");
		iconImg.material = material;
		maskImg.gameObject.SetActive(!isPurchased);
		iconImg.SetNativeSize();
		if (attrData.Occupation == PlayerOccupation.None || !isUnlocked)
		{
			occuIcon.gameObject.SetActive(value: false);
		}
		else
		{
			occuIcon.gameObject.SetActive(value: true);
			occuIcon.sprite = parentPanel.OccupationIcon[attrData.Occupation];
		}
		originalScale = base.transform.localScale.x;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isUnlocked)
		{
			scaleTween.KillTween();
			scaleTween = base.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, currentShowingEquipCode, isCheckSuit: false, isUnlocked, isPurchased));
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, currentShowingEquipCode, isCheckSuit: false, isUnlocked, isPurchased));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = base.transform.DOScale(originalScale, 0.15f);
	}
}
