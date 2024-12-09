using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : UIView
{
	private Image maskImg;

	public override string UIViewName => "MaskUI";

	public override string UILayerName => "OutGameLayer";

	public override void OnSpawnUI()
	{
		maskImg = base.transform.Find("Mask").GetComponent<Image>();
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Mask UI ...");
	}

	public void ShowMask(Action callback)
	{
		maskImg.raycastTarget = true;
		maskImg.color = Color.clear;
		maskImg.DOFade(1f, 1.2f).OnComplete(delegate
		{
			callback?.Invoke();
		});
	}

	public void ActiveRaycast()
	{
		maskImg.raycastTarget = true;
	}

	public void DeactiveRaycast()
	{
		maskImg.raycastTarget = false;
	}

	public void ShowMaskAndFade(float maskTime, Action callback)
	{
		maskImg.color = Color.clear;
		maskImg.raycastTarget = true;
		StartCoroutine(ShowMaskAndFade_IE(maskTime, callback));
	}

	private IEnumerator ShowMaskAndFade_IE(float maskTime, Action callback)
	{
		maskImg.color = Color.clear;
		maskImg.DOFade(1f, 1.2f);
		yield return new WaitForSeconds(1.2f + maskTime);
		maskImg.DOFade(0f, 1.2f).OnComplete(delegate
		{
			maskImg.raycastTarget = false;
			callback?.Invoke();
		});
	}

	public void ShowMaskAndFade(float maskTime, Action middleAction, Action endAction)
	{
		maskImg.color = Color.clear;
		maskImg.raycastTarget = true;
		StartCoroutine(ShowMaskAndFade_IE(maskTime, middleAction, endAction));
	}

	private IEnumerator ShowMaskAndFade_IE(float maskTime, Action middleAction, Action endAction)
	{
		maskImg.DOFade(1f, 1.2f).OnComplete(delegate
		{
			middleAction?.Invoke();
		});
		yield return new WaitForSeconds(1.2f + maskTime);
		maskImg.DOFade(0f, 1.2f).OnComplete(delegate
		{
			maskImg.raycastTarget = false;
			endAction?.Invoke();
		});
	}

	public void ShowMask(float waitTime, Action callback)
	{
		maskImg.color = Color.clear;
		StartCoroutine(ShowMask_IE(waitTime, 1.2f, callback));
	}

	private IEnumerator ShowMask_IE(float waitTime, float fadeTime, Action callback)
	{
		maskImg.raycastTarget = true;
		yield return new WaitForSeconds(waitTime);
		maskImg.DOFade(1f, fadeTime).OnComplete(delegate
		{
			callback?.Invoke();
		});
	}

	public void ShowMask(float waitTime, float fadeTime, Action callback)
	{
		maskImg.color = Color.clear;
		StartCoroutine(ShowMask_IE(waitTime, fadeTime, callback));
	}

	public void ShowFade(Action callback)
	{
		maskImg.DOFade(0f, 1.2f).OnComplete(delegate
		{
			maskImg.raycastTarget = false;
			callback?.Invoke();
		});
	}
}
