using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockCardCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private RectTransform m_RectTransform;

	private UsualNoDesCardInfo cardInfo;

	public Transform SpaceTimePanel;

	private Image spaceTimeImg;

	private Text spaceTimeAmountText;

	public string CardCode;

	public int Layer;

	private CardUnlockPanelBase.UnlockCardHandler cardHandler;

	public CardUnlockPanelBase.UnlockCardHandler CardHandler => cardHandler;

	private void Awake()
	{
		cardInfo = base.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
		SpaceTimePanel = base.transform.Find("Mask");
		spaceTimeImg = base.transform.Find("Mask/SpaceTime").GetComponent<Image>();
		spaceTimeAmountText = base.transform.Find("Mask/Amount").GetComponent<Text>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	private void OnDisable()
	{
		cardHandler.OnDisable();
	}

	public void LoadCard(CardUnlockPanelBase.UnlockCardHandler handler)
	{
		cardInfo.LoadCard(CardCode);
		cardHandler = handler;
		cardHandler.OnLoad();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			cardHandler.HandleClick();
		}
	}

	public void SetTimespaceComsume(int amount)
	{
		spaceTimeAmountText.text = amount.ToString();
	}

	public void SetLockMask(Sprite sprite)
	{
		spaceTimeImg.sprite = sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, CardCode, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
