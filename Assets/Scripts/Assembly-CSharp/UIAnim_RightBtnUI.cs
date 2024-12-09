using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIAnim_RightBtnUI : UIAnimBase
{
	[SerializeField]
	private List<Transform> buttonList;

	[SerializeField]
	private Transform buttonListStartTrans;

	private List<Tween> tweenList = new List<Tween>();

	private float originalButtonListPosX;

	public void Init()
	{
		if (buttonList != null && buttonList.Count > 0)
		{
			originalButtonListPosX = buttonList[0].localPosition.x;
		}
	}

	private void Reset()
	{
		foreach (Transform button in buttonList)
		{
			Vector3 localPosition = button.localPosition;
			localPosition = (button.localPosition = localPosition.WithV3(buttonListStartTrans.localPosition.x));
		}
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
	}

	public override void StartAnim()
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		WaitForSeconds waitfor80mili = new WaitForSeconds(0.08f);
		foreach (Transform button in buttonList)
		{
			tweenList.Add(button.DOLocalMoveX(originalButtonListPosX, 0.5f).SetEase(Ease.InQuint));
			yield return waitfor80mili;
		}
	}
}
