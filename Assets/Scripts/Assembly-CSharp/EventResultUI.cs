using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventResultUI : UIView, IPointerClickHandler, IEventSystemHandler
{
	private Action ClickHandler;

	private bool isPanelMoving;

	private RectTransform MaskRect;

	private UsualWithDesCardInfo _cardInfo;

	private SingleEquipDesCtrl _singleEquipDesCtrl;

	private Queue<string> currentShowingEquipQueue = new Queue<string>();

	private Queue<KeyCtrl> allEquipKeyPools = new Queue<KeyCtrl>();

	private Dictionary<string, KeyCtrl> allShowingKeyCtrls = new Dictionary<string, KeyCtrl>();

	private Transform equipKeyRoot;

	private RectTransform equipKeyRectRoot;

	private SkillDesPanelCtrl _skillDesPanelCtrl;

	public override string UIViewName => "EventResultUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		MaskRect = base.transform.Find("BgMask/Mask").GetComponent<RectTransform>();
		InitCardPanel();
		InitSingleEquipDes();
		InitSkillPanel();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!isPanelMoving)
		{
			ClickHandler?.Invoke();
		}
	}

	private void MoveParticalToBagIcon()
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_general_trail_jinengzhuangbei");
		vfxBase.Play();
		Vector3 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.ViewportToWorldPoint(Vector2.one * 0.5f);
		vector.z = -5f;
		vfxBase.transform.position = vector;
		RoomUI roomUi = SingletonDontDestroy<UIManager>.Instance.GetView("RoomUI") as RoomUI;
		Vector3 endPos = roomUi.bagBtn.transform.position;
		endPos.z = -5f;
		float x = Mathf.Clamp(vector.x, endPos.x, UnityEngine.Random.Range(0.3f, 0.7f));
		float y = (vector.y + endPos.y) / 2f + UnityEngine.Random.Range(4f, 8f);
		vfxBase.transform.TransformMoveByBezier(vector, new Vector3(x, y, -5f), endPos, 0.7f, delegate
		{
			roomUi.BagBtnHint();
			vfxBase.Recycle();
			VfxBase vfxBase2 = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_wupinhuode_hit");
			vfxBase2.transform.position = endPos;
			vfxBase2.Play();
		});
	}

	private void InitCardPanel()
	{
		_cardInfo = base.transform.Find("BgMask/Mask/UsualWithDesBigCard").GetComponent<UsualWithDesCardInfo>();
	}

	public void ShowCard(string cardCode)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		_cardInfo.gameObject.SetActive(value: true);
		_cardInfo.LoadCard(cardCode);
		ClickHandler = HideCardPanel;
		_cardInfo.transform.localScale = Vector3.zero;
		isPanelMoving = true;
		_cardInfo.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			isPanelMoving = false;
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(MaskRect);
	}

	private void HideCardPanel()
	{
		isPanelMoving = true;
		_cardInfo.transform.DOScale(0f, 0.3f).OnComplete(delegate
		{
			isPanelMoving = false;
			_cardInfo.gameObject.SetActive(value: false);
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		});
		MoveParticalToBagIcon();
	}

	private void InitSingleEquipDes()
	{
		_singleEquipDesCtrl = base.transform.Find("BgMask/Mask/SingleEquipDes").GetComponent<SingleEquipDesCtrl>();
		equipKeyRoot = _singleEquipDesCtrl.transform.Find("TitleRoot/KeyRoot");
		equipKeyRectRoot = equipKeyRoot.GetComponent<RectTransform>();
	}

	public void ShowEquip(string equipCode)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用装备窗口");
		_singleEquipDesCtrl.gameObject.SetActive(value: true);
		_singleEquipDesCtrl.LoadEquip(equipCode, isEquiped: false, AddEquipKeys);
		ClickHandler = HideEquip;
		_singleEquipDesCtrl.transform.localScale = Vector3.zero;
		isPanelMoving = true;
		_singleEquipDesCtrl.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			isPanelMoving = false;
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(MaskRect);
	}

	public void ShowEquip(List<string> equips)
	{
		if (equips != null && equips.Count > 0)
		{
			ShowEquip(equips[0]);
			for (int i = 1; i < equips.Count; i++)
			{
				currentShowingEquipQueue.Enqueue(equips[i]);
			}
		}
	}

	private void HideEquip()
	{
		if (currentShowingEquipQueue.Count > 0)
		{
			RecycleAllEquipKeys();
			_singleEquipDesCtrl.LoadEquip(currentShowingEquipQueue.Dequeue(), isEquiped: false, AddEquipKeys);
			return;
		}
		isPanelMoving = true;
		_singleEquipDesCtrl.transform.DOScale(0f, 0.3f).OnComplete(delegate
		{
			isPanelMoving = false;
			_singleEquipDesCtrl.gameObject.SetActive(value: false);
			RecycleAllEquipKeys();
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		});
		MoveParticalToBagIcon();
	}

	public void AddEquipKeys(List<KeyValuePair> keys, bool isKey)
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
		LayoutRebuilder.ForceRebuildLayoutImmediate(equipKeyRectRoot);
	}

	private KeyCtrl GetKeyCtrl()
	{
		if (allEquipKeyPools.Count > 0)
		{
			KeyCtrl keyCtrl = allEquipKeyPools.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", equipKeyRoot).GetComponent<KeyCtrl>();
	}

	private void RecycleAllEquipKeys()
	{
		if (allShowingKeyCtrls.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<string, KeyCtrl> allShowingKeyCtrl in allShowingKeyCtrls)
		{
			allShowingKeyCtrl.Value.gameObject.SetActive(value: false);
			allEquipKeyPools.Enqueue(allShowingKeyCtrl.Value);
		}
		allShowingKeyCtrls.Clear();
	}

	private void InitSkillPanel()
	{
		_skillDesPanelCtrl = base.transform.Find("BgMask/Mask/SkillDesPanel").GetComponent<SkillDesPanelCtrl>();
	}

	public void ShowSkillDes(string skillCode)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("技能详情");
		_skillDesPanelCtrl.gameObject.SetActive(value: true);
		_skillDesPanelCtrl.ShowDescription(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode, isOnBattle: false);
		ClickHandler = HideSkillDes;
		_skillDesPanelCtrl.transform.localScale = Vector3.zero;
		isPanelMoving = true;
		_skillDesPanelCtrl.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			isPanelMoving = false;
		});
		LayoutRebuilder.ForceRebuildLayoutImmediate(MaskRect);
	}

	private void HideSkillDes()
	{
		isPanelMoving = true;
		_skillDesPanelCtrl.transform.DOScale(0f, 0.3f).OnComplete(delegate
		{
			isPanelMoving = false;
			_skillDesPanelCtrl.gameObject.SetActive(value: false);
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		});
		MoveParticalToBagIcon();
	}
}
