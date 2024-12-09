using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleIlluPlotCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string NotUnlockName = "???";

	private const string NotUnlockContentKey = "PlotNotUnlockContentKey";

	private Image iconImg;

	private string currentShowingPlotCode;

	private bool isUnlocked;

	private Tween scaleTween;

	private float originalScale;

	private RectTransform m_RectTransform;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadPlot(PlotData data, bool isUnlocked, Material material)
	{
		currentShowingPlotCode = data.PlotCode;
		this.isUnlocked = isUnlocked;
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(data.IconName, "Sprites/Plot");
		iconImg.material = material;
		originalScale = base.transform.localScale.x;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isUnlocked)
		{
			scaleTween.KillTween();
			scaleTween = base.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
			{
				PlotData plotData = DataManager.Instance.GetPlotData(currentShowingPlotCode);
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.MonsterDescription, new ItemHoverHintUI.MonsterDescriptionHoverData(m_RectTransform, plotData.NameKey.LocalizeText(), plotData.ContentKey.LocalizeText()));
			});
		}
		else
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.MonsterDescription, new ItemHoverHintUI.MonsterDescriptionHoverData(m_RectTransform, "???", "PlotNotUnlockContentKey".LocalizeText()));
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		scaleTween.KillTween();
		scaleTween = base.transform.DOScale(originalScale, 0.15f);
	}
}
