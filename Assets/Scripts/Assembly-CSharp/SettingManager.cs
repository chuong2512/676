using System.IO;
using UnityEngine;

public class SettingManager : SingletonDontDestroy<SettingManager>
{
	private const string DataName = "Setting.json";

	public float MusicVolume { get; private set; }

	public float SoundVolume { get; private set; }

	public int Resolution_Width { get; private set; }

	public int Resolution_Height { get; private set; }

	public int MaxFrame { get; private set; }

	public int WindowType { get; private set; }

	public int Language { get; private set; }

	public void ChangeLanguage(int value)
	{
		Language = value;
	}

	public void ChangeWindowType(int value)
	{
		WindowType = value;
	}

	public void ChangeMaxFrame(int value)
	{
		MaxFrame = value;
	}

	public void ChangeResolution(int screenWidth, int screenHeight)
	{
		Resolution_Width = screenWidth;
		Resolution_Height = screenHeight;
	}

	public void ChangeSoundVolume(float volume)
	{
		SoundVolume = volume;
	}

	public void ChangeMusicVolume(float volume)
	{
		MusicVolume = volume;
	}

	public void LoadPlayerSetting()
	{
		string path = Path.Combine(Application.persistentDataPath, "Setting.json");
		if (File.Exists(path))
		{
			SettingData settingData = JsonUtility.FromJson<SettingData>(File.ReadAllText(path));
			MusicVolume = settingData.MusicVolume;
			SoundVolume = settingData.SoundVolume;
			Resolution_Width = settingData.Resolution_Width;
			Resolution_Height = settingData.Resolution_Height;
			MaxFrame = settingData.MaxFrame;
			WindowType = settingData.WindowType;
			Language = settingData.Language;
		}
		else
		{
			SetDefaultSetting();
			SaveCurrentSettingToSettingFile();
		}
	}

	public void SaveCurrentSettingToSettingFile()
	{
		string path = Path.Combine(Application.persistentDataPath, "Setting.json");
		SettingData obj = new SettingData
		{
			MusicVolume = MusicVolume,
			SoundVolume = SoundVolume,
			Resolution_Width = Resolution_Width,
			Resolution_Height = Resolution_Height,
			MaxFrame = MaxFrame,
			WindowType = WindowType,
			Language = Language
		};
		File.WriteAllText(path, JsonUtility.ToJson(obj, prettyPrint: true));
	}

	public void SetDefaultSetting()
	{
		MusicVolume = 0.5f;
		SoundVolume = 0.5f;
		Resolution_Width = Screen.currentResolution.width;
		Resolution_Height = Screen.currentResolution.height;
		MaxFrame = 1;
		WindowType = 0;
		Language = ((Application.systemLanguage != SystemLanguage.ChineseSimplified && Application.systemLanguage != SystemLanguage.ChineseTraditional) ? 1 : 0);
	}
}
