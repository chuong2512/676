using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOpenUI : UIView
{
	public Sprite englishContentSprites;

	public Sprite chineseContentSprites;

	private Transform logoImg;

	private Image contentText;

	private Image maskImg;

	public override string UIViewName => "GameOpenUI";

	public override string UILayerName => "NormalLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		StartOpenScene();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destory Game Open UI...");
	}

	public override void OnSpawnUI()
	{
		logoImg = base.transform.Find("Bg/Logo");
		logoImg.transform.localScale = Vector3.one * 0.3f;
		contentText = base.transform.Find("Bg/Content").GetComponent<Image>();
		maskImg = base.transform.Find("Mask").GetComponent<Image>();
		contentText.sprite = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? chineseContentSprites : englishContentSprites);
	}

	private void StartOpenScene()
	{
		maskImg.color = Color.clear;
		contentText.color = new Color(1f, 1f, 1f, 0f);
		StartCoroutine(StartOpenScene_IE());
	}

	private IEnumerator StartOpenScene_IE()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("Logo");
		yield return new WaitForSeconds(0.5f);
		yield return new WaitForSeconds(1f);
		logoImg.DOScale(0.2f, 1.5f).SetEase(Ease.InCubic);
		yield return new WaitForSeconds(1.8f);
		contentText.DOColor(Color.white, 1f);
		yield return new WaitForSeconds(2.2f);
		maskImg.DOColor(Color.black, 0.5f).OnComplete(delegate
		{
			SingletonDontDestroy<Game>.Instance.SwitchScene(1);
		});
	}
}
