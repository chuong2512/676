using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtentSlider : Slider
{
	[Serializable]
	public class ExtensionSliderEvent : UnityEvent
	{
	}

	[SerializeField]
	private ExtensionSliderEvent m_OnPointUp = new ExtensionSliderEvent();

	public ExtensionSliderEvent MOnPointUp
	{
		get
		{
			return m_OnPointUp;
		}
		set
		{
			m_OnPointUp = value;
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		m_OnPointUp.Invoke();
	}
}
