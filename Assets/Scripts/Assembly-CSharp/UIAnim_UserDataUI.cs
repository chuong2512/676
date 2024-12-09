using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_UserDataUI : UIAnimBase
{
	public List<Image> scrollImg;

	public List<Text> dataContes;

	public List<Image> deleteBtnImg;

	public List<Image> modifyBtnImg;

	public List<Image> addBtnImg;

	public Transform quitBtnTrans;

	public Image bg;

	public Text title;

	public Transform quitBtnStartTrans;

	public Image highlightPro;

	private List<Tween> tweenList = new List<Tween>();

	private float originBtnStartPosY;

	public void Init()
	{
		originBtnStartPosY = quitBtnTrans.localPosition.y;
	}

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		tweenList.Add(bg.transform.DOScale(1f, 0.3f).SetEase(Ease.InCubic));
		tweenList.Add(bg.Fade(0f, 1f, 0.3f));
		tweenList.Add(quitBtnTrans.DOLocalMoveY(originBtnStartPosY, 0.5f).SetEase(Ease.InQuint));
		yield return new WaitForSeconds(0.25f);
		tweenList.Add(title.DOFade(1f, 0.5f));
		for (int i = 0; i < 3; i++)
		{
			tweenList.Add(scrollImg[i].DOFillAmount(1f, 0.2f));
			tweenList.Add(scrollImg[i].DOFade(1f, 0.2f));
			yield return new WaitForSeconds(0.05f);
			tweenList.Add(dataContes[i].DOFade(1f, 0.5f));
			tweenList.Add(addBtnImg[i].DOFade(1f, 0.5f));
			yield return new WaitForSeconds(0.1f);
			tweenList.Add(deleteBtnImg[i].DOFade(1f, 0.5f));
			tweenList.Add(modifyBtnImg[i].DOFade(1f, 0.5f));
		}
		tweenList.Add(highlightPro.DOFillAmount(1f, 0.5f));
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		title.WithCol(0f);
		quitBtnTrans.localPosition = quitBtnTrans.localPosition.WithV3(null, quitBtnStartTrans.localPosition.y);
		bg.transform.localScale = Vector3.one * 0.5f;
		for (int i = 0; i < 3; i++)
		{
			scrollImg[i].WithCol(0f);
			scrollImg[i].fillAmount = 0.05f;
			dataContes[i].WithCol(0f);
			addBtnImg[i].WithCol(0f);
			deleteBtnImg[i].WithCol(0f);
			modifyBtnImg[i].WithCol(0f);
		}
		highlightPro.fillAmount = 0f;
	}
}
