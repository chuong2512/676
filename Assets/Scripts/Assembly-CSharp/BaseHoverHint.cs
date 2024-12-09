using UnityEngine;

public abstract class BaseHoverHint : MonoBehaviour
{
	protected ItemHoverHintUI parentUI;

	public RectTransform m_RectTransform { get; private set; }

	private void Awake()
	{
		m_RectTransform = base.transform.GetComponent<RectTransform>();
		OnAwake();
	}

	protected abstract void OnAwake();

	public void SetParentUI(ItemHoverHintUI parentUI)
	{
		this.parentUI = parentUI;
	}
}
