using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_ChooseOccupationUI : UIAnimBase
{
	[SerializeField]
	private Image illustrationImg;

	[SerializeField]
	private Image nameFrame;

	[SerializeField]
	private Text nameTxt;

	[SerializeField]
	private Image maxHealthImg;

	[SerializeField]
	private Image initMoneyImg;

	[SerializeField]
	private Text healthTxt;

	[SerializeField]
	private Text moneyTxt;

	[SerializeField]
	private Text occupationTxt;

	[SerializeField]
	private Image lineImg;

	[SerializeField]
	private Text specNamgeTxt;

	[SerializeField]
	private Text specTxt;

	[SerializeField]
	private Image prophesyBtnImg;

	[SerializeField]
	private Image purchaseBtnImg;

	[SerializeField]
	private Button purchaseBtn;

	[SerializeField]
	private Text purchaseTxt;

	[SerializeField]
	private Image presetImg;

	[SerializeField]
	private Button presetBtn;

	[SerializeField]
	private Text presetTxt;

	[SerializeField]
	private Outline startBtnTxtOutline;

	[SerializeField]
	private Image leftBtnImg;

	[SerializeField]
	private Image rightbtnImg;

	public Image prophesyDesBg;

	public CanvasGroup prophesyTxt;

	public Text prophesyVisualTxt;

	public Image proLine;

	public Text proTitle;

	private List<Tween> tweenList = new List<Tween>();

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		WaitForSeconds waitfor200mili = new WaitForSeconds(0.2f);
		WaitForSeconds waitfor50mili = new WaitForSeconds(0.05f);
		tweenList.Add(illustrationImg.Fade(0f, 1f, 1f));
		yield return new WaitForSeconds(0.5f);
		tweenList.Add(startBtnTxtOutline.FadeCol(Color.white, Color.black, 2f));
		tweenList.Add(nameFrame.DOFade(1f, 0.7f));
		tweenList.Add(nameTxt.DOFade(1f, 0.7f));
		tweenList.Add(leftBtnImg.DOFade(1f, 1f));
		tweenList.Add(rightbtnImg.DOFade(1f, 1f));
		if (SingletonDontDestroy<Game>.Instance.CurrentUserData.IsUnlockPrephesyCard)
		{
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("选人界面_预言羊皮纸出现");
		}
		tweenList.Add(prophesyDesBg.material.DOFloat(1f, "_ThreshHold", 1f));
		tweenList.Add(prophesyDesBg.DOFade(1f, 0.3f));
		yield return waitfor100mili;
		tweenList.Add(maxHealthImg.DOFade(1f, 0.7f));
		yield return waitfor50mili;
		tweenList.Add(healthTxt.DOFade(1f, 0.5f));
		yield return waitfor100mili;
		tweenList.Add(initMoneyImg.DOFade(1f, 0.7f));
		yield return waitfor50mili;
		tweenList.Add(moneyTxt.DOFade(1f, 0.5f));
		tweenList.Add(occupationTxt.DOFade(1f, 1f));
		yield return waitfor50mili;
		tweenList.Add(lineImg.DOFade(1f, 0.5f));
		yield return waitfor100mili;
		tweenList.Add(specNamgeTxt.DOFade(1f, 1f));
		yield return waitfor50mili;
		tweenList.Add(specTxt.DOFade(1f, 1f));
		yield return waitfor100mili;
		Button button = purchaseBtn;
		bool interactable = (presetBtn.interactable = true);
		button.interactable = interactable;
		tweenList.Add(prophesyBtnImg.DOFade(1f, 1f));
		tweenList.Add(purchaseBtnImg.DOFade(1f, 1f));
		tweenList.Add(proLine.DOFillAmount(1f, 0.5f));
		tweenList.Add(presetImg.DOFade(1f, 1f));
		tweenList.Add(presetTxt.DOFade(1f, 1f));
		tweenList.Add(purchaseTxt.DOFade(1f, 1f));
		yield return waitfor100mili;
		tweenList.Add(proTitle.DOFade(1f, 0.5f));
		yield return waitfor200mili;
		tweenList.Add(prophesyTxt.DOFade(1f, 0.5f));
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void Reset()
	{
		StopAllCoroutines();
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		nameFrame.WithCol(0f);
		nameTxt.WithCol(0f);
		maxHealthImg.WithCol(0f);
		initMoneyImg.WithCol(0f);
		healthTxt.WithCol(0f);
		moneyTxt.WithCol(0f);
		occupationTxt.WithCol(0f);
		lineImg.WithCol(0f);
		specNamgeTxt.WithCol(0f);
		specTxt.WithCol(0f);
		prophesyBtnImg.WithCol(0f);
		purchaseBtnImg.WithCol(0f);
		purchaseTxt.WithCol(0f);
		presetImg.WithCol(0f);
		presetTxt.WithCol(0f);
		leftBtnImg.WithCol(0f);
		rightbtnImg.WithCol(0f);
		prophesyDesBg.material.SetFloat("_ThreshHold", 0f);
		prophesyDesBg.WithCol(0f);
		prophesyVisualTxt.WithCol(0f);
		prophesyTxt.alpha = 0f;
		prophesyVisualTxt.WithCol(0f);
		proLine.fillAmount = 0f;
		proTitle.WithCol(0f);
		startBtnTxtOutline.effectColor = Color.clear;
		purchaseBtn.interactable = false;
		presetBtn.interactable = false;
	}
}
