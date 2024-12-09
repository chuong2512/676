using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : UIView
{
	private Image maskImg;

	private Image deathSceneImg;

	private Text deathTxt;

	private static readonly int Range = Shader.PropertyToID("_Range");

	private List<Tween> tweenList = new List<Tween>();

	public override string UIViewName => "DeathUI";

	public override string UILayerName => "OutGameLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		StartAnim((Action)objs[0], (Action)objs[1]);
	}

	private void StartAnim(Action act1, Action act2)
	{
		Reset();
		StopAllCoroutines();
		StartCoroutine(StartAnimCo(act1, act2));
	}

	private void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		maskImg.material.SetFloat(Range, 0.8f);
		OccupationData occupationData = DataManager.Instance.GetOccupationData(Singleton<GameManager>.Instance.Player.PlayerOccupation);
		deathSceneImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.DeathSceneImg, occupationData.DefaultSpritePath);
		deathSceneImg.WithCol(0f);
		deathTxt.WithCol(0f);
	}

	private IEnumerator StartAnimCo(Action act1, Action act2)
	{
		maskImg.material.DOFloat(0f, "_Range", 2f).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			act1?.Invoke();
		});
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("心跳");
		float musicVolume = SingletonDontDestroy<SettingManager>.Instance.MusicVolume;
		tweenList.Add(DOTween.To(() => musicVolume, delegate(float x)
		{
			musicVolume = x;
		}, 0f, 1.5f).OnUpdate(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.ChangeMusicVolume(musicVolume);
		}));
		float soundVolume = SingletonDontDestroy<SettingManager>.Instance.SoundVolume;
		tweenList.Add(DOTween.To(() => soundVolume, delegate(float x)
		{
			soundVolume = x;
		}, 0f, 1f).OnUpdate(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.ChangeSoundVolume(soundVolume);
		}));
		yield return new WaitForSeconds(2f);
		yield return new WaitForSeconds(0.1f);
		SingletonDontDestroy<AudioManager>.Instance.PlayMainBGMNoLoop("失败ME");
		musicVolume = 0f;
		tweenList.Add(DOTween.To(() => musicVolume, delegate(float x)
		{
			musicVolume = x;
		}, SingletonDontDestroy<SettingManager>.Instance.MusicVolume, 0.5f).OnUpdate(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.ChangeMusicVolume(musicVolume);
		}));
		tweenList.Add(deathSceneImg.DOFade(1f, 2f));
		yield return new WaitForSeconds(1f);
		tweenList.Add(deathTxt.DOFade(1f, 1f));
		yield return new WaitForSeconds(3f);
		SingletonDontDestroy<AudioManager>.Instance.ChangeSoundVolume(SingletonDontDestroy<SettingManager>.Instance.SoundVolume);
		tweenList.Add(deathSceneImg.DOFade(0f, 2f));
		tweenList.Add(deathTxt.DOFade(0f, 2f));
		yield return new WaitForSeconds(2f);
		act2?.Invoke();
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
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
		maskImg = base.transform.Find("Root/CloseEyeEffect").GetComponent<Image>();
		deathSceneImg = base.transform.Find("Root/DeathImg").GetComponent<Image>();
		deathTxt = deathSceneImg.transform.Find("DeathWords").GetComponent<Text>();
	}
}
