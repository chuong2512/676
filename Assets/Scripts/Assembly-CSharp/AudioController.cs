using DG.Tweening;
using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioType audioType;

	private AudioSource m_AudioSource;

	private Tween fadeTween;

	public AudioSource M_AudioSource => m_AudioSource;

	public bool IsPrivate { get; set; }

	public bool IsPlaying => m_AudioSource.isPlaying;

	private void Awake()
	{
		m_AudioSource = base.gameObject.AddComponent<AudioSource>();
		m_AudioSource.playOnAwake = false;
	}

	private void OnEnable()
	{
		switch (audioType)
		{
		case AudioType.Music:
			SingletonDontDestroy<AudioManager>.Instance.RegisterMusicAudio(m_AudioSource);
			break;
		case AudioType.Sound:
			SingletonDontDestroy<AudioManager>.Instance.RegisterSoundAudio(m_AudioSource);
			break;
		}
	}

	private void OnDisable()
	{
		switch (audioType)
		{
		case AudioType.Music:
			SingletonDontDestroy<AudioManager>.Instance.UnregisterMusicAudio(m_AudioSource);
			break;
		case AudioType.Sound:
			SingletonDontDestroy<AudioManager>.Instance.UnregisterSoundAudio(m_AudioSource);
			break;
		}
	}

	private void Update()
	{
		if (!IsPrivate && !IsPlaying && audioType == AudioType.Sound)
		{
			SingletonDontDestroy<AudioManager>.Instance.RecycleAudioController(this);
		}
	}

	public void PlayAudio(AudioClip clip, bool isLoop)
	{
		if (fadeTween != null && fadeTween.IsActive())
		{
			fadeTween.Kill();
		}
		m_AudioSource.clip = clip;
		m_AudioSource.Play();
		m_AudioSource.loop = isLoop;
	}

	public void PlayAudio(AudioClip clip, bool isLoop, float pitch)
	{
		m_AudioSource.pitch = pitch;
		PlayAudio(clip, isLoop);
	}

	public void PauseAudio()
	{
		m_AudioSource.Pause();
	}

	public void PlayerAudioWithFade(AudioClip clip, float targetVolume, bool isLoop)
	{
		if (fadeTween != null && fadeTween.IsActive())
		{
			fadeTween.Kill();
		}
		m_AudioSource.volume = 0f;
		m_AudioSource.clip = clip;
		m_AudioSource.Play();
		m_AudioSource.loop = isLoop;
		fadeTween = m_AudioSource.DOFade(targetVolume, 0.4f);
	}

	public void FadeRecycle()
	{
		m_AudioSource.DOFade(0f, 0.4f).OnComplete(delegate
		{
			SingletonDontDestroy<AudioManager>.Instance.RecycleAudioController(this);
		});
	}

	public void ResetAudio()
	{
		m_AudioSource.clip = null;
		m_AudioSource.pitch = 1f;
	}
}
