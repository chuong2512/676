using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragScrollView : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	[Header("要拖动的ScrollRect")]
	public ScrollRect DragScorll;

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (DragScorll != null)
		{
			DragScorll.OnBeginDrag(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (DragScorll != null)
		{
			DragScorll.OnDrag(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (DragScorll != null)
		{
			DragScorll.OnEndDrag(eventData);
		}
	}
}
