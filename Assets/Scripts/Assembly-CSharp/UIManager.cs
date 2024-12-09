using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonDontDestroy<UIManager>
{
	public const string UIBackgroundLayer = "BackgroundLayer";

	public const string UIDefaultLayer = "DefaultLayer";

	public const string UINormalLayer = "NormalLayer";

	public const string UITipsLayer = "TipsLayer";

	public const string OutGameLayer = "OutGameLayer";

	public const string UIPrefabAssetPath = "UI";

	private Dictionary<string, Transform> LayersNameToTrasnform = new Dictionary<string, Transform>();

	private Dictionary<string, UIView> allShowingUI = new Dictionary<string, UIView>();

	private Dictionary<string, UIView> allHideUI = new Dictionary<string, UIView>();

	private bool isInitialized;

	private static readonly int WidthBlackRate = Shader.PropertyToID("_WidthBlackRate");

	private static readonly int HeightBlackRate = Shader.PropertyToID("_HeightBlackRate");

	public static float WorldScale;

	public static float MaskY;

	public static float ScreenLocalHalfHeight;

	public static float ScreenLocalHalfWidth;

	public float ScaleRate => 25f / 27f;

	protected override void Awake()
	{
		base.Awake();
		if (!isInitialized && SingletonDontDestroy<UIManager>.Instance == this)
		{
			InitUIManager();
		}
	}

	private void InitUIManager()
	{
		LayersNameToTrasnform["BackgroundLayer"] = base.transform.Find("Background");
		LayersNameToTrasnform["DefaultLayer"] = base.transform.Find("Default");
		LayersNameToTrasnform["NormalLayer"] = base.transform.Find("Normal");
		LayersNameToTrasnform["TipsLayer"] = base.transform.Find("Tips");
		LayersNameToTrasnform["OutGameLayer"] = base.transform.Find("OutGame");
		ScreenCalculateAndSelfAdapation();
		isInitialized = true;
		EventManager.RegisterPermanentEvent(EventEnum.E_ScreenResolutionChanged, OnScreenResolutionChanged);
	}

	private void OnScreenResolutionChanged(EventData data)
	{
		ScreenCalculateAndSelfAdapation();
	}

	private void ScreenCalculateAndSelfAdapation()
	{
		Vector2 vector = new Vector2(Screen.width, Screen.height);
		Material material = base.transform.Find("UIBlackMask").GetComponent<Image>().material;
		float num = vector.x / vector.y;
		float num2 = 1.77777779f;
		float value = 0f;
		float num3 = 0f;
		CanvasScaler component = GetComponent<CanvasScaler>();
		GetComponent<Canvas>().worldCamera = SingletonDontDestroy<CameraController>.Instance.MainCamera;
		WorldScale = 1f;
		MaskY = 0f;
		if (num - num2 > 0.01f)
		{
			component.matchWidthOrHeight = 1f;
			value = (num - num2) / (2f * num);
		}
		else if (num - num2 < -0.01f)
		{
			component.matchWidthOrHeight = 0f;
			num3 = (1f / num - 1f / num2) / (2f / num);
			WorldScale = 1f - num3 * 2f;
			MaskY = num3;
		}
		material.SetFloat(WidthBlackRate, value);
		material.SetFloat(HeightBlackRate, num3);
		ScreenLocalHalfHeight = 540f;
		ScreenLocalHalfWidth = 960f;
	}

	public void DestoryAllView()
	{
		if (allShowingUI.Count > 0)
		{
			foreach (KeyValuePair<string, UIView> item in allShowingUI)
			{
				Object.Destroy(item.Value.gameObject);
			}
			allShowingUI.Clear();
		}
		if (allHideUI.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, UIView> item2 in allHideUI)
		{
			Object.Destroy(item2.Value.gameObject);
		}
		allHideUI.Clear();
	}

	public void DestoryView(string uiName)
	{
		UIView value = null;
		if (allShowingUI.TryGetValue(uiName, out value))
		{
			Object.Destroy(value.gameObject);
			allShowingUI.Remove(uiName);
		}
		else if (allHideUI.TryGetValue(uiName, out value))
		{
			Object.Destroy(value.gameObject);
			allHideUI.Remove(uiName);
		}
	}

	public void ReInitAllUI()
	{
		foreach (KeyValuePair<string, UIView> item in allHideUI)
		{
			item.Value.ReInitUI();
		}
	}

	public UIView ShowView(string uiName, params object[] objs)
	{
		if (allShowingUI.TryGetValue(uiName, out var value))
		{
			value.transform.SetAsLastSibling();
			return value;
		}
		UIView value2 = null;
		if (allHideUI.TryGetValue(uiName, out value2))
		{
			allShowingUI[uiName] = value2;
			allHideUI.Remove(uiName);
			value2.transform.SetAsLastSibling();
			value2.ShowView(objs);
			BroadcastViewShow(uiName);
			return value2;
		}
		GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace(uiName, "UI", base.transform, isCache: false);
		if (gameObject != null)
		{
			value2 = gameObject.GetComponent<UIView>();
			value2.OnSpawnUI();
			value2.transform.SetParent(LayersNameToTrasnform[value2.UILayerName]);
			value2.transform.SetAsLastSibling();
			value2.transform.localPosition = Vector3.zero;
			allShowingUI[uiName] = value2;
			value2.ShowView(objs);
			BroadcastViewShow(uiName);
			return value2;
		}
		Debug.LogError("Can not load ui, cause ui prefab not exist : " + uiName);
		return null;
	}

	private void BroadcastViewShow(string UIViewName)
	{
		EventManager.BroadcastEvent(EventEnum.E_ShowUIView, new SimpleEventData
		{
			stringValue = UIViewName
		});
	}

	public void HideView(UIView view)
	{
		view.HideView();
		allHideUI[view.UIViewName] = view;
		allShowingUI.Remove(view.UIViewName);
	}

	public void HideView(string uiName)
	{
		UIView value = null;
		if (allShowingUI.TryGetValue(uiName, out value))
		{
			HideView(value);
		}
	}

	public void RemoveUIFromShowingList(string uiName)
	{
		allShowingUI.Remove(uiName);
	}

	public void HideAllShowingView()
	{
		if (allShowingUI.Count <= 0)
		{
			return;
		}
		List<string> list = new List<string>(allShowingUI.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			UIView value = null;
			if (allShowingUI.TryGetValue(list[i], out value) && value.AutoHideBySwitchScene)
			{
				HideView(value);
			}
		}
	}

	public UIView GetView(string uiName)
	{
		UIView value = null;
		if (allShowingUI.TryGetValue(uiName, out value))
		{
			return value;
		}
		return null;
	}

	public UIView ForceGetView(string uiName)
	{
		UIView value = null;
		if (allShowingUI.TryGetValue(uiName, out value))
		{
			return value;
		}
		if (allHideUI.TryGetValue(uiName, out value))
		{
			return value;
		}
		return null;
	}
}
