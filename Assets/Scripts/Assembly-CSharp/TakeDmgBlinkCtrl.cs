using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TakeDmgBlinkCtrl : MonoBehaviour
{
	public enum BlinkType
	{
		shield,
		blood
	}

	private Image redImg;

	private float ratio;

	private BlinkType type;

	private Tween redImgTween;

	public void Init()
	{
		redImg = GetComponent<Image>();
		ratio = 0f;
	}

	public void Blink(float newRatio, BlinkType _type, Action blinkCompleteAction)
	{
		if ((_type != 0 || type != BlinkType.blood) && (!(newRatio < ratio) || _type != type))
		{
			if (redImgTween != null && redImgTween.IsActive())
			{
				redImgTween.Complete();
			}
			type = _type;
			redImg.color = ((type == BlinkType.blood) ? "#fa2c2cff".ToColor() : "#ddddddff".ToColor());
			redImg.color = new Color(redImg.color.r, redImg.color.g, redImg.color.b, Mathf.Lerp(0.6f, 1f, newRatio));
			redImgTween = redImg.DOFade(0f, Mathf.Lerp(0.6f, 1f, ratio)).OnComplete(delegate
			{
				ratio = 0f;
				blinkCompleteAction?.Invoke();
			});
			ratio = newRatio;
			type = BlinkType.shield;
		}
	}
}
