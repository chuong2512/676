using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_BagUI_Card : MonoBehaviour
{
	[SerializeField]
	private List<Transform> buttonList;

	[SerializeField]
	private Transform buttonListStartTrans;

	private float originalButtonListPosY;

	[SerializeField]
	private Text hint;

	[SerializeField]
	private Image inventoryCardRoot;

	[SerializeField]
	private Image mainCardScrollImg;

	[SerializeField]
	private Image supCardScrollImg;

	[SerializeField]
	private Image mainCardTitleImg;

	[SerializeField]
	private List<Text> mainCardTitleTxt;

	[SerializeField]
	private Image supCardTitleImg;

	[SerializeField]
	private List<Text> supCardTitleTxt;

	private List<Tween> tweenList = new List<Tween>();

	private List<Tween> tweenList2 = new List<Tween>();

	private Coroutine startCoroutine;

	private Coroutine startCoroutine_Main;

	private Coroutine startCoroutine_Sup;

	private Coroutine slotExpandCoroutin;

	public void StartAnim(Dictionary<string, BagCardSlotCtrl> mainHandSlots, Dictionary<string, BagCardSlotCtrl> supHandCardSlots, Dictionary<string, BagCardCtrl> mainHandCards, Dictionary<string, BagCardCtrl> supHandCards)
	{
		ResetTween(mainHandSlots, supHandCardSlots, mainHandCards, supHandCards);
		if (startCoroutine != null)
		{
			StopCoroutine(startCoroutine);
		}
		if (startCoroutine_Sup != null)
		{
			StopCoroutine(startCoroutine_Sup);
		}
		if (startCoroutine_Main != null)
		{
			StopCoroutine(startCoroutine_Main);
		}
		startCoroutine = StartCoroutine(StartAnimCo(mainHandSlots, supHandCardSlots, mainHandCards, supHandCards));
	}

	public void SetSlotsAnim(Dictionary<string, BagCardSlotCtrl> cardSlots, Dictionary<string, BagCardCtrl> cards)
	{
		ResetSlot(cardSlots, cards);
		if (slotExpandCoroutin != null)
		{
			StopCoroutine(slotExpandCoroutin);
		}
		slotExpandCoroutin = StartCoroutine(ResetCardAnimCo(cardSlots, cards));
	}

	private IEnumerator ResetCardAnimCo(Dictionary<string, BagCardSlotCtrl> cardSlots, Dictionary<string, BagCardCtrl> cards)
	{
		List<BagCardCtrl> cardList = cards.Values.ToList();
		List<BagCardSlotCtrl> slotList = cardSlots.Values.ToList();
		WaitForSeconds waitfor50mili = new WaitForSeconds(0.05f);
		for (int i = 0; i < cards.Count; i++)
		{
			tweenList2.Add(slotList[i].CanvasGroup.DOFade(1f, 0.3f));
			yield return waitfor50mili;
			tweenList2.Add(cardList[i].CanvasGroup.DOFade(1f, 0.7f));
		}
	}

	public void Init()
	{
		originalButtonListPosY = buttonList[0].localPosition.y;
	}

	private void ResetTween(Dictionary<string, BagCardSlotCtrl> mainHandSlots, Dictionary<string, BagCardSlotCtrl> supHandCardSlots, Dictionary<string, BagCardCtrl> mainHandCards, Dictionary<string, BagCardCtrl> supHandCards)
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		foreach (Transform button in buttonList)
		{
			Vector3 localPosition = button.localPosition;
			localPosition = (button.localPosition = localPosition.WithV3(null, buttonListStartTrans.localPosition.y));
		}
		float num3 = (mainCardScrollImg.fillAmount = (supCardScrollImg.fillAmount = 0.05f));
		inventoryCardRoot.WithCol(0f);
		mainCardTitleImg.WithCol(0f);
		supCardTitleImg.WithCol(0f);
		hint.WithCol(0f);
		foreach (Text item in mainCardTitleTxt)
		{
			item.WithCol(0f);
		}
		foreach (Text item2 in supCardTitleTxt)
		{
			item2.WithCol(0f);
		}
		foreach (BagCardSlotCtrl value in mainHandSlots.Values)
		{
			value.CanvasGroup.alpha = 0f;
		}
		foreach (BagCardSlotCtrl value2 in supHandCardSlots.Values)
		{
			value2.CanvasGroup.alpha = 0f;
		}
		foreach (BagCardCtrl value3 in mainHandCards.Values)
		{
			value3.CanvasGroup.alpha = 0f;
		}
		foreach (BagCardCtrl value4 in supHandCards.Values)
		{
			value4.CanvasGroup.alpha = 0f;
		}
	}

	private void ResetSlot(Dictionary<string, BagCardSlotCtrl> cardSlots, Dictionary<string, BagCardCtrl> cards)
	{
		foreach (Tween item in tweenList2)
		{
			item.KillTween();
		}
		foreach (BagCardSlotCtrl value in cardSlots.Values)
		{
			value.CanvasGroup.alpha = 0f;
		}
		foreach (BagCardCtrl value2 in cards.Values)
		{
			value2.CanvasGroup.alpha = 0f;
		}
	}

	private IEnumerator StartAnimCo(Dictionary<string, BagCardSlotCtrl> mainHandSlots, Dictionary<string, BagCardSlotCtrl> supHandCardSlots, Dictionary<string, BagCardCtrl> mainHandCards, Dictionary<string, BagCardCtrl> supHandCards)
	{
		WaitForSeconds waitfor50mili = new WaitForSeconds(0.05f);
		WaitForSeconds waitfor70mili = new WaitForSeconds(0.07f);
		tweenList.Add(inventoryCardRoot.DOFade(1f, 0.2f));
		yield return new WaitForSeconds(0.1f);
		tweenList.Add(mainCardScrollImg.DOFillAmount(1f, 0.3f));
		foreach (Transform button in buttonList)
		{
			tweenList.Add(button.DOLocalMoveY(originalButtonListPosY, 0.5f).SetEase(Ease.InQuint));
			yield return waitfor50mili;
		}
		tweenList.Add(hint.DOFade(1f, 1f));
		tweenList.Add(supCardScrollImg.DOFillAmount(1f, 0.3f));
		tweenList.Add(mainCardTitleImg.DOFade(1f, 0.5f));
		startCoroutine_Main = StartCoroutine(ShowManHandCardCo(mainHandSlots, mainHandCards));
		foreach (Text t2 in mainCardTitleTxt)
		{
			yield return waitfor70mili;
			tweenList.Add(t2.DOFade(1f, 0.5f));
		}
		startCoroutine_Sup = StartCoroutine(ShowSupHandCardCo(supHandCardSlots, supHandCards));
		tweenList.Add(supCardTitleImg.DOFade(1f, 0.5f));
		foreach (Text t2 in supCardTitleTxt)
		{
			yield return waitfor50mili;
			tweenList.Add(t2.DOFade(1f, 0.5f));
		}
	}

	private IEnumerator ShowSupHandCardCo(Dictionary<string, BagCardSlotCtrl> supHandCardSlots, Dictionary<string, BagCardCtrl> supHandCards)
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		List<BagCardSlotCtrl> slotCtrls = supHandCardSlots.Values.ToList();
		List<BagCardCtrl> ctrls = supHandCards.Values.ToList();
		for (int i = 0; i < supHandCards.Count; i++)
		{
			tweenList.Add(slotCtrls[i].CanvasGroup.DOFade(1f, 0.3f));
			yield return waitfor100mili;
			tweenList.Add(ctrls[i].CanvasGroup.DOFade(1f, 0.7f));
		}
	}

	private IEnumerator ShowManHandCardCo(Dictionary<string, BagCardSlotCtrl> mainHandSlots, Dictionary<string, BagCardCtrl> mainHandCards)
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		List<BagCardSlotCtrl> slotCtrls = mainHandSlots.Values.ToList();
		List<BagCardCtrl> cardCtrls = mainHandCards.Values.ToList();
		for (int i = 0; i < mainHandCards.Count; i++)
		{
			tweenList.Add(slotCtrls[i].CanvasGroup.DOFade(1f, 0.3f));
			yield return waitfor100mili;
			tweenList.Add(cardCtrls[i].CanvasGroup.DOFade(1f, 0.7f));
		}
	}
}
