using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemHoverHintUI : UIView
{
	private class AdjustHandler
	{
		public RectTransform TargetRectTransform;

		public RectTransform RectTransform;

		public RectTransform KeyRectTransform;

		public float xOffset;

		public Action AdjustLeftHandler;

		public Action AdjustRightHandler;

		public AdjustHandler(RectTransform targetRectTransform, RectTransform rectTransform, RectTransform keyRectTransform, float xOffset, Action adjustLeftHandler, Action adjustRightHandler)
		{
			KeyRectTransform = keyRectTransform;
			TargetRectTransform = targetRectTransform;
			this.xOffset = xOffset;
			RectTransform = rectTransform;
			AdjustLeftHandler = adjustLeftHandler;
			AdjustRightHandler = adjustRightHandler;
		}
	}

	private abstract class HoverHandler
	{
		protected ItemHoverHintUI parentPanel;

		protected HoverHandler(ItemHoverHintUI parentPanel)
		{
			this.parentPanel = parentPanel;
		}

		public abstract AdjustHandler HandleHoverInfo(HoverData data);

		public abstract void HandleHoverOver();
	}

	private class MonsterDescriptionHoverHintHandler : HoverHandler
	{
		private bool isInitMonsterDescriptionHoverHint;

		private MonsterDescriptionHoverHint _monsterDescriptionHoverHint;

		public MonsterDescriptionHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitMonsterDescriptionHoverHint()
		{
			if (!isInitMonsterDescriptionHoverHint)
			{
				isInitMonsterDescriptionHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("MonsterDescriptionHoverHint", "Prefabs", parentPanel.transform);
				_monsterDescriptionHoverHint = gameObject.GetComponent<MonsterDescriptionHoverHint>();
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			MonsterDescriptionHoverData monsterDescriptionHoverData;
			if ((monsterDescriptionHoverData = data as MonsterDescriptionHoverData) != null)
			{
				InitMonsterDescriptionHoverHint();
				_monsterDescriptionHoverHint.gameObject.SetActive(value: true);
				_monsterDescriptionHoverHint.LoadMonsterDescription(monsterDescriptionHoverData.NameStr, monsterDescriptionHoverData.DescriptionContentStr);
				return new AdjustHandler(data.TargetRectTransform, _monsterDescriptionHoverHint.m_RectTransform, null, 0f, null, null);
			}
			throw new InvalidCastException("Cannot cast data to MonsterDescriptionHoverData");
		}

		public override void HandleHoverOver()
		{
			_monsterDescriptionHoverHint.gameObject.SetActive(value: false);
		}
	}

	private class EquipDescriptionHoverHintHandler : HoverHandler
	{
		private const string NotUnlockedKey = "EquipNotUnlockedDesKey";

		private const string NotPurchasedKey = "EquipNotPurchasedDesKey";

		private const string NotUnlockedName = "???";

		private EquipDescriptionHoverHint _equipDescriptionHoverHint;

		private bool isInitEquipDescriptionHoverHint;

		public EquipDescriptionHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitEquipDescriptionHoverHint()
		{
			if (!isInitEquipDescriptionHoverHint)
			{
				isInitEquipDescriptionHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("EquipDescriptionHoverHint", "Prefabs", parentPanel.transform);
				_equipDescriptionHoverHint = gameObject.GetComponent<EquipDescriptionHoverHint>();
				_equipDescriptionHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			EquipmentDescriptionHoverData equipmentDescriptionHoverData;
			if ((equipmentDescriptionHoverData = data as EquipmentDescriptionHoverData) != null)
			{
				InitEquipDescriptionHoverHint();
				_equipDescriptionHoverHint.gameObject.SetActive(value: true);
				EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipmentDescriptionHoverData.ItemCode);
				_equipDescriptionHoverHint.SetEquipHoverDescriptionBaseInfo(equipmentDescriptionHoverData.IsUnlocked ? equipmentCardAttr.NameKey.LocalizeText() : "???", equipmentDescriptionHoverData.IsUnlocked ? equipmentCardAttr.DesKeyNormal.LocalizeText() : (equipmentDescriptionHoverData.IsPurchased ? "EquipNotUnlockedDesKey".LocalizeText() : "EquipNotPurchasedDesKey".LocalizeText()), equipmentCardAttr.EquipmentType.ToString().LocalizeText());
				if (equipmentDescriptionHoverData.IsUnlocked)
				{
					_equipDescriptionHoverHint.AddKeys(equipmentCardAttr.AllKeys);
					if (equipmentCardAttr.SuitType != 0)
					{
						_equipDescriptionHoverHint.LoadSuitInfo(SuitHandler.GetSuitInfo(equipmentCardAttr.SuitType), equipmentDescriptionHoverData.IsCheckSuit);
					}
					else
					{
						_equipDescriptionHoverHint.HideSuitInfo();
					}
				}
				else
				{
					_equipDescriptionHoverHint.HideSuitInfo();
				}
				_equipDescriptionHoverHint.ForceRebuildLayoutImmediate();
				return new AdjustHandler(data.TargetRectTransform, _equipDescriptionHoverHint.m_RectTransform, (equipmentDescriptionHoverData.IsUnlocked && equipmentCardAttr.AllKeys.Count > 0) ? _equipDescriptionHoverHint.KeyRootRect : null, 0f, _equipDescriptionHoverHint.ShowKeyAtLeft, _equipDescriptionHoverHint.ShowKeyAtRight);
			}
			throw new InvalidCastException("Cannot cast data to UsualItemDescriptionHoverData");
		}

		public override void HandleHoverOver()
		{
			_equipDescriptionHoverHint.gameObject.SetActive(value: false);
			_equipDescriptionHoverHint.RecycleAllKeys();
		}
	}

	private class SkillDescriptionHoverHintHandler : HoverHandler
	{
		private const string NotUnlockedKey = "SkillNotUnlockedDesKey";

		private const string NotPurchasedKey = "SkillNotPurchasedDesKey";

		private const string NotUnlockedName = "???";

		private SkillDescriptionHoverHint _skillDescriptionHoverHint;

		private bool isInitSkillDescriptionHoverHint;

		public SkillDescriptionHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitSkillDescriptionHoverHint()
		{
			if (!isInitSkillDescriptionHoverHint)
			{
				isInitSkillDescriptionHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SkillDescriptionHoverHint", "Prefabs", parentPanel.transform);
				_skillDescriptionHoverHint = gameObject.GetComponent<SkillDescriptionHoverHint>();
				_skillDescriptionHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			SkillItemDescriptionHoverData skillItemDescriptionHoverData;
			if ((skillItemDescriptionHoverData = data as SkillItemDescriptionHoverData) != null)
			{
				InitSkillDescriptionHoverHint();
				_skillDescriptionHoverHint.gameObject.SetActive(value: true);
				SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(skillItemDescriptionHoverData.PlayerOccupation, skillItemDescriptionHoverData.ItemCode);
				_skillDescriptionHoverHint.LoadSkillBaseInfo(skillItemDescriptionHoverData.IsUnlocked ? skillCardAttr.NameKey.LocalizeText() : "???", skillItemDescriptionHoverData.IsUnlocked ? skillCardAttr.DesKeyNormal.LocalizeText() : (skillItemDescriptionHoverData.IsPurchased ? "SkillNotUnlockedDesKey".LocalizeText() : "SkillNotPurchasedDesKey".LocalizeText()));
				if (skillItemDescriptionHoverData.IsUnlocked)
				{
					_skillDescriptionHoverHint.AddKeys(skillCardAttr.AllKeys);
					_skillDescriptionHoverHint.LoadSkillComsumeInfo(skillItemDescriptionHoverData.PlayerOccupation, skillItemDescriptionHoverData.ItemCode, skillItemDescriptionHoverData.isCheckCardStat);
				}
				else
				{
					_skillDescriptionHoverHint.HideComsume();
				}
				_skillDescriptionHoverHint.ForceRebuildLayoutImmediate();
				return new AdjustHandler(data.TargetRectTransform, _skillDescriptionHoverHint.m_RectTransform, (skillItemDescriptionHoverData.IsUnlocked && skillCardAttr.AllKeys.Count > 0) ? _skillDescriptionHoverHint.KeyRootRect : null, 0f, _skillDescriptionHoverHint.ShowKeyAtLeft, _skillDescriptionHoverHint.ShowKeyAtRight);
			}
			throw new InvalidCastException("Cannot cast data to SkillItemDescriptionHoverData");
		}

		public override void HandleHoverOver()
		{
			_skillDescriptionHoverHint.gameObject.SetActive(value: false);
			_skillDescriptionHoverHint.RecycleAllKeys();
		}
	}

	private class CardDescriptionHoverHintHandler : HoverHandler
	{
		private const string NotUnlockedKey = "CardNotUnlockedDesKey";

		private const string NotPurchasedKey = "CardNotPurchasedDesKey";

		private const string NotUnlockedName = "???";

		private bool isInitCardDescriptionHoverHint;

		private CardDescriptionHoverHint _cardDescriptionHoverHint;

		public CardDescriptionHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitCardDescriptionHoverHint()
		{
			if (!isInitCardDescriptionHoverHint)
			{
				isInitCardDescriptionHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("CardDescriptionHoverHint", "Prefabs", parentPanel.transform);
				_cardDescriptionHoverHint = gameObject.GetComponent<CardDescriptionHoverHint>();
				_cardDescriptionHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			UsualItemDescriptionHoverData usualItemDescriptionHoverData;
			if ((usualItemDescriptionHoverData = data as UsualItemDescriptionHoverData) != null)
			{
				InitCardDescriptionHoverHint();
				_cardDescriptionHoverHint.gameObject.SetActive(value: true);
				UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(usualItemDescriptionHoverData.ItemCode);
				_cardDescriptionHoverHint.SetCardBaseInfo(usualItemDescriptionHoverData.IsUnlocked ? usualCardAttr.NameKey.LocalizeText() : "???", usualItemDescriptionHoverData.IsUnlocked ? usualCardAttr.DesKeyNormal.LocalizeText() : (usualItemDescriptionHoverData.IsPurchased ? "CardNotUnlockedDesKey".LocalizeText() : "CardNotPurchasedDesKey".LocalizeText()));
				if (usualItemDescriptionHoverData.IsUnlocked)
				{
					_cardDescriptionHoverHint.AddKeys(usualCardAttr.AllKeys);
				}
				_cardDescriptionHoverHint.ForceRebuildLayoutImmediate();
				return new AdjustHandler(data.TargetRectTransform, _cardDescriptionHoverHint.m_RectTransform, (usualItemDescriptionHoverData.IsUnlocked && usualCardAttr.AllKeys.Count > 0) ? _cardDescriptionHoverHint.KeyRootRect : null, 0f, _cardDescriptionHoverHint.ShowKeyAtLeft, _cardDescriptionHoverHint.ShowKeyAtRight);
			}
			throw new InvalidCastException("Cannot cast data to UsualItemDescriptionHoverData");
		}

		public override void HandleHoverOver()
		{
			_cardDescriptionHoverHint.gameObject.SetActive(value: false);
			_cardDescriptionHoverHint.RecycleAllKeys();
		}
	}

	private class NormalMessageHoverHintHandler : HoverHandler
	{
		private bool isInitNormalMessageHoverHint;

		private NormalMessageHoverHint _normalMessageHoverHint;

		public NormalMessageHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitNormalMessageHoverHint()
		{
			if (!isInitNormalMessageHoverHint)
			{
				isInitNormalMessageHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("NormalMessageHoverHint", "Prefabs", parentPanel.transform);
				_normalMessageHoverHint = gameObject.GetComponent<NormalMessageHoverHint>();
				_normalMessageHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			NormalMessageHoverData normalMessageHoverData;
			if ((normalMessageHoverData = data as NormalMessageHoverData) != null)
			{
				InitNormalMessageHoverHint();
				_normalMessageHoverHint.gameObject.SetActive(value: true);
				_normalMessageHoverHint.SetMessageHint(normalMessageHoverData.content, normalMessageHoverData.allKeys);
				return new AdjustHandler(data.TargetRectTransform, _normalMessageHoverHint.m_RectTransform, (normalMessageHoverData.allKeys != null && normalMessageHoverData.allKeys.Count > 0) ? _normalMessageHoverHint.KeyRootRect : null, 0f, _normalMessageHoverHint.ShowKeyAtLeft, _normalMessageHoverHint.ShowKeyAtRight);
			}
			throw new InvalidCastException("Cannot cast data to NormalMessageHoverData");
		}

		public override void HandleHoverOver()
		{
			_normalMessageHoverHint.gameObject.SetActive(value: false);
			_normalMessageHoverHint.RecycleAllKeys();
		}
	}

	private class CardHoverHintHandler : HoverHandler
	{
		private bool isInitCardHoverHint;

		private CardHoverHint cardHoverHint;

		public CardHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitCardHoverHint()
		{
			if (!isInitCardHoverHint)
			{
				isInitCardHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("CardHoverHint", "Prefabs", parentPanel.transform);
				cardHoverHint = gameObject.GetComponent<CardHoverHint>();
				cardHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			UsualItemDescriptionHoverData usualItemDescriptionHoverData;
			if ((usualItemDescriptionHoverData = data as UsualItemDescriptionHoverData) != null)
			{
				InitCardHoverHint();
				cardHoverHint.gameObject.SetActive(value: true);
				UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(usualItemDescriptionHoverData.ItemCode);
				cardHoverHint.SetCard(usualCardAttr);
				return new AdjustHandler(data.TargetRectTransform, cardHoverHint.m_RectTransform, (usualCardAttr.AllKeys.Count > 0) ? cardHoverHint.KeyRootRect : null, 0f, cardHoverHint.ShowKeyAtLeft, cardHoverHint.ShowKeyAtRight);
			}
			throw new InvalidCastException("Cannot cast data to UsualItemDescriptionHoverData");
		}

		public override void HandleHoverOver()
		{
			cardHoverHint.gameObject.SetActive(value: false);
			cardHoverHint.RecycleAllKeys();
		}
	}

	private class BuffDetailHoverHintHandler : HoverHandler
	{
		private BuffDetailHoverHint _buffDetailHoverHint;

		private bool isInitBuffDetailHoverHint;

		public BuffDetailHoverHintHandler(ItemHoverHintUI parentPanel)
			: base(parentPanel)
		{
		}

		private void InitBuffDetailHoverHint()
		{
			if (!isInitBuffDetailHoverHint)
			{
				isInitBuffDetailHoverHint = true;
				GameObject gameObject = SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("BuffDetailHoverHint", "Prefabs", parentPanel.transform);
				_buffDetailHoverHint = gameObject.GetComponent<BuffDetailHoverHint>();
				_buffDetailHoverHint.SetParentUI(parentPanel);
			}
		}

		public override AdjustHandler HandleHoverInfo(HoverData data)
		{
			BuffDetailHoverData buffDetailHoverData;
			if ((buffDetailHoverData = data as BuffDetailHoverData) != null)
			{
				InitBuffDetailHoverHint();
				_buffDetailHoverHint.gameObject.SetActive(value: true);
				BuffData buffDataByBuffType = DataManager.Instance.GetBuffDataByBuffType(buffDetailHoverData.buff.BuffType);
				_buffDetailHoverHint.LoadBuffDetail((buffDataByBuffType.BuffType + "_Name").LocalizeText(), (buffDataByBuffType.BuffType + "_Des").LocalizeText(), buffDataByBuffType.BuffRoundType, buffDetailHoverData.buff.GetBuffHinAmount());
				return new AdjustHandler(data.TargetRectTransform, _buffDetailHoverHint.m_RectTransform, null, 0f, null, null);
			}
			throw new InvalidCastException("Cannot cast data to BuffDetailHoverData");
		}

		public override void HandleHoverOver()
		{
			_buffDetailHoverHint.gameObject.SetActive(value: false);
		}
	}

	[Serializable]
	public enum HoverType
	{
		MonsterDescription,
		EquipDescription,
		SkillDescription,
		CardDescription,
		NormalMessage,
		Card,
		BuffDetail
	}

	public class HoverData
	{
		public RectTransform TargetRectTransform;

		public HoverData(RectTransform targetRectTransform)
		{
			TargetRectTransform = targetRectTransform;
		}
	}

	public class MonsterDescriptionHoverData : HoverData
	{
		public string NameStr;

		public string DescriptionContentStr;

		public MonsterDescriptionHoverData(RectTransform targetRectTransform, string name, string descriptionContentStr)
			: base(targetRectTransform)
		{
			NameStr = name;
			DescriptionContentStr = descriptionContentStr;
		}
	}

	public class UsualItemDescriptionHoverData : HoverData
	{
		public string ItemCode;

		public bool IsUnlocked;

		public bool IsPurchased;

		public UsualItemDescriptionHoverData(RectTransform targetRectTransform, string itemCode, bool isUnlocked, bool isPurchased)
			: base(targetRectTransform)
		{
			ItemCode = itemCode;
			IsUnlocked = isUnlocked;
			IsPurchased = isPurchased;
		}
	}

	public class EquipmentDescriptionHoverData : HoverData
	{
		public string ItemCode;

		public bool IsUnlocked;

		public bool IsPurchased;

		public bool IsCheckSuit;

		public EquipmentDescriptionHoverData(RectTransform targetRectTransform, string itemCode, bool isCheckSuit, bool isUnlocked, bool isPurchased)
			: base(targetRectTransform)
		{
			ItemCode = itemCode;
			IsUnlocked = isUnlocked;
			IsPurchased = isPurchased;
			IsCheckSuit = isCheckSuit;
		}
	}

	public class SkillItemDescriptionHoverData : UsualItemDescriptionHoverData
	{
		public PlayerOccupation PlayerOccupation;

		public bool isCheckCardStat;

		public SkillItemDescriptionHoverData(RectTransform targetRectTransform, string itemCode, PlayerOccupation playerOccupation, bool isUnlocked, bool isPurchased, bool isCheckCardStat)
			: base(targetRectTransform, itemCode, isUnlocked, isPurchased)
		{
			PlayerOccupation = playerOccupation;
			this.isCheckCardStat = isCheckCardStat;
		}
	}

	public class NormalMessageHoverData : HoverData
	{
		public string content;

		public List<KeyValuePair> allKeys;

		public NormalMessageHoverData(RectTransform targetRectTransform, string content, List<KeyValuePair> allKeys)
			: base(targetRectTransform)
		{
			this.content = content;
			this.allKeys = allKeys;
		}
	}

	public class BuffDetailHoverData : HoverData
	{
		public BaseBuff buff;

		public BuffDetailHoverData(RectTransform targetRectTransform, BaseBuff buff)
			: base(targetRectTransform)
		{
			this.buff = buff;
		}
	}

	private CanvasGroup _canvasGroup;

	private AdjustHandler _handler;

	private bool isShowing;

	private HoverHandler currentHandler;

	private static Dictionary<HoverType, HoverHandler> AllHoverHandlers;

	private Camera mainCamera;

	private Queue<KeyCtrl> allKeyCtrlPools = new Queue<KeyCtrl>();

	public override string UIViewName => "ItemHoverHintUI";

	public override string UILayerName => "TipsLayer";

	public Camera MainCamera
	{
		get
		{
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}
			return mainCamera;
		}
	}

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		HoverType key = (HoverType)objs[0];
		currentHandler = AllHoverHandlers[key];
		_handler = currentHandler.HandleHoverInfo((HoverData)objs[1]);
		_canvasGroup.DOKill();
		_canvasGroup.alpha = 0f;
		StartCoroutine(Show_IE(_handler));
	}

	private IEnumerator Show_IE(AdjustHandler handler)
	{
		yield return new WaitForSecondsRealtime(0.1f);
		isShowing = true;
		AdjustPos(handler);
		_canvasGroup.DOFade(1f, 0.3f);
	}

	public override void HideView()
	{
		isShowing = false;
		base.gameObject.SetActive(value: false);
		currentHandler.HandleHoverOver();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		AllHoverHandlers = new Dictionary<HoverType, HoverHandler>
		{
			{
				HoverType.MonsterDescription,
				new MonsterDescriptionHoverHintHandler(this)
			},
			{
				HoverType.EquipDescription,
				new EquipDescriptionHoverHintHandler(this)
			},
			{
				HoverType.SkillDescription,
				new SkillDescriptionHoverHintHandler(this)
			},
			{
				HoverType.CardDescription,
				new CardDescriptionHoverHintHandler(this)
			},
			{
				HoverType.NormalMessage,
				new NormalMessageHoverHintHandler(this)
			},
			{
				HoverType.Card,
				new CardHoverHintHandler(this)
			},
			{
				HoverType.BuffDetail,
				new BuffDetailHoverHintHandler(this)
			}
		};
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	private void Update()
	{
		if (isShowing && !_handler.TargetRectTransform.gameObject.activeInHierarchy)
		{
			SingletonDontDestroy<UIManager>.Instance.HideView(this);
		}
	}

	private void AdjustPos(AdjustHandler handler)
	{
		Vector3 vector = MainCamera.ScreenToViewportPoint(Input.mousePosition);
		Vector3 vector2 = MainCamera.WorldToViewportPoint(handler.TargetRectTransform.transform.position);
		Vector3 position = new Vector3(vector2.x, vector.y);
		Vector3 position2 = MainCamera.ViewportToWorldPoint(position);
		position2.z = handler.RectTransform.position.z;
		handler.RectTransform.position = position2;
		if (vector2.x > 0.5f)
		{
			Vector3 offset = (handler.xOffset + handler.TargetRectTransform.sizeDelta.x / 2f) * Vector3.left;
			AdjustLeft(handler, offset);
		}
		else
		{
			Vector3 offset2 = (handler.xOffset + handler.TargetRectTransform.sizeDelta.x / 2f) * Vector3.right;
			AdjustRight(handler, offset2);
		}
	}

	private void AdjustLeft(AdjustHandler handler, Vector3 offset)
	{
		offset += Vector3.left * handler.RectTransform.sizeDelta.x / 2f;
		handler.RectTransform.localPosition += offset;
		if ((bool)handler.KeyRectTransform)
		{
			float num = handler.RectTransform.sizeDelta.x / 2f - handler.RectTransform.localPosition.x + 395f - UIManager.ScreenLocalHalfWidth;
			if (num > 0f)
			{
				handler.RectTransform.localPosition += Vector3.right * num;
			}
		}
		AdjustBottom(handler);
		handler.AdjustLeftHandler?.Invoke();
	}

	private void AdjustRight(AdjustHandler handler, Vector3 offset)
	{
		offset += Vector3.right * handler.RectTransform.sizeDelta.x / 2f;
		handler.RectTransform.localPosition += offset;
		if ((bool)handler.KeyRectTransform)
		{
			float num = handler.RectTransform.sizeDelta.x / 2f + handler.RectTransform.localPosition.x + 395f - UIManager.ScreenLocalHalfWidth;
			if (num > 0f)
			{
				handler.RectTransform.localPosition += Vector3.left * num;
			}
		}
		AdjustBottom(handler);
		handler.AdjustRightHandler?.Invoke();
	}

	private void AdjustBottom(AdjustHandler handler)
	{
		float b = ((handler.KeyRectTransform == null) ? 0f : handler.KeyRectTransform.sizeDelta.y);
		float num = Mathf.Max(handler.RectTransform.sizeDelta.y, b) - handler.RectTransform.localPosition.y - UIManager.ScreenLocalHalfHeight;
		if (num > 0f)
		{
			handler.RectTransform.localPosition += Vector3.up * num;
		}
	}

	public KeyCtrl GetKeyCtrl(Transform root)
	{
		if (allKeyCtrlPools.Count > 0)
		{
			KeyCtrl keyCtrl = allKeyCtrlPools.Dequeue();
			keyCtrl.gameObject.SetActive(value: true);
			return keyCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("KeyCtrl", "Prefabs", root).GetComponent<KeyCtrl>();
	}

	public void RecycleKeyCtrl(KeyCtrl ctrl)
	{
		ctrl.gameObject.SetActive(value: false);
		allKeyCtrlPools.Enqueue(ctrl);
	}
}
