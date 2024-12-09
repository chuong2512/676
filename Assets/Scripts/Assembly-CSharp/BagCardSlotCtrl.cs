using UnityEngine;
using UnityEngine.UI;

public class BagCardSlotCtrl : MonoBehaviour
{
	private Transform slotRoot;

	private Text amountText;

	private int cardAmount;

	private Text handFlagText;

	private Transform handFlagTrans;

	public CanvasGroup CanvasGroup { get; set; }

	public int CardAmount => cardAmount;

	private void Awake()
	{
		slotRoot = base.transform.Find("Slot/CardRoot");
		amountText = base.transform.Find("Amount").GetComponent<Text>();
		handFlagTrans = base.transform.Find("HandFlagImg");
		handFlagText = base.transform.Find("HandFlagImg/HandText").GetComponent<Text>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void SetCard(BagCardCtrl bagCard, string handFlag, int amount, bool isEquiped)
	{
		bagCard.transform.SetParent(slotRoot);
		bagCard.transform.localScale = Vector3.one;
		bagCard.transform.localPosition = Vector3.zero;
		if (!isEquiped)
		{
			handFlagTrans.gameObject.SetActive(value: true);
			handFlagText.text = handFlag;
		}
		else
		{
			handFlagTrans.gameObject.SetActive(value: false);
		}
		UpdateAmount(amount);
	}

	public void UpdateAmount(int amount)
	{
		cardAmount = amount;
		amountText.text = "×" + amount;
	}
}
