using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseEquipCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
{
	private ImageExtent_EquipChooseCtrl iconImg;

	private RectTransform iconRect;

	[HideInInspector]
	public string currentEquipCode;

	private ChooseEquipUI _chooseEquipUi;

	private Image highlightImg;

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<ImageExtent_EquipChooseCtrl>();
		iconImg.material = Object.Instantiate(iconImg.material);
		iconRect = iconImg.GetComponent<RectTransform>();
		highlightImg = base.transform.Find("Highlight").GetComponent<Image>();
	}

	public void LoadEquip(ChooseEquipUI chooseEquipUi, string code)
	{
		_chooseEquipUi = chooseEquipUi;
		currentEquipCode = code;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(code);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
		iconImg.threshold = 1f;
		SetHighlightActive(isActive: false);
	}

	public void BurnEquip()
	{
		float value = 1f;
		DOTween.To(() => value, delegate(float x)
		{
			value = x;
		}, 0f, 2f).OnUpdate(delegate
		{
			iconImg.threshold = value;
		});
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_xiaohao");
		vfxBase.transform.position = base.transform.position;
		vfxBase.transform.localScale = Vector3.one * 0.4f;
		vfxBase.Play();
	}

	public void SetHighlightActive(bool isActive)
	{
		highlightImg.gameObject.SetActive(isActive);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(iconRect, currentEquipCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("通用按钮");
		_chooseEquipUi.OnChooseEquiped(this);
	}
}
