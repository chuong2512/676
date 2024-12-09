using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIAnim_Common : MonoBehaviour
{
	[SerializeField]
	private List<Transform> buttonList;

	[SerializeField]
	private Transform buttonListStartTrans;

	private float originalButtonListPosY;

	private List<Tween> tweenList = new List<Tween>();

	private List<Tween> tweenList2 = new List<Tween>();

	private Coroutine startCoroutine;

	private Coroutine slotExpandCoroutin;

	public void StartAnim()
	{
		Reset();
		if (startCoroutine != null)
		{
			StopCoroutine(startCoroutine);
		}
		startCoroutine = StartCoroutine(StartAnimCo());
	}

	public void SetSlotsAnim(List<CanvasGroup> allshowingElements)
	{
		ResetSlot(allshowingElements);
		if (slotExpandCoroutin != null)
		{
			StopCoroutine(slotExpandCoroutin);
		}
		foreach (CanvasGroup allshowingElement in allshowingElements)
		{
			tweenList2.Add(allshowingElement.DOFade(1f, 0.5f));
		}
	}

	public void Init()
	{
		if (buttonList != null && buttonList.Count > 0)
		{
			originalButtonListPosY = buttonList[0].localPosition.y;
		}
	}

	private void Reset()
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
	}

	private void ResetSlot(List<CanvasGroup> allshowingElements)
	{
		foreach (Tween item in tweenList2)
		{
			item.KillTween();
		}
		foreach (CanvasGroup allshowingElement in allshowingElements)
		{
			allshowingElement.alpha = 0f;
		}
	}

	private IEnumerator StartAnimCo()
	{
		WaitForSeconds waitfor50mili = new WaitForSeconds(0.05f);
		yield return new WaitForSeconds(0.1f);
		foreach (Transform button in buttonList)
		{
			tweenList.Add(button.DOLocalMoveY(originalButtonListPosY, 0.5f).SetEase(Ease.InQuint));
			yield return waitfor50mili;
		}
	}
}
