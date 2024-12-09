using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardHeapUI : EscCloseUIView
{
	private Transform remainCardRoot;

	private Transform discardRoot;

	private Queue<CardHeapCardCtrl> cardHeapCardPool = new Queue<CardHeapCardCtrl>();

	private HashSet<CardHeapCardCtrl> allShowingHeapCards = new HashSet<CardHeapCardCtrl>();

	private Transform rightKeyRoot;

	private RectTransform rightKeyRectRoot;

	private Transform leftKeyRoot;

	private RectTransform leftKeyRectRoot;

	private Text handFlagText;

	private Transform cardRemainBgTrans;

	private Transform cardDisacardBgTrans;

	private Transform startPoint;

	private Transform endPoint;

	private Button confirmBtn;

	private Scrollbar cardRemainScrollbar;

	private Scrollbar cardDiscardScrollbar;

	public override string UIViewName => "PlayerCardHeapUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("牌堆界面");
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllCards();
	}

	protected override void OnHide()
	{
		base.OnHide();
		Hide();
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Player Card Heap UI...");
	}

	public override void OnSpawnUI()
	{
		startPoint = base.transform.Find("Bg/StartPoint");
		endPoint = base.transform.Find("Bg/EndPoint");
		cardRemainBgTrans = base.transform.Find("Bg/CardRemainBg");
		remainCardRoot = cardRemainBgTrans.Find("CardRemain/Mask/Content");
		cardDisacardBgTrans = base.transform.Find("Bg/CardDiscardBg");
		discardRoot = cardDisacardBgTrans.Find("CardDiscard/Mask/Content");
		confirmBtn = base.transform.Find("Bg/ConfirmBtn").GetComponent<Button>();
		confirmBtn.onClick.AddListener(Hide);
		rightKeyRoot = base.transform.Find("Bg/RightKeyRoot");
		rightKeyRectRoot = rightKeyRoot.GetComponent<RectTransform>();
		leftKeyRoot = base.transform.Find("Bg/LeftKeyRoot");
		leftKeyRectRoot = leftKeyRoot.GetComponent<RectTransform>();
		handFlagText = base.transform.Find("Bg/HandFlag/FlagText").GetComponent<Text>();
		cardRemainScrollbar = base.transform.Find("Bg/CardRemainBg/CardRemain/Scrollbar").GetComponent<Scrollbar>();
		cardDiscardScrollbar = base.transform.Find("Bg/CardDiscardBg/CardDiscard/Scrollbar").GetComponent<Scrollbar>();
	}

	public void ShowCardHeap(Dictionary<string, int> remain, Dictionary<string, int> discard, bool isMain)
	{
		if (!remain.IsNull() && remain.Count > 0)
		{
			foreach (KeyValuePair<string, int> item in remain)
			{
				Debug.Log($"Card : {item.Key}, amount : {item.Value}");
				CardHeapCardCtrl card = GetCard(remainCardRoot);
				card.LoadCard(item.Key, item.Value, isRemain: true);
				allShowingHeapCards.Add(card);
			}
		}
		if (!discard.IsNull())
		{
			foreach (KeyValuePair<string, int> item2 in discard)
			{
				CardHeapCardCtrl card2 = GetCard(discardRoot);
				card2.LoadCard(item2.Key, item2.Value, isRemain: false);
				allShowingHeapCards.Add(card2);
			}
		}
		handFlagText.text = (isMain ? "MainHand" : "SupHand").LocalizeText();
		StartCoroutine(ShowCardHeapAnim_IE());
	}

	private IEnumerator ShowCardHeapAnim_IE()
	{
		cardRemainBgTrans.position = new Vector3(cardRemainBgTrans.position.x, startPoint.position.y, cardRemainBgTrans.position.z);
		cardDisacardBgTrans.position = new Vector3(cardDisacardBgTrans.position.x, startPoint.position.y, cardDisacardBgTrans.position.z);
		confirmBtn.gameObject.SetActive(value: false);
		cardRemainBgTrans.DOMoveY(endPoint.position.y, 0.6f).SetEase(Ease.OutQuint);
		yield return null;
		cardRemainScrollbar.value = 1f;
		cardDiscardScrollbar.value = 1f;
		yield return new WaitForSeconds(0.2f);
		cardDisacardBgTrans.DOMoveY(endPoint.position.y, 0.6f).SetEase(Ease.OutQuint).OnComplete(delegate
		{
			confirmBtn.gameObject.SetActive(value: true);
		});
	}

	private CardHeapCardCtrl GetCard(Transform root)
	{
		if (cardHeapCardPool.Count > 0)
		{
			CardHeapCardCtrl cardHeapCardCtrl = cardHeapCardPool.Dequeue();
			cardHeapCardCtrl.gameObject.SetActive(value: true);
			cardHeapCardCtrl.transform.SetParent(root);
			return cardHeapCardCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("CardHeapCardCtrl", "Prefabs", root).GetComponent<CardHeapCardCtrl>();
	}

	private void RecycleAllCards()
	{
		if (allShowingHeapCards.Count <= 0)
		{
			return;
		}
		foreach (CardHeapCardCtrl allShowingHeapCard in allShowingHeapCards)
		{
			allShowingHeapCard.gameObject.SetActive(value: false);
			cardHeapCardPool.Enqueue(allShowingHeapCard);
		}
		allShowingHeapCards.Clear();
	}

	private void Hide()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
