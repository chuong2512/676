using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummaryCardCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Text cardInfoText;

	private RectTransform m_RectTransform;

	private string currentCardCode;

	private void Awake()
	{
		cardInfoText = base.transform.Find("CardInfo").GetComponent<Text>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadCard(string cardCode, int amount)
	{
		currentCardCode = cardCode;
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		cardInfoText.text = usualCardAttr.NameKey.LocalizeText() + "×" + amount;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.Card, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, currentCardCode, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
