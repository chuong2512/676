using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BubbleShopTalkCtrl : MonoBehaviour
{
	private Text shopContentText;

	private Sequence shopSequence;

	private Coroutine shopCor;

	private BubbleTalkUI _bubbleTalkUi;

	private CanvasGroup _canvasGroup;

	private void Awake()
	{
		shopContentText = base.transform.Find("Content").GetComponent<Text>();
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void OnDisable()
	{
		StopCoroutine(shopCor);
		HideShopBubble();
	}

	public void ShowShopBubble(string content, Vector3 pos, BubbleTalkUI bubbleTalkUi)
	{
		if (shopSequence != null && shopSequence.IsActive())
		{
			shopSequence.Complete();
		}
		_bubbleTalkUi = bubbleTalkUi;
		base.transform.position = pos;
		shopSequence = DOTween.Sequence();
		shopContentText.color = Color.clear;
		shopContentText.text = content;
		base.transform.localScale = new Vector3(0f, 1f, 1f);
		shopSequence.Append(base.transform.DOScaleX(1f, 0.6f).SetEase(Ease.OutElastic));
		shopSequence.Append(shopContentText.DOColor(Color.black, 0.4f));
		if (shopCor != null)
		{
			StopCoroutine(shopCor);
		}
		shopCor = StartCoroutine(BubbleCounter_IE(HideShopBubble));
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("气泡对话");
	}

	public void SpecialHide()
	{
		if (shopCor != null)
		{
			StopCoroutine(shopCor);
		}
		HideShopBubble();
	}

	private IEnumerator BubbleCounter_IE(Action callback)
	{
		yield return new WaitForSeconds(3.4f);
		callback?.Invoke();
	}

	private void HideShopBubble()
	{
		base.gameObject.SetActive(value: false);
		_bubbleTalkUi.RecycleShopBubble(this);
	}
}
