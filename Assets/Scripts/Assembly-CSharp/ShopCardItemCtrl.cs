using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCardItemCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private static readonly Color DiscountColor = "0FCC3BFF".HexColorToColor();

	private static readonly Color CanBuyColor = "DE9E1CFF".HexColorToColor();

	private static readonly Color CanNotBuyColor = "B53218FF".HexColorToColor();

	private UsualWithDesCardInfo _cardInfo;

	private CardShopUI _cardShopUi;

	private Text priceText;

	private Outline priceTextOutline;

	private Image saleImg;

	private Tween cardMoveTween;

	private bool isSaled;

	private float initLocalPosY;

	private Transform rightKeyRoot;

	private Transform leftKeyRoot;

	public string CurrentCardCode { get; private set; }

	public int Price { get; private set; }

	public bool IsSaled => isSaled;

	private void Awake()
	{
		_cardInfo = base.transform.Find("UsualWithDesCard").GetComponent<UsualWithDesCardInfo>();
		priceText = base.transform.Find("PriceText").GetComponent<Text>();
		priceTextOutline = base.transform.Find("PriceText").GetComponent<Outline>();
		saleImg = base.transform.Find("UsualWithDesCard/SaleImg").GetComponent<Image>();
		initLocalPosY = _cardInfo.transform.localPosition.y;
		rightKeyRoot = base.transform.Find("RightKeyRoot");
		leftKeyRoot = base.transform.Find("LeftKeyRoot");
	}

	public void LoadCard(string cardCode, CardShopUI cardShopUi)
	{
		_cardInfo.LoadCard(cardCode);
		_cardShopUi = cardShopUi;
		CurrentCardCode = cardCode;
		SpecialUsualCardAttr specialUsualCardAttr = DataManager.Instance.GetSpecialUsualCardAttr(cardCode);
		Price = specialUsualCardAttr.Value;
		priceText.text = Price + "G";
		saleImg.gameObject.SetActive(value: false);
		isSaled = false;
		SetPriceColor();
		EventManager.RegisterEvent(EventEnum.E_PlayerCoinUpdate, OnPlayerUpdateCoin);
	}

	public void SetSale()
	{
		saleImg.gameObject.SetActive(value: true);
		SpecialUsualCardAttr specialUsualCardAttr = DataManager.Instance.GetSpecialUsualCardAttr(CurrentCardCode);
		Price = Mathf.FloorToInt((float)specialUsualCardAttr.Value * 0.5f);
		priceText.text = Price + "G";
		isSaled = true;
		SetPriceColor();
	}

	private void OnPlayerUpdateCoin(EventData data)
	{
		SetPriceColor();
	}

	private void SetPriceColor()
	{
		if (Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= Price)
		{
			priceText.color = (isSaled ? DiscountColor : CanBuyColor);
			_cardInfo.SetImageColorTint(isTrue: false);
		}
		else
		{
			priceText.color = CanNotBuyColor;
			_cardInfo.SetImageColorTint(isTrue: true);
		}
	}

	public void OnRecycle()
	{
		EventManager.UnregisterEvent(EventEnum.E_PlayerCoinUpdate, OnPlayerUpdateCoin);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			BagCardDesUI obj = SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI;
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("PURCHASE".LocalizeText()).AppendFormat("(").Append(Price)
				.Append("G)");
			obj.ShowBigCard(CurrentCardCode, stringBuilder.ToString(), delegate
			{
				SingletonDontDestroy<UIManager>.Instance.HideView("BagCardDesUI");
				_cardShopUi.OnClickPurchaesBtn();
			}, Singleton<GameManager>.Instance.Player.PlayerInventory.PlayerMoney >= Price);
			_cardShopUi.SetCurrentChooseItem(this);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_cardInfo.transform.DOScale(1f, 0.2f);
		ShowKeys();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_cardInfo.transform.DOScale(0.7f, 0.2f);
		HideKeys();
	}

	private void ShowKeys()
	{
		Vector2 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(base.transform.position);
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(CurrentCardCode);
		_cardShopUi.ShowKeys((vector.x > 0.5f) ? leftKeyRoot : rightKeyRoot, usualCardAttr.AllKeys);
	}

	private void HideKeys()
	{
		_cardShopUi.HideKeys();
	}
}
