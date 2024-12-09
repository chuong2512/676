using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleEventBtnCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string GameOoptionIconAssetPath = "Sprites/GameEvent/OptIcon";

	private static Dictionary<string, MethodInfo> allHoverHandlerMethodInfos = new Dictionary<string, MethodInfo>();

	private Button btn;

	private Text btnText;

	private Image iconImg;

	private OptionData currentOptionData;

	private Action currentHandler;

	private GameEventUI _gameEventUi;

	private RectTransform m_RectTransform;

	public Button Btn => btn;

	private void Awake()
	{
		btn = GetComponent<Button>();
		btnText = base.transform.Find("Text").GetComponent<Text>();
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		btn.onClick.AddListener(OnClickBtn);
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadBtnInfo(GameEventUI gameEventUi, OptionData data, Action handler, bool isInteractive)
	{
		_gameEventUi = gameEventUi;
		currentOptionData = data;
		SetBtnInteractive(isInteractive);
		currentHandler = handler;
		btnText.text = data.OptionKeys.LocalizeText();
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.OptionIconName, "Sprites/GameEvent/OptIcon");
	}

	private void OnClickBtn()
	{
		if (_gameEventUi.isBtnActive)
		{
			if (!currentOptionData.HoverHintCode.IsNullOrEmpty())
			{
				SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
			}
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
			currentHandler?.Invoke();
		}
	}

	public void SetBtnInteractive(bool isActive)
	{
		btn.interactable = isActive;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!currentOptionData.HoverHintCode.IsNullOrEmpty())
		{
			ProcessHover();
		}
	}

	private void ProcessHover()
	{
		if (!allHoverHandlerMethodInfos.TryGetValue(currentOptionData.HoverType, out var value))
		{
			value = typeof(SingleEventBtnCtrl).GetMethod(currentOptionData.HoverType + "HoverHint", BindingFlags.Static | BindingFlags.NonPublic);
			allHoverHandlerMethodInfos.Add(currentOptionData.HoverType, value);
		}
		value.Invoke(null, new object[2] { m_RectTransform, currentOptionData.HoverHintCode });
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!currentOptionData.HoverHintCode.IsNullOrEmpty())
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		}
	}

	private static void EquipmentHoverHint(RectTransform target, string equipCode)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(target, equipCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
	}

	private static void GiftHoverHint(RectTransform target, string giftNameStr)
	{
		BaseGift.GiftName giftName = (BaseGift.GiftName)Enum.Parse(typeof(BaseGift.GiftName), giftNameStr);
		GiftData giftDataByGiftName = DataManager.Instance.GetGiftDataByGiftName(giftName);
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(target, giftDataByGiftName.NameKey.LocalizeText() + "\n" + giftDataByGiftName.DesKey.LocalizeText(), giftDataByGiftName.AllKeys));
	}

	private static void SkillHoverHint(RectTransform target, string skillCode)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(target, skillCode, Singleton<GameManager>.Instance.Player.PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: true));
	}

	private static void UsualCardHoverHint(RectTransform target, string cardCode)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(target, cardCode, isUnlocked: true, isPurchased: true));
	}
}
