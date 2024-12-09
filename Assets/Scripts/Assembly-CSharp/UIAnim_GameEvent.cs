using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_GameEvent : UIAnimBase
{
	[SerializeField]
	private CanvasGroup m_CanvasGroup;

	[SerializeField]
	private CanvasGroup illustrationBgCg;

	[SerializeField]
	private CanvasGroup btnListCg;

	[SerializeField]
	private Text titleText;

	[SerializeField]
	private Text contentTxt;

	[SerializeField]
	private Image healthIconImg;

	[SerializeField]
	private Image coinIconImg;

	[SerializeField]
	private Image illustrationImg;

	[SerializeField]
	private CanvasGroup healthCG;

	[SerializeField]
	private Text coinAmountTxt;

	[SerializeField]
	private Transform eventBgTran;

	public Action SetBtnTrueAct;

	private List<Tween> tweenList = new List<Tween>();

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(ShowEventAnim_IE());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator ShowEventAnim_IE()
	{
		WaitForSeconds wait200mili = new WaitForSeconds(0.2f);
		new WaitForSeconds(0.1f);
		tweenList.Add(eventBgTran.DOScale(1f, 0.3f));
		tweenList.Add(m_CanvasGroup.DOFade(1f, 0.3f));
		yield return wait200mili;
		tweenList.Add(healthIconImg.DOFade(1f, 0.5f));
		tweenList.Add(healthCG.DOFade(1f, 0.5f));
		tweenList.Add(coinIconImg.DOFade(1f, 0.5f));
		tweenList.Add(coinAmountTxt.DOFade(1f, 0.5f));
		yield return wait200mili;
		tweenList.Add(titleText.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.15f);
		tweenList.Add(contentTxt.DOFade(1f, 1f));
		tweenList.Add(illustrationBgCg.DOFade(1f, 0.4f));
		tweenList.Add(illustrationImg.transform.DOScale(1f, 0.7f));
		yield return wait200mili;
		tweenList.Add(btnListCg.DOFade(1f, 0.2f).OnComplete(delegate
		{
			SetBtnTrueAct?.Invoke();
		}));
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		eventBgTran.localScale = Vector3.zero;
		illustrationImg.transform.localScale = Vector3.one * 1.3f;
		titleText.WithCol(0f);
		contentTxt.WithCol(0f);
		healthIconImg.WithCol(0f);
		coinIconImg.WithCol(0f);
		coinAmountTxt.WithCol(0f);
		CanvasGroup canvasGroup = healthCG;
		CanvasGroup canvasGroup2 = illustrationBgCg;
		CanvasGroup canvasGroup3 = btnListCg;
		float num2 = (m_CanvasGroup.alpha = 0f);
		float num4 = (canvasGroup3.alpha = num2);
		float num7 = (canvasGroup.alpha = (canvasGroup2.alpha = num4));
	}
}
