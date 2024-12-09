using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuffIconCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string BuffIconPath = "Sprites/BuffIcon";

	private Image buffIconImg;

	private RectTransform buffIconRect;

	private Text buffHintText;

	private BaseBuff baseBuff;

	private bool isScreen;

	private Tween buffHintTextTween;

	private Tween buffHintIconTween;

	private void Awake()
	{
		buffIconImg = GetComponent<Image>();
		buffIconRect = buffIconImg.GetComponent<RectTransform>();
		buffHintText = base.transform.Find("HintText").GetComponent<Text>();
	}

	private void OnDisable()
	{
		if (!buffHintTextTween.IsNull() && buffHintTextTween.IsActive())
		{
			buffHintTextTween.Complete();
		}
	}

	public void LoadBuff(BaseBuff buff, bool isScreen)
	{
		this.isScreen = isScreen;
		baseBuff = buff;
		buffIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(buff.BuffType.ToString(), "Sprites/BuffIcon");
		buffHintText.text = buff.GetBuffHint();
		buff.SetUpdateBuffAction(this);
	}

	public void UpdateBuff()
	{
		if (!buffHintTextTween.IsNull() && buffHintTextTween.IsActive())
		{
			buffHintTextTween.Complete();
		}
		buffHintTextTween = buffHintText.transform.TransformHint();
		buffHintText.text = baseBuff.GetBuffHint();
	}

	public void BuffEffectHint()
	{
		if (!buffHintIconTween.IsNull() && buffHintIconTween.IsActive())
		{
			buffHintIconTween.Complete();
		}
		buffHintIconTween = buffIconImg.transform.TransformHint();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ShowKey();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HideKey();
	}

	private void ShowKey()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.BuffDetail, new ItemHoverHintUI.BuffDetailHoverData(buffIconRect, baseBuff));
	}

	private void HideKey()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
	}
}
