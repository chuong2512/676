using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
//	public VideoPlayer VideoPlayer;

	public Text lyricTxt;

	public RawImage img;

	public List<LyricStruct> lyrics = new List<LyricStruct>();

	public List<LyricStruct> lyrics_En = new List<LyricStruct>();

	public Image circleImg;

	private float previousTime;

	private float counter;

	private AudioController _audioController;

	private const float PressTime = 1.5f;

	public void StartAnim()
	{
	//	VideoPlayer.targetCamera = Camera.main;
		previousTime = 0f;
		img.color = Color.clear;
		StartCoroutine(LyricCo());
		lyricTxt.WithCol(0f);
	}

	private void OnDisable()
	{
		_audioController.PauseAudio();
		SingletonDontDestroy<AudioManager>.Instance.RecycleAudioController(_audioController);
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Escape))
		{
			EscapeCounter();
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			StopEscapeCounter();
		}
	}

	private void EscapeCounter()
	{
		counter += Time.deltaTime;
		if (counter < 1.5f)
		{
			circleImg.fillAmount = counter / 1.5f;
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("CGUI");
		}
	}

	private void StopEscapeCounter()
	{
		counter = 0f;
		circleImg.fillAmount = 0f;
	}

	private IEnumerator LyricCo()
	{
		//while (!VideoPlayer.isPlaying)
		//{
		//	yield return null;
		//}
		List<LyricStruct> theLyrics = ((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? lyrics : lyrics_En);
		img.DOColor(Color.white, 0.3f);
		AudioClip clip = SingletonDontDestroy<ResourceManager>.Instance.LoadSound((SingletonDontDestroy<SettingManager>.Instance.Language == 0) ? "CG混音ZH" : "CG混音EN");
		_audioController = SingletonDontDestroy<AudioManager>.Instance.GetUnuseSoundAudioController();
		_audioController.PlayAudio(clip, isLoop: false);
		for (int i = 0; i < theLyrics.Count; i++)
		{
			string value = theLyrics[i].key.LocalizeText();
			if (!value.IsNullOrEmpty())
			{
				yield return new WaitForSeconds(((float)theLyrics[i].startingMiliSecs - previousTime) / 1000f);
				previousTime = theLyrics[i].startingMiliSecs;
				lyricTxt.text = value;
				lyricTxt.DOFade(1f, 0.1f);
				yield return new WaitForSeconds(((float)theLyrics[i].fadingMiliSecs - previousTime) / 1000f);
				previousTime = theLyrics[i].fadingMiliSecs;
				lyricTxt.DOFade(0f, 0.1f);
			}
		}
		//while (VideoPlayer.isPlaying)
		//{
		//	yield return null;
		//}
		SingletonDontDestroy<UIManager>.Instance.HideView("CGUI");
	}
}
