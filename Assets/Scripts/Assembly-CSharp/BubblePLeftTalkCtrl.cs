using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubblePLeftTalkCtrl : MonoBehaviour
{
	private Text mtopContentText;

	private Sequence mTopSequence;

	private Coroutine mTopCor;

	private BubbleTalkUI _bubbleTalkUi;

	private CanvasGroup _canvasGroup;

	private bool isRecycled;

	private void Awake()
	{
		mtopContentText = base.transform.Find("Content").GetComponent<Text>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		StopCoroutine(mTopCor);
		HidePLeftBubble();
	}

	public void ShowPLeftBubble(string content, Vector3 pos, BubbleTalkUI bubbleTalkUi)
	{
		if (mTopSequence != null && mTopSequence.IsActive())
		{
			mTopSequence.Complete();
		}
		_bubbleTalkUi = bubbleTalkUi;
		base.transform.position = pos;
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
		mTopCor = StartCoroutine(BubbleCounter_IE(HidePLeftBubble));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("气泡对话");
		isRecycled = false;
	}

	public void SpecialHide()
	{
		if (mTopCor != null)
		{
			StopCoroutine(mTopCor);
		}
		base.transform.DOLocalRotate(new Vector3(0f, 0f, 90f), 0.4f);
		_canvasGroup.DOFade(0f, 0.2f);
		base.transform.DOScale(0f, 0.4f).OnComplete(delegate
		{
			base.transform.rotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			_canvasGroup.alpha = 1f;
			HidePLeftBubble();
		});
	}

	private IEnumerator BubbleCounter_IE(Action callback)
	{
		yield return new WaitForSeconds(3.4f);
		callback?.Invoke();
	}

	private void HidePLeftBubble()
	{
		if (!isRecycled)
		{
			isRecycled = true;
			base.gameObject.SetActive(value: false);
			_bubbleTalkUi.RecyclePLeftBubble(this);
		}
	}
}
