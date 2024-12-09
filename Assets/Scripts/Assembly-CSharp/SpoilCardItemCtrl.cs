using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpoilCardItemCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private UsualNoDesCardInfo _cardInfo;

	private Text amountText;

	private RectTransform m_RectTransform;

	public string CurrentCardCode { get; private set; }

	private void Awake()
	{
		_cardInfo = base.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
		amountText = base.transform.Find("Amount").GetComponent<Text>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadCard(string cardCode, int amount)
	{
		CurrentCardCode = cardCode;
		_cardInfo.LoadCard(cardCode);
		amountText.text = ((amount == 1) ? string.Empty : $"×{amount}");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, CurrentCardCode, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
