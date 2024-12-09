using UnityEngine;
using UnityEngine.EventSystems;

public class MessageHint : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private bool isShowing;

	public Transform target;

	public string Key;

	public string extraKey;

	private RectTransform m_RectTransform;

	private void Awake()
	{
		isShowing = false;
		m_RectTransform = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (isShowing && !target.gameObject.activeInHierarchy)
		{
			HideHint();
		}
	}

	private void ShowHint()
	{
		if (!isShowing)
		{
			string content = (extraKey.IsNullOrEmpty() ? Key.LocalizeText() : (Key.LocalizeText() + ":" + extraKey.LocalizeText()));
			isShowing = true;
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.NormalMessage, new ItemHoverHintUI.NormalMessageHoverData(m_RectTransform, content, null));
		}
	}

	private void HideHint()
	{
		if (isShowing)
		{
			isShowing = false;
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ShowHint();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HideHint();
	}
}
