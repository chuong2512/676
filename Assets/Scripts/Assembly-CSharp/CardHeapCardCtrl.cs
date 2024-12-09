using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHeapCardCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[HideInInspector]
	public string currentCardCode;

	private UsualWithDesCardInfo cardInfo;

	private Text amountText;

	private bool isRemain;

	private void Awake()
	{
		cardInfo = base.transform.Find("UsualWithDesCard").GetComponent<UsualWithDesCardInfo>();
		amountText = base.transform.Find("Amount").GetComponent<Text>();
	}

	public void LoadCard(string cardCode, int amount, bool isRemain)
	{
		this.isRemain = isRemain;
		currentCardCode = cardCode;
		cardInfo.LoadCard(cardCode);
		amountText.text = $"× {amount}";
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI).ShowBigCard(currentCardCode);
		}
	}
}
