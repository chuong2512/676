using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager : SingletonDontDestroy<ResourceManager>
{
	private class BgmLoadData
	{
		public Action<AudioClip> Handler;
	}

	public const string CardImagePath = "Sprites/Cards";

	public const string SkillIconPath = "Sprites/SkillIcon";

	public const string EquipIconPath = "Sprites/Equipment";

	public const string SuitIconPath = "Sprites/Suit";

	public const string GiftIconPath = "Sprites/Gift";

	public const string PrefabsPath = "Prefabs";

	public const string EffectConfigPath = "EffectConfigScriObj";

	public const string EnemyEffectConfigPath = "EffectConfigScriObj/Enemy";

	public const string VfxPath = "Prefabs/VFX";

	public const string BGMPath = "BGM";

	public const string SoundPath = "Sounds";

	public const string MaterialPath = "Materials";

	public const string MonsterIlluPath = "Sprites/Monster";

	public const string PlotIlluPath = "Sprites/Plot";

	public const string GuideTipIlluPath = "Sprites/GuideTips";

	private Dictionary<string, GameObject> allPrefabCache = new Dictionary<string, GameObject>();

	private Dictionary<string, Sprite> allSpriteCache = new Dictionary<string, Sprite>();

	private Dictionary<string, ScriptableObject> allScriptObjectCache = new Dictionary<string, ScriptableObject>();

	private Dictionary<string, GameObject> allVfxCache = new Dictionary<string, GameObject>();

	private Dictionary<string, AudioClip> allLoadedAudioClips = new Dictionary<string, AudioClip>();

	private Dictionary<string, Queue<BgmLoadData>> allLoadingBgmData = new Dictionary<string, Queue<BgmLoadData>>();

	private Dictionary<string, Material> allMaterialsCache = new Dictionary<string, Material>();

	public void UnloadAllAssets()
	{
		allPrefabCache.Clear();
		allSpriteCache.Clear();
		allVfxCache.Clear();
		allScriptObjectCache.Clear();
		allLoadedAudioClips.Clear();
		allLoadingBgmData.Clear();
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public T LoadAsset<T>(string path) where T : UnityEngine.Object
	{
		T val = Resources.Load<T>(path);
		if ((UnityEngine.Object)val == (UnityEngine.Object)null)
		{
			Debug.LogError("Cannot load asset from resources file, assetPath : " + path);
		}
		return val;
	}

	public T LoadAsset<T>(string assetName, string assetPath) where T : UnityEngine.Object
	{
		T val = Resources.Load<T>(Path.Combine(assetPath, assetName));
		if ((UnityEngine.Object)val == (UnityEngine.Object)null)
		{
			Debug.Log($"<color=red>Cannot load asset from resources file, assetName : {assetName}, assetPath : {assetPath}</color>");
		}
		return val;
	}

	public GameObject LoadPrefab(string prefabName, string prefabePath, bool isCache)
	{
		GameObject value = null;
		if (!allPrefabCache.TryGetValue(prefabName, out value))
		{
			value = LoadAsset<GameObject>(prefabName, prefabePath);
			if (isCache)
			{
				allPrefabCache[prefabName] = value;
			}
		}
		return value;
	}

	public GameObject LoadPrefabInstace(string prefabName, string prefabePath, Transform parent, bool isCache = true)
	{
		return UnityEngine.Object.Instantiate(LoadPrefab(prefabName, prefabePath, isCache), parent);
	}

	public Sprite LoadSprite(string spriteName, string spritePath)
	{
		Sprite value = null;
		if (!allSpriteCache.TryGetValue(spriteName, out value))
		{
			value = LoadAsset<Sprite>(spriteName, spritePath);
			allSpriteCache[spriteName] = value;
		}
		return value;
	}

	public T LoadScriptObject<T>(string scrobjAssetName, string scrobjAssetPath) where T : ScriptableObject
	{
		if (allScriptObjectCache.TryGetValue(scrobjAssetName, out var value))
		{
			return (T)value;
		}
		T val = LoadAsset<T>(scrobjAssetName, scrobjAssetPath);
		allScriptObjectCache.Add(scrobjAssetName, val);
		return val;
	}

	public GameObject LoadVfxInstance(string vfxName, Transform root)
	{
		if (allVfxCache.TryGetValue(vfxName, out var value))
		{
			return UnityEngine.Object.Instantiate(value, root);
		}
		try
		{
			GameObject gameObject = LoadAsset<GameObject>(vfxName, "Prefabs/VFX");
			allVfxCache.Add(vfxName, gameObject);
			return UnityEngine.Object.Instantiate(gameObject, root);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message + "----" + vfxName);
			throw;
		}
	}

	public AudioClip LoadSound(string soundName)
	{
		if (allLoadedAudioClips.TryGetValue(soundName, out var value))
		{
			return value;
		}
		AudioClip audioClip = LoadAsset<AudioClip>(soundName, "Sounds");
		allLoadedAudioClips.Add(soundName, audioClip);
		return audioClip;
	}

	public void LoadBgmAsync(string bgmName, Action<AudioClip> handler)
	{
		if (handler.IsNull())
		{
			throw new NullReferenceException("Handler can not be null");
		}
		if (allLoadedAudioClips.TryGetValue(bgmName, out var value))
		{
			handler(value);
			return;
		}
		if (allLoadingBgmData.TryGetValue(bgmName, out var value2))
		{
			value2.Enqueue(new BgmLoadData
			{
				Handler = handler
			});
			return;
		}
		Queue<BgmLoadData> queue = new Queue<BgmLoadData>();
		queue.Enqueue(new BgmLoadData
		{
			Handler = handler
		});
		allLoadingBgmData.Add(bgmName, queue);
		StartCoroutine(LoadBgm_IE(bgmName));
	}

	private IEnumerator LoadBgm_IE(string bgmName)
	{
		string path = Path.Combine("BGM", bgmName);
		ResourceRequest request = Resources.LoadAsync(path);
		while (!request.isDone)
		{
			yield return null;
		}
		AudioClip audioClip = (AudioClip)request.asset;
		allLoadedAudioClips.Add(bgmName, audioClip);
		Queue<BgmLoadData> queue = allLoadingBgmData[bgmName];
		while (queue.Count > 0)
		{
			queue.Dequeue().Handler(audioClip);
		}
		allLoadingBgmData.Remove(bgmName);
	}

	public Material LoadMaterial(string materialName, string materialPath)
	{
		if (allMaterialsCache.TryGetValue(materialName, out var value))
		{
			return value;
		}
		Material material = LoadAsset<Material>(materialName, materialPath);
		allMaterialsCache.Add(materialName, material);
		return material;
	}
}
