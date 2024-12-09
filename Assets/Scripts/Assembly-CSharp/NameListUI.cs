using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NameListUI : UIView
{
	public Scrollbar scrollBar;

	public Image titleImg;

	public Text listText;

	public Sprite[] titleSprites;

	private List<Tween> tweenList = new List<Tween>();

	public override string UIViewName => "NameListUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		listText.WithCol(0f);
		titleImg.WithCol(0f);
		yield return null;
		scrollBar.value = 1f;
		titleImg.sprite = titleSprites[(SingletonDontDestroy<SettingManager>.Instance.Language != 0) ? 1 : 0];
		tweenList.Add(titleImg.Fade(0f, 1f, 1f));
		yield return new WaitForSeconds(1f);
		tweenList.Add(listText.DOFade(1f, 1f));
		yield return new WaitForSeconds(1f);
		float barvalue = 1f;
		tweenList.Add(DOTween.To(() => barvalue, delegate(float x)
		{
			barvalue = x;
		}, 0f, 30f).OnUpdate(delegate
		{
			scrollBar.value = barvalue;
		}).SetEase(Ease.Linear));
		yield return new WaitForSeconds(32f);
		listText.DOFade(0f, 2f);
		yield return new WaitForSeconds(2.5f);
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	public override void HideView()
	{
		StopAllCoroutines();
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}
}
