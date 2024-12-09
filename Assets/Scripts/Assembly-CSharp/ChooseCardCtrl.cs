using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCardCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[HideInInspector]
	public string currentCardCode;

	[HideInInspector]
	public UsualCard currentUsualCard;

	private UsualWithDesCardInfo cardInfo;

	private Transform keyRoot_L;

	private Transform keyRoot_R;

	private Image outlightImg;

	private ChooseCardUI _chooseCardUi;

	protected float localpositionX;

	private void Awake()
	{
		cardInfo = base.transform.Find("UsualWithDesCard").GetComponent<UsualWithDesCardInfo>();
		outlightImg = base.transform.Find("Outlight").GetComponent<Image>();
		keyRoot_L = base.transform.Find("KeyRoot_Left");
		keyRoot_R = base.transform.Find("KeyRoot_Right");
	}

	public void LoadCard(ChooseCardUI chooseCardUi, string cardCode)
	{
		_chooseCardUi = chooseCardUi;
		currentCardCode = cardCode;
		cardInfo.LoadCard(cardCode);
		SetNormal();
	}

	public void SetNormal()
	{
		outlightImg.gameObject.SetActive(value: false);
	}

	public void SetHighlight()
	{
		outlightImg.gameObject.SetActive(value: true);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			_chooseCardUi.OnChooseCardWithAmount(this);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(currentCardCode);
		Vector2 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.WorldToViewportPoint(base.transform.position);
		_chooseCardUi.ShowKeys(usualCardAttr.AllKeys, (vector.x > 0.5f) ? keyRoot_L : keyRoot_R);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_chooseCardUi.HideKeys();
	}
}
