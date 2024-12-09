using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleWitchTalkCtrl : MonoBehaviour
{
	private Text witchContentText;

	private Sequence witchSequence;

	private Coroutine mTopCor;

	private BubbleTalkUI _bubbleTalkUi;

	private CanvasGroup _canvasGroup;

	private void Awake()
	{
		witchContentText = base.transform.Find("Content").GetComponent<Text>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		StopCoroutine(mTopCor);
		HideWitchBubble();
	}

	public void ShowWitchBubble(string content, Vector3 pos, BubbleTalkUI bubbleTalkUi)
	{
		if (witchSequence != null && witchSequence.IsActive())
		{
			witchSequence.Complete();
		}
		_bubbleTalkUi = bubbleTalkUi;
		base.transform.position = pos;
		witchSequence = DOTween.Sequence();
		witchContentText.color = Color.clear;
		witchContentText.text = content;
		base.transform.localScale = new Vector3(0f, 1f, 1f);
		witchSequence.Append(base.transform.DOScaleX(1f, 0.6f).SetEase(Ease.OutElastic));
		witchSequence.Append(witchContentText.DOColor(Color.black, 0.4f));
		if (mTopCor != null)
		{
			StopCoroutine(mTopCor);
		}
		mTopCor = StartCoroutine(BubbleCounter_IE(HideWitchBubble));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("气泡对话");
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
			HideWitchBubble();
		});
	}

	private IEnumerator BubbleCounter_IE(Action callback)
	{
		yield return new WaitForSeconds(3.4f);
		callback?.Invoke();
	}

	private void HideWitchBubble()
	{
		base.gameObject.SetActive(value: false);
		_bubbleTalkUi.RecycleWitchBubble(this);
	}
}
