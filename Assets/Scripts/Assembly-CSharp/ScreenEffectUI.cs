using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffectUI : UIView
{
	private Image fullScreenImg;

	private Sequence bgColorTween;

	private TakeDmgBlinkCtrl _takeDmgBlinkCtrl;

	public override string UIViewName => "ScreenEffectUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Screen Effect UI ...");
	}

	public override void OnSpawnUI()
	{
		fullScreenImg = base.transform.Find("Root/FullScreen").GetComponent<Image>();
		InitTakeDmgBlink();
	}

	public void ChangeScreenColor(Color targetColor, Ease ease, float changeTime, float durationTime, float recoveryTime)
	{
		if (!bgColorTween.IsNull() && bgColorTween.IsActive())
		{
			bgColorTween.Complete();
		}
		bgColorTween = DOTween.Sequence();
		bgColorTween.Append(fullScreenImg.DOColor(targetColor, changeTime).SetEase(ease));
		bgColorTween.Append(fullScreenImg.DOColor(targetColor, durationTime));
		bgColorTween.Append(fullScreenImg.DOColor(Color.clear, recoveryTime));
	}

	private void InitTakeDmgBlink()
	{
		_takeDmgBlinkCtrl = base.transform.Find("Root/TakeDmgBlink").GetComponent<TakeDmgBlinkCtrl>();
		_takeDmgBlinkCtrl.Init();
	}

	public void ShowDmgBlink(float newRatio, TakeDmgBlinkCtrl.BlinkType _type)
	{
		_takeDmgBlinkCtrl.Blink(newRatio, _type, HideDmgBlink);
		_takeDmgBlinkCtrl.gameObject.SetActive(value: true);
	}

	private void HideDmgBlink()
	{
		_takeDmgBlinkCtrl.gameObject.SetActive(value: false);
	}
}
