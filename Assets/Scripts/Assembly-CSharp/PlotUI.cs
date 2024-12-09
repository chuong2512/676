using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlotUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private Tween scaleTween;

	private Image iconImg;

	private Text tmpText;

	private Text contentText;

	private Text nameText;

	private Action handler;

	private bool isCanHide;

	public override string UIViewName => "PlotUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("Tips出现时");
		base.gameObject.SetActive(value: true);
		LoadPlotInfo((string)objs[0]);
		handler = (Action)objs[1];
		StartCoroutine(Count_IE());
	}

	private IEnumerator Count_IE()
	{
		scaleTween.KillTween();
		tmpText.transform.localScale = Vector3.zero;
		scaleTween = tmpText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
		isCanHide = false;
		float counter = 0f;
		while (counter <= 1f)
		{
			counter += Time.deltaTime;
			yield return null;
		}
		isCanHide = true;
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		iconImg = base.transform.Find("Mask/TmpText/Bg/Icon").GetComponent<Image>();
		tmpText = base.transform.Find("Mask/TmpText").GetComponent<Text>();
		contentText = base.transform.Find("Mask/TmpText/Bg/ContentText").GetComponent<Text>();
		nameText = base.transform.Find("Mask/TmpText/Bg/NameText").GetComponent<Text>();
	}

	private void LoadPlotInfo(string plotCode)
	{
		PlotData plotData = DataManager.Instance.GetPlotData(plotCode);
		string text3 = (tmpText.text = (contentText.text = plotData.ContentKey.LocalizeText()));
		nameText.text = plotData.NameKey.LocalizeText();
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(plotData.IconName, "Sprites/Plot");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (isCanHide)
		{
			handler?.Invoke();
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}
}
