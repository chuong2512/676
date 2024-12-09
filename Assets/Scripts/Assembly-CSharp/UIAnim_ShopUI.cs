using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_ShopUI : UIAnimBase
{
	public Text coinText;

	public Image quitImg;

	public Text quitTxt;

	private List<Tween> tweenList = new List<Tween>();

	private List<Transform> trans = new List<Transform>();

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		tweenList.Add(quitImg.DOFade(1f, 0.5f));
		tweenList.Add(quitTxt.DOFade(1f, 0.5f));
		tweenList.Add(coinText.DOFade(1f, 0.5f));
		foreach (Transform tran in trans)
		{
			tran.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		foreach (Transform tran in trans)
		{
			tran.localScale = Vector3.zero;
		}
		quitImg.WithCol(0f);
		quitTxt.WithCol(0f);
		coinText.WithCol(0f);
	}

	public void SetItems(List<Transform> trans)
	{
		this.trans = trans;
	}
}
