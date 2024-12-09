using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillChooseCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	[HideInInspector]
	public string currentSkill;

	private Image bgImg;

	private ImageExtent_EquipChooseCtrl skillIcon;

	private Text nameText;

	private Image highlightImg;

	private RectTransform m_RectTransform;

	private ChooseSkillUI _chooseSkillUi;

	private void Awake()
	{
		m_RectTransform = GetComponent<RectTransform>();
		bgImg = GetComponent<Image>();
		skillIcon = base.transform.Find("Icon").GetComponent<ImageExtent_EquipChooseCtrl>();
		skillIcon.material = Object.Instantiate(skillIcon.material);
		nameText = base.transform.Find("Name").GetComponent<Text>();
		highlightImg = base.transform.Find("Highlight").GetComponent<Image>();
	}

	public void LoadSkill(ChooseSkillUI chooseSkillUi, string skill)
	{
		_chooseSkillUi = chooseSkillUi;
		currentSkill = skill;
		SkillCard skillCard = FactoryManager.GetSkillCard(Singleton<GameManager>.Instance.Player.PlayerOccupation, skill);
		nameText.text = skillCard.CardName;
		skillIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCard.SkillCardAttr.IllustrationName, "Sprites/SkillIcon");
		skillIcon.threshold = 1f;
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
			skillIcon.threshold = value;
		});
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_xiaohao");
		vfxBase.transform.position = base.transform.position;
		vfxBase.transform.localScale = Vector3.one * 0.3f;
		vfxBase.Play();
	}

	public void SetHighlightActive(bool isActive)
	{
		highlightImg.gameObject.SetActive(isActive);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			_chooseSkillUi.OnChooseSkill(this);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkill, Singleton<GameManager>.Instance.Player.PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: true));
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
