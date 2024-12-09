using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_Gambling : UIAnimBase
{
	public Image quitImg;

	public Text quitText;

	public CanvasGroup healthCg;

	public Image coinImg;

	public Text coingAmountTxt;

	public Text gamblingBtnTxt;

	private Coroutine startCoroutine;

	private List<Tween> tweenList = new List<Tween>();

	public override void StartAnim()
	{
		Reset();
		if (startCoroutine != null)
		{
			StopCoroutine(startCoroutine);
		}
		startCoroutine = StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		tweenList.Add(quitImg.DOFade(1f, 0.5f));
		tweenList.Add(quitText.DOFade(1f, 0.5f));
		tweenList.Add(healthCg.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.1f);
		tweenList.Add(coinImg.DOFade(1f, 0.5f));
		tweenList.Add(coingAmountTxt.DOFade(1f, 0.5f));
		tweenList.Add(gamblingBtnTxt.DOFade(1f, 0.5f));
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		quitImg.WithCol(0f);
		quitText.WithCol(0f);
		healthCg.alpha = 0f;
		coinImg.WithCol(0f);
		coingAmountTxt.WithCol(0f);
		gamblingBtnTxt.WithCol(0f);
	}
}
