using System;
using UnityEngine;

[Serializable]
public class SettingData
{
	public const float DefaultMusicVolume = 0.5f;

	public const float DefaultSoundVolume = 0.5f;

	public const int DefaultMaxFrame = 1;

	public const int DefaultWindowType = 0;

	public float MusicVolume;

	public float SoundVolume;

	public int Resolution_Width;

	public int Resolution_Height;

	public int MaxFrame;

	public int WindowType;

	public int Language;

	public SettingData()
	{
		MusicVolume = 0.5f;
		SoundVolume = 0.5f;
		Resolution_Width = 0;
		Resolution_Height = 0;
		MaxFrame = 1;
		WindowType = 0;
		Language = ((Application.systemLanguage != SystemLanguage.Chinese) ? 1 : 0);
	}
}
