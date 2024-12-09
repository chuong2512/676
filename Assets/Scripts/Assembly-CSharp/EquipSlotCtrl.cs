using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlotCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Image iconImg;

	private Text nameText;

	private Image newIconImg;

	private RectTransform m_RectTransform;

	private Tween scaleTween;

	private float originalScale;

	private BagUI_EquipPanel _bagUiEquipPanel;

	private Action<string> tryRemoveNewHandler;

	public string CurrentEquipCode { get; private set; }

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("NameBg/NameText").GetComponent<Text>();
		newIconImg = base.transform.Find("NewImg").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadEquip(BagUI_EquipPanel bagUiEquipPanel, Action<string> tryRemoveNewHandler, string equipCode, bool isNew)
	{
		this.tryRemoveNewHandler = tryRemoveNewHandler;
		_bagUiEquipPanel = bagUiEquipPanel;
		CurrentEquipCode = equipCode;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
		nameText.text = equipmentCardAttr.NameKey.LocalizeText();
		newIconImg.gameObject.SetActive(isNew);
		originalScale = iconImg.transform.localScale.x;
	}

	public void SetNewIconActive(bool isActive)
	{
		newIconImg.gameObject.SetActive(isActive);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			_bagUiEquipPanel.ShowEquipInfo(CurrentEquipCode);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!CurrentEquipCode.IsNullOrEmpty())
		{
			tryRemoveNewHandler?.Invoke(CurrentEquipCode);
			scaleTween.KillTween();
			scaleTween = iconImg.transform.DOScale(1.1f * originalScale, 0.1f).OnComplete(delegate
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, CurrentEquipCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
			});
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = iconImg.transform.DOScale(originalScale, 0.15f);
	}
}
