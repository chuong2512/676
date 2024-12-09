using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleIlluSkillCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string NotUnlockedName = "???";

	private Image iconImg;

	private Text nameText;

	private bool isUnlocked;

	private bool isPurchased;

	private Image lockMaskImg;

	private string currentShowingSkillCode;

	private PlayerOccupation playerOccupation;

	private Tween scaleTween;

	private float originalScale;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		lockMaskImg = base.transform.Find("LockMask").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadSkill(SkillIlluBookPanel parentPanel, SkillCardAttr data, bool isUnlocked, bool isPurchased)
	{
		this.isUnlocked = isUnlocked;
		this.isPurchased = isPurchased;
		currentShowingSkillCode = data.CardCode;
		playerOccupation = parentPanel.CurrentPlayerOccupation;
		nameText.text = (isUnlocked ? data.NameKey.LocalizeText() : "???");
		iconImg.sprite = (isUnlocked ? SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.IllustrationName, "Sprites/SkillIcon") : parentPanel.NotUnlockedSkillSprite);
		lockMaskImg.gameObject.SetActive(!isPurchased);
		originalScale = iconImg.transform.localScale.x;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isUnlocked)
		{
			scaleTween.KillTween();
			scaleTween = iconImg.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentShowingSkillCode, playerOccupation, isUnlocked, isPurchased, isCheckCardStat: false));
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentShowingSkillCode, playerOccupation, isUnlocked, isPurchased, isCheckCardStat: false));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = iconImg.transform.DOScale(originalScale, 0.15f);
	}
}
