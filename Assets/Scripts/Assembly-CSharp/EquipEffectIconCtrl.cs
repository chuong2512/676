using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipEffectIconCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private ImageExtent equipIconImg;

	private Text hintText;

	private Func<string> getDesFunc;

	private Tween effectHintIconTween;

	private RectTransform m_RectTransform;

	private void Awake()
	{
		equipIconImg = base.transform.Find("Icon").GetComponent<ImageExtent>();
		equipIconImg.material = UnityEngine.Object.Instantiate(equipIconImg.material);
		hintText = base.transform.Find("Hint").GetComponent<Text>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadEquip(EquipmentCard equip, string initContent, Func<string> func)
	{
		equipIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equip.ImageName, "Sprites/Equipment");
		hintText.text = initContent;
		getDesFunc = func;
	}

	public void LoadSuit(SuitType suitType, string hintContent, Func<string> func)
	{
		equipIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(SuitHandler.GetSuitInfo(suitType).SuitIconName, "Sprites/Suit");
		hintText.text = hintContent;
		getDesFunc = func;
	}

	public void SetNotEffect()
	{
		equipIconImg.toggleTint = true;
		Fresh();
	}

	public void SetEffect()
	{
		equipIconImg.toggleTint = false;
		Fresh();
	}

	private void Fresh()
	{
		equipIconImg.SetMaterialDirty();
	}

	public void UpdateEquipHint(string hintContent)
	{
		hintText.text = hintContent;
		if (!effectHintIconTween.IsNull() && effectHintIconTween.IsActive())
		{
			effectHintIconTween.Complete();
		}
		effectHintIconTween = hintText.transform.TransformHint();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ShowDescription();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HideDescription();
	}

	private void ShowDescription()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(m_RectTransform, getDesFunc(), null));
	}

	private void HideDescription()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
