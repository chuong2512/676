using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleMRightTalkCtrl : MonoBehaviour
{
	private Text mtopContentText;

	private Sequence mTopSequence;

	private Coroutine mTopCor;

	private BubbleTalkUI _bubbleTalkUi;

	private void Awake()
	{
		mtopContentText = base.transform.Find("Content").GetComponent<Text>();
	}

	private void OnDisable()
	{
		StopCoroutine(mTopCor);
		HideMRightBubble();
	}

	public void ShowMRightBubble(string content, BubbleTalkUI bubbleTalkUi)
	{
		if (mTopSequence != null && mTopSequence.IsActive())
		{
			mTopSequence.Complete();
		}
		_bubbleTalkUi = bubbleTalkUi;
		mTopSequence = DOTween.Sequence();
		mtopContentText.color = Color.clear;
		mtopContentText.text = content;
		base.transform.localScale = new Vector3(1f, 0f, 1f);
		mTopSequence.Append(base.transform.DOScaleY(1f, 0.6f).SetEase(Ease.OutElastic));
		mTopSequence.Append(mtopContentText.DOColor(Color.black, 0.4f));
		if (mTopCor != null)
		{
			StopCoroutine(mTopCor);
		}
		mTopCor = StartCoroutine(BubbleCounter_IE(HideMRightBubble));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("气泡对话");
	}

	private IEnumerator BubbleCounter_IE(Action callback)
	{
		yield return new WaitForSeconds(3.4f);
		callback?.Invoke();
	}

	private void HideMRightBubble()
	{
		base.gameObject.SetActive(value: false);
		_bubbleTalkUi.RecycleMRightBubble(this);
	}
}
