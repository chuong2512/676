using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDesUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private Transform keyRoot;

	private RectTransform keyRectRoot;

	private Transform equipDesRoot;

	private Queue<KeyCtrl> allKeyCtrlPools = new Queue<KeyCtrl>();

	private Dictionary<string, KeyCtrl> allShowingKeyCtrls = new Dictionary<string, KeyCtrl>();

	private Queue<SingleEquipDesCtrl> allSingleEquipDesCtrlPool = new Queue<SingleEquipDesCtrl>(2);

	private Dictionary<string, SingleEquipDesCtrl> allShowingDesCtrls = new Dictionary<string, SingleEquipDesCtrl>(2);

	public override string UIViewName => "EquipDesUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用装备窗口");
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllKeys();
		RecycleAllEquipDes();
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用装备窗口关闭");
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		equipDesRoot = base.transform.Find("BgMask");
		keyRoot = base.transform.Find("BgMask/KeyRoot");
		keyRectRoot = keyRoot.GetComponent<RectTransform>();
	}

	public void LoadEquipment(string equipCode, int index, bool isEquiped)
	{
		SingleEquipDesCtrl singleEquipDes = GetSingleEquipDes();
		singleEquipDes.transform.SetSiblingIndex(index);
		singleEquipDes.LoadEquip(equipCode, isEquiped, AddKeys);
		allShowingDesCtrls.Add(equipCode, singleEquipDes);
	}

	public void LoadEquipment(string equipCode, int index, string functionName, Action functionAction, bool isInteractive)
	{
		SingleEquipDesCtrl singleEquipDes = GetSingleEquipDes();
		singleEquipDes.transform.SetSiblingIndex(index);
		singleEquipDes.LoadEquip(equipCode, isEquiped: false, AddKeys, functionName, functionAction, isInteractive);
		allShowingDesCtrls.Add(equipCode, singleEquipDes);
	}

	public void LoadEquipment(string equipCode, int index, string btnName, bool isEquiped, Action functionAction, bool isInteractive)
	{
		SingleEquipDesCtrl singleEquipDes = GetSingleEquipDes();
		singleEquipDes.transform.SetSiblingIndex(index);
		singleEquipDes.LoadEquip(equipCode, isEquiped, AddKeys, btnName, functionAction, isInteractive);
		allShowingDesCtrls.Add(equipCode, singleEquipDes);
	}

	public void LoadContrastEquipment(string equipCode, string btnName, Action functionAction, bool isInteractive)
	{
		string equipCode2 = string.Empty;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		PlayerEquipment playerEquipment = Singleton<GameManager>.Instance.Player.PlayerEquipment;
		switch (equipmentCardAttr.EquipmentType)
		{
		case EquipmentType.Helmet:
			equipCode2 = playerEquipment.Helmet.CardCode;
			break;
		case EquipmentType.Breastplate:
			equipCode2 = playerEquipment.Breastplate.CardCode;
			break;
		case EquipmentType.Glove:
			equipCode2 = playerEquipment.Glove.CardCode;
			break;
		case EquipmentType.Ornament:
			equipCode2 = playerEquipment.Ornament.CardCode;
			break;
		case EquipmentType.MainHandWeapon:
			equipCode2 = playerEquipment.MainHandWeapon.CardCode;
			break;
		case EquipmentType.SupHandWeapon:
			equipCode2 = playerEquipment.SupHandWeapon.CardCode;
			break;
		case EquipmentType.Shoes:
			equipCode2 = playerEquipment.Shoes.CardCode;
			break;
		}
		LoadEquipment(equipCode2, 0, isEquiped: true);
		LoadEquipment(equipCode, 1, btnName, isEquiped: false, functionAction, isInteractive);
	}

	private SingleEquipDesCtrl GetSingleEquipDes()
	{
		if (allSingleEquipDesCtrlPool.Count > 0)
		{
			SingleEquipDesCtrl singleEquipDesCtrl = allSingleEquipDesCtrlPool.Dequeue();
			singleEquipDesCtrl.gameObject.SetActive(value: true);
			return singleEquipDesCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleEquipDes", "Prefabs", equipDesRoot).GetComponent<SingleEquipDesCtrl>();
	}

	private void RecycleAllEquipDes()
	{
		if (allShowingDesCtrls.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, SingleEquipDesCtrl> allShowingDesCtrl in allShowingDesCtrls)
		{
			allShowingDesCtrl.Value.gameObject.SetActive(value: false);
			allSingleEquipDesCtrlPool.Enqueue(allShowingDesCtrl.Value);
		}
		allShowingDesCtrls.Clear();
	}

	public void AddKeys(List<KeyValuePair> keys, bool isKey)
	{
		if (keys == null || keys.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair key2 in keys)
		{
			string key = (isKey ? key2.key.LocalizeText() : key2.key);
			string des = (isKey ? key2.value.LocalizeText() : key2.value);
			if (!allShowingKeyCtrls.ContainsKey(key))
			{
				KeyCtrl keyCtrl = GetKeyCtrl();
				keyCtrl.LoadKey(key, des);
				allShowingKeyCtrls.Add(key, keyCtrl);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(keyRectRoot);
	}

	private KeyCtrl GetKeyCtrl()
	{
		if (allKeyCtrlPools.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyCtrlPools.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", keyRoot).GetComponent<KeyCtrl>();
	}

	private void RecycleAllKeys()
	{
		if (allShowingKeyCtrls.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, KeyCtrl> allShowingKeyCtrl in allShowingKeyCtrls)
		{
			allShowingKeyCtrl.Value.gameObject.SetActive(value: false);
			allKeyCtrlPools.Enqueue(allShowingKeyCtrl.Value);
		}
		allShowingKeyCtrls.Clear();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
