using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleMonsterIlluBooksCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string NotUnlockName = "???";

	private const string NotUnlockContentKey = "MONSTERNOTUNLOCKCONTENTKEY";

	private Text nameText;

	private Image illustrationImg;

	private string currentShowingEnemyCode;

	private bool isUnlocked;

	private float originalScale;

	private Tween scaleTween;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		nameText = base.transform.Find("NameBottom/Name").GetComponent<Text>();
		illustrationImg = base.transform.Find("IllustrationImg").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadMonster(EnemyData data, Material material, bool isUnlocked)
	{
		this.isUnlocked = isUnlocked;
		currentShowingEnemyCode = data.EnemyCode;
		illustrationImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.EnemyIllustrationName, "Sprites/Monster");
		nameText.text = (isUnlocked ? data.NameKey.LocalizeText() : "???");
		illustrationImg.material = material;
		originalScale = base.transform.localScale.x;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isUnlocked)
		{
			scaleTween.KillTween();
			scaleTween = base.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
			{
				EnemyData enemyAttr = DataManager.Instance.GetEnemyAttr(currentShowingEnemyCode);
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.MonsterDescription, new ItemHoverHintUI.MonsterDescriptionHoverData(m_RectTransform, enemyAttr.NameKey.LocalizeText(), enemyAttr.DesKey.LocalizeText()));
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.MonsterDescription, new ItemHoverHintUI.MonsterDescriptionHoverData(m_RectTransform, "???", "MONSTERNOTUNLOCKCONTENTKEY".LocalizeText()));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = base.transform.DOScale(originalScale, 0.15f);
	}
}
