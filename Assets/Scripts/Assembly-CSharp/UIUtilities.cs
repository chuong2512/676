using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIUtilities
{
	public static bool IsCursorOnUI(List<string> ignoreTags, int inputID = -1)
	{
		inputID = -1;
		if (ignoreTags == null)
		{
			throw new ArgumentNullException("ignoreTags can not be null");
		}
		if (EventSystem.current.IsPointerOverGameObject(inputID))
		{
			List<RaycastResult> list = new List<RaycastResult>();
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};
			EventSystem.current.RaycastAll(eventData, list);
			for (int i = 0; i < list.Count; i++)
			{
				bool flag = true;
				for (int j = 0; j < ignoreTags.Count; j++)
				{
					if (list[i].gameObject.CompareTag(ignoreTags[j]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}
}
