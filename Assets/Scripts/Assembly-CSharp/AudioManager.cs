using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonDontDestroy<AudioManager>
{
	private Transform musicRoot;

	private Transform soundRoot;

	private HashSet<string> currentFramePlayingSoundClps = new HashSet<string>();

	private Queue<AudioController> allSoundPool = new Queue<AudioController>();

	private Queue<AudioController> allBgmPool = new Queue<AudioController>();

	private AudioController currentBgmAudio;

	private string MainBGMName;

	private bool isStartCurrentFramePlayingClearCor;

	private Dictionary<string, AudioController> allPlayingLoopSounds = new Dictionary<string, AudioController>();

	private HashSet<AudioSource> allRegistMusicAudios = new HashSet<AudioSource>();

	private HashSet<AudioSource> allRegistSoundAudios = new HashSet<AudioSource>();

	protected override void Awake()
	{
		base.Awake();
		if (!(SingletonDontDestroy<AudioManager>.Instance != this))
		{
			GameObject gameObject = new GameObject("MusicRoot");
			gameObject.transform.SetParent(base.transform);
			musicRoot = gameObject.transform;
			GameObject gameObject2 = new GameObject("SoundRoot");
			gameObject2.transform.SetParent(base.transform);
			soundRoot = gameObject2.transform;
		}
	}

	public AudioController GetUnuseSoundAudioController()
	{
		if (allSoundPool.Count > 0)
		{
			AudioController audioController = allSoundPool.Dequeue();
			audioController.gameObject.SetActive(value: true);
			return audioController;
		}
		GameObject obj = new GameObject("SoundController");
		obj.transform.SetParent(soundRoot);
		obj.SetActive(value: true);
		AudioController audioController2 = obj.AddComponent<AudioController>();
		audioController2.audioType = AudioType.Sound;
		return audioController2;
	}

	public void PlaySound(string soundName)
	{
		if (currentFramePlayingSoundClps.Contains(soundName))
		{
			return;
		}
		AudioClip clip = SingletonDontDestroy<ResourceManager>.Instance.LoadSound(soundName);
		if (allSoundPool.Count > 0)
		{
			AudioController audioController = allSoundPool.Dequeue();
			audioController.gameObject.SetActive(value: true);
			audioController.PlayAudio(clip, isLoop: false);
			currentFramePlayingSoundClps.Add(soundName);
			if (!isStartCurrentFramePlayingClearCor)
			{
				isStartCurrentFramePlayingClearCor = true;
				StartCoroutine(CurrentFramePlayeringClear_IE());
			}
			return;
		}
		GameObject obj = new GameObject("SoundController");
		obj.transform.SetParent(soundRoot);
		obj.SetActive(value: false);
		AudioController audioController2 = obj.AddComponent<AudioController>();
		audioController2.audioType = AudioType.Sound;
		obj.SetActive(value: true);
		audioController2.PlayAudio(clip, isLoop: false);
		currentFramePlayingSoundClps.Add(soundName);
		if (!isStartCurrentFramePlayingClearCor)
		{
			isStartCurrentFramePlayingClearCor = true;
			StartCoroutine(CurrentFramePlayeringClear_IE());
		}
	}

	public void PlaySound(string soundName, float pitch)
	{
		if (currentFramePlayingSoundClps.Contains(soundName))
		{
			return;
		}
		AudioClip clip = SingletonDontDestroy<ResourceManager>.Instance.LoadSound(soundName);
		if (allSoundPool.Count > 0)
		{
			AudioController audioController = allSoundPool.Dequeue();
			audioController.gameObject.SetActive(value: true);
			audioController.PlayAudio(clip, isLoop: false);
			currentFramePlayingSoundClps.Add(soundName);
			if (!isStartCurrentFramePlayingClearCor)
			{
				isStartCurrentFramePlayingClearCor = true;
				StartCoroutine(CurrentFramePlayeringClear_IE());
			}
			return;
		}
		GameObject obj = new GameObject("SoundController");
		obj.SetActive(value: false);
		obj.transform.SetParent(soundRoot);
		AudioController audioController2 = obj.AddComponent<AudioController>();
		audioController2.audioType = AudioType.Sound;
		obj.SetActive(value: true);
		audioController2.PlayAudio(clip, isLoop: false, pitch);
		currentFramePlayingSoundClps.Add(soundName);
		if (!isStartCurrentFramePlayingClearCor)
		{
			isStartCurrentFramePlayingClearCor = true;
			StartCoroutine(CurrentFramePlayeringClear_IE());
		}
	}

	public void PlayerSound_Loop(string soundName)
	{
		if (allPlayingLoopSounds.ContainsKey(soundName))
		{
			return;
		}
		AudioClip clip = SingletonDontDestroy<ResourceManager>.Instance.LoadSound(soundName);
		if (allSoundPool.Count > 0)
		{
			AudioController audioController = allSoundPool.Dequeue();
			audioController.gameObject.SetActive(value: true);
			audioController.PlayAudio(clip, isLoop: true);
			currentFramePlayingSoundClps.Add(soundName);
			if (!isStartCurrentFramePlayingClearCor)
			{
				isStartCurrentFramePlayingClearCor = true;
				StartCoroutine(CurrentFramePlayeringClear_IE());
			}
			allPlayingLoopSounds[soundName] = audioController;
		}
		else
		{
			GameObject obj = new GameObject("SoundController");
			obj.SetActive(value: false);
			obj.transform.SetParent(soundRoot);
			AudioController audioController2 = obj.AddComponent<AudioController>();
			audioController2.audioType = AudioType.Sound;
			obj.SetActive(value: true);
			audioController2.PlayAudio(clip, isLoop: true);
			allPlayingLoopSounds[soundName] = audioController2;
		}
	}

	public void StopLoopSound(string soundName)
	{
		if (allPlayingLoopSounds.TryGetValue(soundName, out var value))
		{
			value.PauseAudio();
			RecycleAudioController(value);
			allPlayingLoopSounds.Remove(soundName);
		}
	}

	public AudioController GetLoopSoundAudioController(string soundName)
	{
		if (allPlayingLoopSounds.TryGetValue(soundName, out var value))
		{
			return value;
		}
		return null;
	}

	private IEnumerator CurrentFramePlayeringClear_IE()
	{
		yield return null;
		currentFramePlayingSoundClps.Clear();
		isStartCurrentFramePlayingClearCor = false;
	}

	private void PlayBGM(string bgmName)
	{
		if (!bgmName.IsNullOrEmpty())
		{
			SingletonDontDestroy<ResourceManager>.Instance.LoadBgmAsync(bgmName, SwitchBgm);
		}
	}

	public void PlayMainBGM(string bgmName, bool isReplaceMainBgm = true)
	{
		if (isReplaceMainBgm)
		{
			MainBGMName = bgmName;
		}
		PlayBGM(bgmName);
	}

	public void PlayMainBGMNoLoop(string bgmName)
	{
		SingletonDontDestroy<ResourceManager>.Instance.LoadBgmAsync(bgmName, SwitchBgmNoLoop);
	}

	public void SetMainBGM(string bgmName)
	{
		MainBGMName = bgmName;
	}

	public void PauseMainBGM()
	{
		if (currentBgmAudio != null)
		{
			currentBgmAudio.FadeRecycle();
			currentBgmAudio = null;
		}
	}

	public void RecoveryToMainBGM()
	{
		PlayBGM(MainBGMName);
	}

	private void SwitchBgm(AudioClip clip)
	{
		if (!currentBgmAudio.IsNull())
		{
			currentBgmAudio.FadeRecycle();
		}
		currentBgmAudio = GetMusicController();
		currentBgmAudio.PlayerAudioWithFade(clip, SingletonDontDestroy<SettingManager>.Instance.MusicVolume, isLoop: true);
	}

	private void SwitchBgmNoLoop(AudioClip clip)
	{
		if (!currentBgmAudio.IsNull())
		{
			currentBgmAudio.FadeRecycle();
		}
		currentBgmAudio = GetMusicController();
		currentBgmAudio.PlayerAudioWithFade(clip, SingletonDontDestroy<SettingManager>.Instance.MusicVolume, isLoop: false);
	}

	private AudioController GetMusicController()
	{
		AudioController audioController = null;
		if (allBgmPool.Count > 0)
		{
			audioController = allBgmPool.Dequeue();
			audioController.gameObject.SetActive(value: true);
		}
		else
		{
			GameObject obj = new GameObject("MusicController");
			obj.SetActive(value: false);
			obj.transform.SetParent(musicRoot);
			AudioController audioController2 = obj.AddComponent<AudioController>();
			audioController2.audioType = AudioType.Music;
			obj.SetActive(value: true);
			audioController = audioController2;
		}
		return audioController;
	}

	public void RecycleAudioController(AudioController ctrl)
	{
		ctrl.ResetAudio();
		ctrl.gameObject.SetActive(value: false);
		switch (ctrl.audioType)
		{
		case AudioType.Music:
			allBgmPool.Enqueue(ctrl);
			break;
		case AudioType.Sound:
			allSoundPool.Enqueue(ctrl);
			break;
		}
	}

	public void RegisterMusicAudio(AudioSource _source)
	{
		if (!allRegistMusicAudios.Add(_source))
		{
			Debug.LogWarning("The music audio you wanna regist ever registed...");
		}
		else
		{
			_source.volume = SingletonDontDestroy<SettingManager>.Instance.MusicVolume;
		}
	}

	public void UnregisterMusicAudio(AudioSource _source)
	{
		if (!allRegistMusicAudios.Remove(_source))
		{
			Debug.LogWarning("The music audio you wanna unregist never registed before...");
		}
	}

	public void RegisterSoundAudio(AudioSource _source)
	{
		if (!allRegistSoundAudios.Add(_source))
		{
			Debug.LogWarning("The sound audio you wanna regist ever registed...");
		}
		else
		{
			_source.volume = SingletonDontDestroy<SettingManager>.Instance.SoundVolume;
		}
	}

	public void UnregisterSoundAudio(AudioSource _source)
	{
		if (!allRegistSoundAudios.Remove(_source))
		{
			Debug.LogWarning("The sound audio you wanna unregist never resgistd before...");
		}
	}

	public void ChangeMusicVolume(float volume)
	{
		foreach (AudioSource allRegistMusicAudio in allRegistMusicAudios)
		{
			allRegistMusicAudio.volume = volume;
		}
	}

	public void ChangeSoundVolume(float volume)
	{
		foreach (AudioSource allRegistSoundAudio in allRegistSoundAudios)
		{
			allRegistSoundAudio.volume = volume;
		}
	}
}
