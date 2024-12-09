using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInfoSkillIconCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Image skillIconImg;

	private bool isShowingDetail;

	private string currentSkillCode;

	private RectTransform m_RectTransform;

	private void Awake()
	{
		skillIconImg = base.transform.Find("Icon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadSkill(string skillCode)
	{
		currentSkillCode = skillCode;
		skillIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(DataManager.Instance.GetSkillCardAttr(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode).IllustrationName, "Sprites/SkillIcon");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkillCode, Singleton<GameManager>.Instance.Player.PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: false));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
