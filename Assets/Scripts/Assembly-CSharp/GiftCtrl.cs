using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GiftCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Image giftIcon;

	private BaseGift currentGift;

	private bool isShowingDes;

	private RectTransform m_RectTransform;

	private void Awake()
	{
		giftIcon = GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadGift(BaseGift gift)
	{
		currentGift = gift;
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(gift.Name);
		giftIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(giftDataByGiftName.GiftIcon, "Sprites/Gift");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ShowDes();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HideDes();
	}

	private void ShowDes()
	{
		if (!isShowingDes)
		{
			isShowingDes = true;
			GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(currentGift.Name);
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(m_RectTransform, giftDataByGiftName.NameKey.LocalizeText() + "\n" + giftDataByGiftName.DesKey.LocalizeText(), giftDataByGiftName.AllKeys));
		}
	}

	private void HideDes()
	{
		if (isShowingDes)
		{
			isShowingDes = false;
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		}
	}
}
