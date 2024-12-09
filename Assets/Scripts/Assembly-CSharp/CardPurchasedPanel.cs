using System.Collections.Generic;
using UnityEngine;

public class CardPurchasedPanel : MonoBehaviour
{
	private Transform cardRoot;

	private PurchasedItemUI parentPanel;

	private UIAnim_Common anim;

	private Queue<SinglePurchasedCardCtrl> allPurchasedCardPools = new Queue<SinglePurchasedCardCtrl>();

	private List<SinglePurchasedCardCtrl> allShowingPurchasedCards = new List<SinglePurchasedCardCtrl>();

	private void Awake()
	{
		cardRoot = base.transform.Find("CommonCardShowPanel/Mask/Content");
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	public void Show(PurchasedItemUI parentPanel)
	{
		RecycleAllShowingCtrls();
		base.gameObject.SetActive(value: true);
		foreach (KeyValuePair<string, ItemPurchasedData> allSpecialCardPurchasedData in DataManager.Instance.GetAllSpecialCardPurchasedDatas())
		{
			SinglePurchasedCardCtrl singlePurchasedCardCtrl = GetSinglePurchasedCardCtrl();
			singlePurchasedCardCtrl.transform.SetAsLastSibling();
			bool isPurchased = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsSpecialUsualCardPurchased(allSpecialCardPurchasedData.Key);
			singlePurchasedCardCtrl.LoadCard(parentPanel, allSpecialCardPurchasedData.Value, isPurchased);
			allShowingPurchasedCards.Add(singlePurchasedCardCtrl);
		}
		anim.StartAnim();
		List<CanvasGroup> list = new List<CanvasGroup>();
		foreach (SinglePurchasedCardCtrl allShowingPurchasedCard in allShowingPurchasedCards)
		{
			list.Add(allShowingPurchasedCard.CanvasGroup);
		}
		anim.SetSlotsAnim(list);
		parentPanel.SetScrolllbar();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}

	private SinglePurchasedCardCtrl GetSinglePurchasedCardCtrl()
	{
		if (allPurchasedCardPools.Count > 0)
		{
			SinglePurchasedCardCtrl singlePurchasedCardCtrl = allPurchasedCardPools.Dequeue();
			singlePurchasedCardCtrl.gameObject.SetActive(value: true);
			return singlePurchasedCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SinglePurchasedCardCtrl", "Prefabs", cardRoot).GetComponent<SinglePurchasedCardCtrl>();
	}

	private void RecycleAllShowingCtrls()
	{
		if (allShowingPurchasedCards.Count > 0)
		{
			for (int i = 0; i < allShowingPurchasedCards.Count; i++)
			{
				allShowingPurchasedCards[i].gameObject.SetActive(value: true);
				allPurchasedCardPools.Enqueue(allShowingPurchasedCards[i]);
			}
			allShowingPurchasedCards.Clear();
		}
	}
}
