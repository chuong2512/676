using DG.Tweening;
using UnityEngine;

public class UIAnim_SpoilsUI : UIAnimBase
{
	public Transform bg;

	private Tween myTween;

	public override void StartAnim()
	{
		myTween.KillTween();
		bg.localScale = Vector3.zero;
		myTween = bg.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
	}
}
