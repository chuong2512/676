using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpSkillSlotCtrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerExitHandler, IPointerEnterHandler
{
	private Image slotImg;

	private Image skillIcon;

	private string currentSkillCode;

	private LevelUpChooseSkillUI _levelUpChooseSkillUi;

	private Text nameText;

	private RectTransform m_RectTransform;

	private Tween scaleTween;

	public string CurrentSkillCode => currentSkillCode;

	private void Awake()
	{
		slotImg = GetComponent<Image>();
		skillIcon = base.transform.Find("Icon").GetComponent<Image>();
		nameText = base.transform.Find("Name").GetComponent<Text>();
		m_RectTransform = skillIcon.GetComponent<RectTransform>();
	}

	public void LoadSkill(string skillCode, LevelUpChooseSkillUI levelUpChooseSkillUi)
	{
		_levelUpChooseSkillUi = levelUpChooseSkillUi;
		currentSkillCode = skillCode;
		SkillCardAttr skillCardAttr = DataManager.Instance.GetSkillCardAttr(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode);
		skillIcon.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCardAttr.IllustrationName, "Sprites/SkillIcon");
		nameText.text = skillCardAttr.NameKey.LocalizeText();
	}

	public void LoadEffect(LevelUpChooseSkillUI levelUpChooseSkillUi)
	{
		_levelUpChooseSkillUi = levelUpChooseSkillUi;
		currentSkillCode = string.Empty;
		PlayerLevelUpEffect playerLevelUpEffect = Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect;
		nameText.text = playerLevelUpEffect.NameKey.LocalizeText();
		skillIcon.sprite = playerLevelUpEffect.IconSprite;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal())
		{
			return;
		}
		_levelUpChooseSkillUi.OnChooseSkill(this);
		if (currentSkillCode.IsNullOrEmpty())
		{
			PlayerLevelUpEffect playerLevelUpEffect = Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect;
			(SingletonDontDestroy<UIManager>.Instance.ShowView("LevelEffectDescriptionUI") as LevelEffectDescriptionUI).ShowLevelUpEffect(playerLevelUpEffect.NameKey.LocalizeText(), playerLevelUpEffect.IconSprite, playerLevelUpEffect.DesKey.LocalizeText(), delegate
			{
				_levelUpChooseSkillUi.OnClickConfirmBtn();
				SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
			});
		}
		else
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SkillDescriptionUI") as SkillDescriptionUI).ShowSkillDescription(Singleton<GameManager>.Instance.Player.PlayerOccupation, currentSkillCode, isOnBattle: false, "Choose".LocalizeText(), delegate
			{
				_levelUpChooseSkillUi.OnClickConfirmBtn();
				SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
			}, isInteractive: true);
		}
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("技能详情");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		scaleTween.KillTween();
		scaleTween = skillIcon.transform.DOScale(1f, 0.3f);
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		scaleTween.KillTween();
		scaleTween = skillIcon.transform.DOScale(1.05f, 0.3f);
		if (!currentSkillCode.IsNullOrEmpty())
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkillCode, Singleton<GameManager>.Instance.Player.PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: true));
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(m_RectTransform, Singleton<GameManager>.Instance.Player.PlayerLevelUpEffect.DesKey.LocalizeText(), null));
		}
	}
}
