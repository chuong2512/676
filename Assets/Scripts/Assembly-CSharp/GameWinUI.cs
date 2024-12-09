using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameWinUI : UIView
{
	public Text[] texts;

	private List<Tween> tweenList = new List<Tween>();

	public Image bg;

	public override string UIViewName => "GameWinUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		Action act = (Action)objs[0];
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		Text[] array = texts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].WithCol(0f);
		}
		bg.WithCol(0f);
		StartCoroutine(StartCo(act));
	}

	private IEnumerator StartCo(Action act)
	{
		tweenList.Add(bg.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.5f);
		Text[] array = texts;
		foreach (Text target in array)
		{
			tweenList.Add(target.DOFade(1f, 1f));
			yield return new WaitForSeconds(2f);
		}
		yield return new WaitForSeconds(2f);
		Text[] array2 = texts;
		foreach (Text target2 in array2)
		{
			tweenList.Add(target2.DOFade(0f, 1.5f));
		}
		yield return new WaitForSeconds(1.5f);
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		act?.Invoke();
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
	}
}
