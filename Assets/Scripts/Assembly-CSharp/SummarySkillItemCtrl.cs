using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummarySkillItemCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Image skillIconImg;

	private string currentSkillCode;

	private RectTransform m_RectTransform;

	private PlayerOccupation PlayerOccupation;

	private void Awake()
	{
		skillIconImg = base.transform.Find("Icon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadSkill(PlayerOccupation playerOccupation, string skillCode)
	{
		PlayerOccupation = playerOccupation;
		currentSkillCode = skillCode;
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(playerOccupation, skillCode);
		skillIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCardAttr.IllustrationName, "Sprites/SkillIcon");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkillCode, PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: false));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
