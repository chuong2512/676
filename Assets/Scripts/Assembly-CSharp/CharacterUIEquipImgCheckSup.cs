using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterUIEquipImgCheckSup : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private string currentEquipCode;

	private Image iconImg;

	private RectTransform m_RectTransform;

	private void Awake()
	{
		iconImg = GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadEquip(string equipCode)
	{
		currentEquipCode = equipCode;
		EquipmentCardAttr equipmentCardAttr = DataManager.Instance.GetEquipmentCardAttr(equipCode);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(equipmentCardAttr.ImageName, "Sprites/Equipment");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.EquipDescription, new ItemHoverHintUI.EquipmentDescriptionHoverData(m_RectTransform, currentEquipCode, isCheckSuit: true, isUnlocked: true, isPurchased: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
