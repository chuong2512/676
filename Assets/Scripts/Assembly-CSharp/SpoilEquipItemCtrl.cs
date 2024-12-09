using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpoilEquipItemCtrl : MonoBehaviour, IPointerExitHandler, IEventSystemHandler, IPointerEnterHandler
{
	private Image iconImg;

	private Transform showPoint;

	private Text nameText;

	private RectTransform m_RectTransform;

	public string CurrentEquipCode { get; private set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		showPoint = base.transform.Find("ShowPoint");
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadEquip(string equipCode)
	{
		CurrentEquipCode = equipCode;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
		nameText.text = equipmentCardAttr.EquipmentType.ToString().LocalizeText();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, CurrentEquipCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
