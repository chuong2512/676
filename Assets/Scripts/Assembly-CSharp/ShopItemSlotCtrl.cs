using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemSlotCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private static readonly Color DiscountColor = "0FCC3BFF".HexColorToColor();

	private static readonly Color CanBuyColor = "DE9E1CFF".HexColorToColor();

	private static readonly Color CanNotBuyColor = "B53218FF".HexColorToColor();

	private Image itemSlotImg;

	private Text itemPriceText;

	private int itemPrice;

	private ShopUI shopUI;

	private Image saleImg;

	private string currentItemCode;

	private int index;

	private bool isSaled;

	private RectTransform m_RectTransform;

	private Tween highlightTween;

	public int ItemPrice => itemPrice;

	public string CurrentItemCode => currentItemCode;

	public bool IsSaled => isSaled;

	private void Awake()
	{
		itemSlotImg = base.transform.Find("Icon").GetComponent<Image>();
		itemPriceText = base.transform.Find("Price").GetComponent<Text>();
		saleImg = base.transform.Find("Sale").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadItem(string equipCode, int index, ShopUI shopUi)
	{
		this.index = index;
		currentItemCode = equipCode;
		shopUI = shopUi;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		itemPrice = equipmentCardAttr.Price;
		saleImg.gameObject.SetActive(value: false);
		itemPriceText.text = $"{itemPrice}G";
		itemSlotImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
		isSaled = false;
		SetPriceColor();
		EventManager.RegisterEvent(EventEnum.E_PlayerCoinUpdate, OnPlayerUpdateCoin);
	}

	public void SetItemSale()
	{
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(currentItemCode);
		itemPrice = Mathf.FloorToInt((float)equipmentCardAttr.Price * 0.5f);
		itemPriceText.text = $"{itemPrice}G";
		saleImg.gameObject.SetActive(value: true);
		isSaled = true;
		SetPriceColor();
	}

	public void OnRecycle()
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerCoinUpdate, OnPlayerUpdateCoin);
	}

	private void OnPlayerUpdateCoin(EventData data)
	{
		SetPriceColor();
	}

	private void SetPriceColor()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= itemPrice)
		{
			itemPriceText.color = (isSaled ? DiscountColor : CanBuyColor);
		}
		else
		{
			itemPriceText.color = CanNotBuyColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			shopUI.SetCurrentChooseItem(this);
			EquipDesUI obj = SingletonDontDestroy<UIManager>.Instance.ShowView("EquipDesUI") as EquipDesUI;
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("PURCHASE".LocalizeText()).AppendFormat("(").Append(itemPrice)
				.Append("G)");
			obj.LoadContrastEquipment(currentItemCode, stringBuilder.ToString(), shopUI.OnClickPurchaseBtn, Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= itemPrice);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, currentItemCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
		if (highlightTween != null && highlightTween.IsActive())
		{
			highlightTween.Complete();
		}
		highlightTween = base.transform.DOScale(1.2f, 0.2f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		if (highlightTween != null && highlightTween.IsActive())
		{
			highlightTween.Complete();
		}
		highlightTween = base.transform.DOScale(1f, 0.2f);
	}
}
