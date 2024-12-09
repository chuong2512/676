using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimatorEventCallback : MonoBehaviour
{
	[Serializable]
	public class AnimatorEventTrigger
	{
		[Serializable]
		public class EnableDisableEvent : UnityEvent
		{
		}

		[SerializeField]
		public EnableDisableEvent Event = new EnableDisableEvent();
	}

	[SerializeField]
	public List<AnimatorEventTrigger> animatorEventTrigger = new List<AnimatorEventTrigger>();

	public void AnimatorEvent()
	{
		for (int i = 0; i < animatorEventTrigger.Count; i++)
		{
			animatorEventTrigger[i].Event.Invoke();
		}
	}
}
