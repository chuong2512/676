using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleProphesyInfoIconCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Image bottomImg;

	private Image iconImg;

	private string currentCardCode;

	private const string ProphesyAssetPath = "Sprites/Prophesy";

	private RectTransform m_RectTransform;

	private void Awake()
	{
		bottomImg = GetComponent<Image>();
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadCard(ProphesyCardData data)
	{
		currentCardCode = data.CardCode;
		bottomImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.IconBottomName, "Sprites/Prophesy");
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.IconName, "Sprites/Prophesy");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ProphesyCardData prophesyCardDataByCardData = DataManager.Instance.GetProphesyCardDataByCardData(currentCardCode);
		string content = prophesyCardDataByCardData.NameKey.LocalizeText() + "\n" + prophesyCardDataByCardData.DescriptionKey.LocalizeText();
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(m_RectTransform, content, null));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
