using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_HelpUI : UIAnimBase
{
	public Image[] tabImgs;

	public Text[] tabTxt;

	public CanvasGroup contentCG;

	private List<Tween> tweenList = new List<Tween>();

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		contentCG.DOFade(1f, 1f);
		for (int i = 0; i < 3; i++)
		{
			tabImgs[i].DOFillAmount(1f, 0.3f).SetEase(Ease.OutBack);
			yield return new WaitForSeconds(0.2f);
			tabTxt[i].DOFade(1f, 0.5f);
		}
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		Image[] array = tabImgs;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].fillAmount = 0f;
		}
		Text[] array2 = tabTxt;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].WithCol(0f);
		}
		contentCG.alpha = 0f;
	}
}
