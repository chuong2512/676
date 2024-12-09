using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnMouseEventCallback : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[Serializable]
	public class MouseEventTrigger
	{
		[Serializable]
		public class MouseEvent : UnityEvent
		{
		}

		[SerializeField]
		public MouseEvent Event = new MouseEvent();
	}

	public bool isCantainDoubleClick;

	[Header("鼠标点击回调")]
	[SerializeField]
	private MouseEventTrigger clickEventTrigger;

	[Header("鼠标进入回调")]
	[SerializeField]
	private MouseEventTrigger enterEventTrigger;

	[Header("鼠标离开回调")]
	[SerializeField]
	private MouseEventTrigger exitEventTrigger;

	[Header("鼠标双击回调")]
	[SerializeField]
	private MouseEventTrigger doubleClickEventTrigger;

	[Header("鼠标滚轮向上滚动回调")]
	[SerializeField]
	private MouseEventTrigger wheelUpEventTrigger;

	[Header("鼠标滚轮向下滚动回调")]
	[SerializeField]
	private MouseEventTrigger wheelDownEventTrigger;

	[Header("鼠标点击拖动（不区分左右键）")]
	[SerializeField]
	private MouseEventTrigger mouseDragEventTrigger;

	[Header("鼠标抬起回调（可用于鼠标拖动结束）")]
	[SerializeField]
	private MouseEventTrigger mouseUpEventTrigger;

	[Header("指针按下回调")]
	[SerializeField]
	private MouseEventTrigger mouseDownEventTrigger;

	private bool isMouseDown;

	private bool isMouseOn;

	private bool isHavePreClick;

	private float clickCounter;

	public float clickInterval = 0.25f;

	public MouseEventTrigger ClickEventTrigger
	{
		get
		{
			if (clickEventTrigger.IsNull())
			{
				clickEventTrigger = new MouseEventTrigger();
			}
			return clickEventTrigger;
		}
	}

	public MouseEventTrigger EnterEventTrigger
	{
		get
		{
			if (enterEventTrigger.IsNull())
			{
				enterEventTrigger = new MouseEventTrigger();
			}
			return enterEventTrigger;
		}
	}

	public MouseEventTrigger ExitEventTrigger
	{
		get
		{
			if (exitEventTrigger.IsNull())
			{
				exitEventTrigger = new MouseEventTrigger();
			}
			return exitEventTrigger;
		}
	}

	public MouseEventTrigger DoubleClickEventTrigger
	{
		get
		{
			if (doubleClickEventTrigger.IsNull())
			{
				doubleClickEventTrigger = new MouseEventTrigger();
			}
			return doubleClickEventTrigger;
		}
	}

	public MouseEventTrigger WheelUpEventTrigger
	{
		get
		{
			if (wheelUpEventTrigger.IsNull())
			{
				wheelUpEventTrigger = new MouseEventTrigger();
			}
			return wheelUpEventTrigger;
		}
	}

	public MouseEventTrigger WheelDownEventTrigger
	{
		get
		{
			if (wheelDownEventTrigger.IsNull())
			{
				wheelDownEventTrigger = new MouseEventTrigger();
			}
			return wheelDownEventTrigger;
		}
	}

	public MouseEventTrigger MouseDragEventTrigger
	{
		get
		{
			if (mouseDragEventTrigger.IsNull())
			{
				mouseDragEventTrigger = new MouseEventTrigger();
			}
			return mouseDragEventTrigger;
		}
	}

	public MouseEventTrigger MouseUpEventTrigger
	{
		get
		{
			if (mouseUpEventTrigger.IsNull())
			{
				mouseUpEventTrigger = new MouseEventTrigger();
			}
			return mouseUpEventTrigger;
		}
	}

	public MouseEventTrigger MouseDownEventTrigger
	{
		get
		{
			if (mouseDownEventTrigger.IsNull())
			{
				mouseDownEventTrigger = new MouseEventTrigger();
			}
			return mouseDownEventTrigger;
		}
	}

	private void Update()
	{
		if (isMouseOn)
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis > 0f)
			{
				if (wheelUpEventTrigger.IsNull())
				{
					return;
				}
				wheelUpEventTrigger.Event.Invoke();
			}
			else if (axis < 0f)
			{
				if (wheelDownEventTrigger.IsNull())
				{
					return;
				}
				wheelDownEventTrigger.Event.Invoke();
			}
			if (isMouseDown)
			{
				if (mouseDragEventTrigger.IsNull())
				{
					return;
				}
				mouseDragEventTrigger.Event.Invoke();
			}
			if (isHavePreClick)
			{
				clickCounter += Time.deltaTime;
				if (clickCounter > clickInterval)
				{
					OnClick();
					clickCounter = 0f;
					isHavePreClick = false;
				}
			}
		}
		else if (isHavePreClick)
		{
			OnClick();
			clickCounter = 0f;
			isHavePreClick = false;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal())
		{
			return;
		}
		if (isHavePreClick)
		{
			if (clickCounter <= clickInterval)
			{
				clickCounter = 0f;
				isHavePreClick = false;
				OnDoubleClick();
			}
		}
		else
		{
			isHavePreClick = true;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			OnEnter();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			OnExit();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			OnDown();
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			OnUp();
		}
	}

	private void OnDown()
	{
		isMouseDown = true;
		if (!mouseDownEventTrigger.IsNull())
		{
			mouseDownEventTrigger.Event.Invoke();
		}
	}

	private void OnUp()
	{
		isMouseDown = false;
		if (!mouseUpEventTrigger.IsNull())
		{
			mouseUpEventTrigger.Event.Invoke();
		}
	}

	private void OnDoubleClick()
	{
		if (isCantainDoubleClick)
		{
			if (clickEventTrigger.IsNull())
			{
				return;
			}
			clickEventTrigger.Event.Invoke();
		}
		if (!doubleClickEventTrigger.IsNull())
		{
			doubleClickEventTrigger.Event.Invoke();
		}
	}

	private void OnClick()
	{
		if (!clickEventTrigger.IsNull())
		{
			clickEventTrigger.Event.Invoke();
		}
	}

	private void OnEnter()
	{
		isMouseOn = true;
		if (!enterEventTrigger.IsNull())
		{
			enterEventTrigger.Event.Invoke();
		}
	}

	private void OnExit()
	{
		isMouseOn = false;
		if (!exitEventTrigger.IsNull())
		{
			exitEventTrigger.Event.Invoke();
		}
	}
}
