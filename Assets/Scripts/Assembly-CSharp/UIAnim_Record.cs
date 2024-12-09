using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_Record : UIAnimBase
{
	public Image bgImg;

	public Transform quitBtnTrans;

	public Transform quitBtnStartTrans;

	private Scrollbar _scrollbar;

	private float originBtnStartPosY;

	private List<Tween> tweenList = new List<Tween>();

	private List<CanvasGroup> cgs;

	public void Init()
	{
		originBtnStartPosY = quitBtnTrans.localPosition.y;
		_scrollbar = base.transform.Find("Mask/Bg/Scrollbar").GetComponent<Scrollbar>();
	}

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		bgImg.DOFade(1f, 0.3f);
		yield return new WaitForSeconds(0.1f);
		tweenList.Add(quitBtnTrans.DOLocalMoveY(originBtnStartPosY, 0.5f).SetEase(Ease.InQuint));
		bgImg.transform.DOScale(1f, 0.3f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(0.2f);
		_scrollbar.value = 1f;
		foreach (CanvasGroup cg in cgs)
		{
			cg.DOFade(1f, 0.5f);
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void SetCgs(List<CanvasGroup> _cgs)
	{
		cgs = _cgs;
	}

	private void Reset()
	{
		bgImg.WithCol(0f);
		bgImg.transform.localScale = Vector3.zero;
		quitBtnTrans.localPosition = quitBtnTrans.localPosition.WithV3(null, quitBtnStartTrans.localPosition.y);
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		foreach (CanvasGroup cg in cgs)
		{
			cg.alpha = 0f;
		}
	}
}
