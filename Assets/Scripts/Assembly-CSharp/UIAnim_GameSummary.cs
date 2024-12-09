using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_GameSummary : UIAnimBase
{
	private List<Tween> tweenList = new List<Tween>();

	public CanvasGroup attrPack;

	public CanvasGroup itemPack;

	public CanvasGroup skillPack;

	public Transform illuStartTrans;

	public Image playerIllus;

	public Text quitTxt;

	public Text cardPanelTitleTxt;

	public Text mainHandTitleTxt;

	public Text supHandTitleTxt;

	public Text attrTitle;

	public Text equipTitle;

	public Text skillTitle;

	public Image attrSepLineImg;

	public Image equipSepLineImg;

	public Image skillSepLineImg;

	private float playerIllsStartPosX;

	private float packsStartPosX;

	private UIAnim_GameSummary_GameProgress subAnim;

	private List<Transform> trans1 = new List<Transform>();

	private List<Transform> trans2 = new List<Transform>();

	public void Init()
	{
		playerIllsStartPosX = playerIllus.transform.localPosition.x;
		packsStartPosX = attrPack.transform.localPosition.x;
		subAnim = GetComponentInChildren<UIAnim_GameSummary_GameProgress>(includeInactive: true);
	}

	public void SetItems(List<Transform> trans1, List<Transform> trans2)
	{
		this.trans1 = trans1;
		this.trans2 = trans2;
	}

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		tweenList.Add(cardPanelTitleTxt.DOFade(1f, 0.5f));
		tweenList.Add(quitTxt.DOFade(1f, 0.5f));
		playerIllus.DOFade(1f, 0.5f);
		playerIllus.transform.DOLocalMoveX(playerIllsStartPosX, 0.3f);
		yield return waitForSeconds;
		subAnim.StartAnim();
		tweenList.Add(mainHandTitleTxt.DOFade(1f, 0.5f));
		tweenList.Add(supHandTitleTxt.DOFade(1f, 0.5f));
		yield return waitfor100mili;
		StartCoroutine(CardCo1());
		StartCoroutine(CardCo2());
		tweenList.Add(attrTitle.DOFade(1f, 0.5f));
		tweenList.Add(attrSepLineImg.DOFillAmount(1f, 0.5f));
		yield return waitfor100mili;
		tweenList.Add(attrPack.DOFade(1f, 0.5f));
		tweenList.Add(attrPack.transform.DOLocalMoveX(1f, 0.4f));
		yield return waitfor100mili;
		tweenList.Add(equipTitle.DOFade(1f, 0.5f));
		tweenList.Add(equipSepLineImg.DOFillAmount(1f, 0.5f));
		yield return waitfor100mili;
		tweenList.Add(itemPack.DOFade(1f, 0.5f));
		tweenList.Add(itemPack.transform.DOLocalMoveX(1f, 0.4f));
		yield return waitfor100mili;
		tweenList.Add(skillTitle.DOFade(1f, 0.5f));
		tweenList.Add(skillSepLineImg.DOFillAmount(1f, 0.5f));
		yield return waitfor100mili;
		tweenList.Add(skillPack.DOFade(1f, 0.5f));
		tweenList.Add(skillPack.transform.DOLocalMoveX(1f, 0.4f));
	}

	private IEnumerator CardCo1()
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		foreach (Transform item in trans1)
		{
			item.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
			yield return waitfor100mili;
		}
	}

	private IEnumerator CardCo2()
	{
		WaitForSeconds waitfor100mili = new WaitForSeconds(0.1f);
		foreach (Transform item in trans2)
		{
			item.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
			yield return waitfor100mili;
		}
	}

	private void Reset()
	{
		subAnim.Reset();
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		foreach (Transform item in trans1)
		{
			item.localScale = Vector3.zero;
		}
		foreach (Transform item2 in trans2)
		{
			item2.localScale = Vector3.zero;
		}
		quitTxt.WithCol(0f);
		playerIllus.WithCol(0f);
		cardPanelTitleTxt.WithCol(0f);
		mainHandTitleTxt.WithCol(0f);
		supHandTitleTxt.WithCol(0f);
		attrTitle.WithCol(0f);
		equipTitle.WithCol(0f);
		skillTitle.WithCol(0f);
		attrSepLineImg.fillAmount = 0f;
		equipSepLineImg.fillAmount = 0f;
		skillSepLineImg.fillAmount = 0f;
		playerIllus.transform.localPosition = illuStartTrans.localPosition;
		CanvasGroup canvasGroup = attrPack;
		CanvasGroup canvasGroup2 = itemPack;
		float num2 = (skillPack.alpha = 0f);
		float num5 = (canvasGroup.alpha = (canvasGroup2.alpha = num2));
		attrPack.transform.localPosition = attrPack.transform.localPosition.WithV3(packsStartPosX + 100f);
		itemPack.transform.localPosition = itemPack.transform.localPosition.WithV3(packsStartPosX + 100f);
		skillPack.transform.localPosition = skillPack.transform.localPosition.WithV3(packsStartPosX + 100f);
	}
}
