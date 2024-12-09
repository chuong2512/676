using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagCardDesUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private Transform keyctrlRoot;

	private RectTransform keyctrlRectRoot;

	private Queue<KeyCtrl> allKeyCtrlsPool = new Queue<KeyCtrl>();

	private List<KeyCtrl> allShowingKeyCtrl = new List<KeyCtrl>();

	private UsualWithDesCardInfo cardInfo;

	private Button functionBtn;

	private Text functionBtnText;

	public override string UIViewName => "BagCardDesUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拾取");
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllKeyCtrl();
		functionBtn.onClick.RemoveAllListeners();
		functionBtn.gameObject.SetActive(value: false);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Bag Card Des UI...");
	}

	public override void OnSpawnUI()
	{
		keyctrlRoot = base.transform.Find("Bg/KeyRoot");
		keyctrlRectRoot = keyctrlRoot.GetComponent<RectTransform>();
		cardInfo = base.transform.Find("Bg/UsualWithDesBigCard").GetComponent<UsualWithDesCardInfo>();
		functionBtn = base.transform.Find("Bg/FunctionBtn").GetComponent<Button>();
		functionBtnText = functionBtn.transform.Find("FunctionName").GetComponent<Text>();
	}

	public void ShowBigCard(string cardCode)
	{
		UsualCard usualCard = FactoryManager.GetUsualCard(cardCode);
		cardInfo.LoadCard(cardCode);
		List<KeyValuePair> keyDescription = usualCard.GetKeyDescription();
		if (keyDescription.IsNull())
		{
			return;
		}
		foreach (KeyValuePair item in keyDescription)
		{
			KeyCtrl keyCtrl = GetKeyCtrl();
			keyCtrl.LoadKey(item.key.LocalizeText(), item.value.LocalizeText());
			allShowingKeyCtrl.Add(keyCtrl);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(keyctrlRectRoot);
	}

	public void ShowBigCard(string cardCode, string btnName, Action callback, bool isInteractive)
	{
		ShowBigCard(cardCode);
		functionBtn.gameObject.SetActive(value: true);
		functionBtnText.text = btnName;
		functionBtn.onClick.AddListener(delegate
		{
			callback?.Invoke();
		});
		functionBtn.interactable = isInteractive;
	}

	private KeyCtrl GetKeyCtrl()
	{
		if (allKeyCtrlsPool.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyCtrlsPool.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", keyctrlRoot).GetComponent<KeyCtrl>();
	}

	private void RecycleAllKeyCtrl()
	{
		if (allShowingKeyCtrl.Count > 0)
		{
			for (int i = 0; i < allShowingKeyCtrl.Count; i++)
			{
				KeyCtrl keyCtrl = allShowingKeyCtrl[i];
				keyCtrl.gameObject.SetActive(value: false);
				allKeyCtrlsPool.Enqueue(keyCtrl);
			}
			allShowingKeyCtrl.Clear();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
