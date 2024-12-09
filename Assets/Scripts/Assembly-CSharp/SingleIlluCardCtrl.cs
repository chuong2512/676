using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleIlluCardCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string CardNotUnlockedName = "???";

	private UsualNoDesCardInfo _cardInfo;

	private Image lockMask;

	private bool isUnlocked;

	private bool isPurchased;

	private string currentShowingCardCode;

	private Tween scaleTween;

	private float originalScale;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		_cardInfo = base.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
		lockMask = base.transform.Find("CardMask").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadCard(CardIllustrBookPanel parentPanel, SpecialUsualCardAttr data, bool isUnlocked, bool isPurchased)
	{
		this.isUnlocked = isUnlocked;
		this.isPurchased = isPurchased;
		currentShowingCardCode = data.CardCode;
		if (isUnlocked)
		{
			_cardInfo.LoadCard(data.CardCode);
		}
		else
		{
			_cardInfo.LoadCard(parentPanel.CardNotUnlockedSprite, "???", string.Empty);
		}
		lockMask.gameObject.SetActive(!isPurchased);
		originalScale = base.transform.localScale.x;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isUnlocked)
		{
			scaleTween.KillTween();
			scaleTween = base.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, currentShowingCardCode, isUnlocked, isPurchased));
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, currentShowingCardCode, isUnlocked, isPurchased));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = base.transform.DOScale(originalScale, 0.15f);
	}
}
