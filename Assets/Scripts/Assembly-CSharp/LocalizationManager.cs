using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalizationManager
{
	private static LocalizationManager _instance;

	private Dictionary<string, string> localizedText;

	public Dictionary<string, string> colorPallet = new Dictionary<string, string>();

	private const string missingTextString = "Not Found";

	public static LocalizationManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LocalizationManager();
			}
			return _instance;
		}
	}

	public void InitLocalizationManager(string fileName)
	{
		LoadLocalizedText(fileName);
	}

    

	public void LoadLocalizedText(string fileName)
	{
        string filePath = "data/" + fileName.Replace(".json", "");
        localizedText = new Dictionary<string, string>();
		colorPallet = new Dictionary<string, string>();
        Debug.Log($"Load file : {filePath}");
        TextAsset jsonTextAsset = Resources.Load<TextAsset>(filePath);
		LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(jsonTextAsset.text);
		for (int i = 0; i < localizationData.items.Count; i++)
		{
			string text = localizationData.items[i].key.ToUpper();
			if (localizedText.ContainsKey(text))
			{
				Debug.Log(text);
			}
			string text2 = localizationData.items[i].value;
			for (int j = 0; j < localizationData.colorPalette.Count; j++)
			{
				text2 = text2.Replace(localizationData.colorPalette[j].name, localizationData.colorPalette[j].color);
			}
			localizedText.Add(text, text2);
			foreach (ColorPaletteItem item in localizationData.colorPalette)
			{
				if (!colorPallet.ContainsKey(item.name))
				{
					colorPallet.Add(item.name, item.color);
				}
			}
		}
	}

	public string GetLocalizedValue(string key)
	{
		string result = key + "Not Found";
		string key2 = key.ToUpper();
		if (localizedText.ContainsKey(key2))
		{
			result = localizedText[key2];
		}
		return result;
	}
}
