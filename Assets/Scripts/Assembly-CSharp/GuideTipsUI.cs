using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GuideTipsUI : UIView
{
	private Action hideHandler;

	private Image bgImg;

	private Text titleText;

	private Image illustrationImg;

	private TextPicMix contentText;

	private Text progressInfo;

	private Button leftBtn;

	private Button rightBtn;

	private Button closeBtn;

	private Image bgMaskImg;

	private Transform targetEndPoint;

	private List<string> allShowingTips;

	private int currentShowingIndex;

	public override string UIViewName => "GuideTipsUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		targetEndPoint = (Transform)objs[0];
		allShowingTips = (List<string>)objs[1];
		hideHandler = (Action)objs[2];
		currentShowingIndex = 0;
		closeBtn.gameObject.SetActive(value: false);
		LoadSingleGuideTip(DataManager.Instance.GetGuideTipData(allShowingTips[0]));
		InitLeftRightBtn();
		UpdateCurrentIndexShowing();
	}

	public override void HideView()
	{
		hideHandler?.Invoke();
		hideHandler = null;
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		bgMaskImg = GetComponent<Image>();
		bgImg = base.transform.Find("Mask/Bg").GetComponent<Image>();
		titleText = base.transform.Find("Mask/Bg/Title").GetComponent<Text>();
		illustrationImg = base.transform.Find("Mask/Bg/Illustration").GetComponent<Image>();
		contentText = base.transform.Find("Mask/Bg/Content").GetComponent<TextPicMix>();
		progressInfo = base.transform.Find("Mask/Bg/ProgressInfo").GetComponent<Text>();
		closeBtn = base.transform.Find("Mask/Bg/CloseBtn").GetComponent<Button>();
		closeBtn.onClick.AddListener(OnClickCloseBtn);
		leftBtn = base.transform.Find("Mask/Bg/LeftBtn").GetComponent<Button>();
		leftBtn.onClick.AddListener(OnClickLeftBtn);
		rightBtn = base.transform.Find("Mask/Bg/RightBtn").GetComponent<Button>();
		rightBtn.onClick.AddListener(OnClickRightBtn);
	}

	private void InitLeftRightBtn()
	{
		leftBtn.gameObject.SetActive(value: false);
		rightBtn.gameObject.SetActive(allShowingTips.Count != 1);
	}

	private void AutoSetLeftRightBtnActive()
	{
		if (currentShowingIndex == 0)
		{
			leftBtn.gameObject.SetActive(value: false);
		}
		else if (currentShowingIndex == 1)
		{
			leftBtn.gameObject.SetActive(value: true);
		}
		if (currentShowingIndex == allShowingTips.Count - 1)
		{
			rightBtn.gameObject.SetActive(value: false);
		}
		else if (currentShowingIndex == allShowingTips.Count - 2)
		{
			rightBtn.gameObject.SetActive(value: true);
		}
	}

	private void LoadSingleGuideTip(GuideTipData data)
	{
		titleText.text = data.TitleKey.LocalizeText();
		contentText.text = data.ContentKey.LocalizeText();
		illustrationImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? data.IllustrationName : data.IllustrationENName, "Sprites/GuideTips");
	}

	private void UpdateCurrentIndexShowing()
	{
		progressInfo.text = string.Format("({0}{1}/{2})", "Tips".LocalizeText(), currentShowingIndex + 1, allShowingTips.Count);
		if (currentShowingIndex == allShowingTips.Count - 1)
		{
			closeBtn.gameObject.SetActive(value: true);
		}
	}

	private void OnClickLeftBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
		if (currentShowingIndex > 0)
		{
			currentShowingIndex--;
			LoadSingleGuideTip(DataManager.Instance.GetGuideTipData(allShowingTips[currentShowingIndex]));
			AutoSetLeftRightBtnActive();
			UpdateCurrentIndexShowing();
		}
	}

	private void OnClickRightBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("右侧标签按钮");
		if (currentShowingIndex < allShowingTips.Count - 1)
		{
			currentShowingIndex++;
			LoadSingleGuideTip(DataManager.Instance.GetGuideTipData(allShowingTips[currentShowingIndex]));
			AutoSetLeftRightBtnActive();
			UpdateCurrentIndexShowing();
		}
	}

	private void OnClickCloseBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用_关闭按钮");
		if (targetEndPoint == null)
		{
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
		else
		{
			StartCoroutine(MoveToTarget_IE());
		}
	}

	private IEnumerator MoveToTarget_IE()
	{
		bgMaskImg.enabled = false;
		bgImg.transform.DOScale(0f, 0.2f);
		bgImg.transform.DOMove(targetEndPoint.position, 0.2f);
		yield return new WaitForSeconds(0.4f);
		bgMaskImg.enabled = true;
		bgImg.transform.localScale = Vector3.one;
		bgImg.transform.localPosition = Vector3.zero;
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
