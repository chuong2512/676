using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BagCardCtrl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private RectTransform m_RectTransform;

	private IBagCardDrag bagcardDrag;

	private float counter;

	private bool isPointDown;

	private bool isPointOn;

	private Coroutine dragCounterCor;

	private Coroutine dragCor;

	private string currentCardCode;

	private bool isEquiped;

	private bool isMain;

	private UsualNoDesCardInfo cardInfo;

	private bool isDraging;

	private ScrollRect DragScorll;

	private Image bagcardNewImg;

	private Tween scaleTween;

	private float originalScale;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		cardInfo = base.transform.Find("UsualNoDesCard").GetComponent<UsualNoDesCardInfo>();
		bagcardNewImg = base.transform.Find("NewImg").GetComponent<Image>();
		m_RectTransform = GetComponent<RectTransform>();
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadCard(string cardCode, IBagCardDrag cardDrag, bool isEquiped, bool isNew, bool isMain, int amount, ScrollRect scrollRectDt)
	{
		DragScorll = scrollRectDt;
		bagcardDrag = cardDrag;
		currentCardCode = cardCode;
		this.isEquiped = isEquiped;
		this.isMain = isMain;
		cardInfo.LoadCard(cardCode);
		if (amount > 0)
		{
			SetCardAmountMoreThanZeroColor();
		}
		else
		{
			SetCardAmountEqualZeroColor();
		}
		bagcardNewImg.gameObject.SetActive(isNew);
		originalScale = base.transform.localScale.x;
	}

	public void SetCardAmountMoreThanZeroColor()
	{
		cardInfo.SetCardUsable();
		cardInfo.FreshCard();
	}

	public void SetCardAmountEqualZeroColor()
	{
		cardInfo.SetCardUnusable();
		cardInfo.FreshCard();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerId == -2 && isEquiped)
		{
			bagcardDrag.ReleaseCard(currentCardCode, isMain);
			bagcardDrag.ForceAddCardToInventory(currentCardCode, isMain);
		}
		else if (eventData.pointerId.IsPointerInputLegal())
		{
			isPointOn = true;
			isPointDown = true;
			counter = 0f;
			dragCounterCor = StartCoroutine(DragCounter_IE());
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		}
	}

	private IEnumerator DragCounter_IE()
	{
		while (isPointDown && isPointOn)
		{
			counter += Time.deltaTime;
			yield return null;
		}
	}

	private void StartDrag()
	{
		DragScorll.OnEndDrag(new PointerEventData(EventSystem.current));
		if (isEquiped)
		{
			isDraging = true;
			bagcardDrag.ReleaseCard(currentCardCode, isMain);
			GameObject obj = bagcardDrag.StartDragCard(currentCardCode, isEquiped);
			dragCor = StartCoroutine(Drag_IE(obj));
		}
		else if (bagcardDrag.RemoveCardFromInventory(currentCardCode))
		{
			isDraging = true;
			GameObject obj2 = bagcardDrag.StartDragCard(currentCardCode, isEquiped);
			dragCor = StartCoroutine(Drag_IE(obj2));
		}
	}

	private IEnumerator Drag_IE(GameObject obj)
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拾取");
		while (isPointDown)
		{
			Vector3 vector = SingletonDontDestroy<CameraController>.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
			obj.transform.position = vector;
			vector = obj.transform.localPosition;
			vector.z = 0f;
			obj.transform.localPosition = vector;
			yield return null;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal())
		{
			return;
		}
		isPointDown = false;
		if (dragCounterCor != null)
		{
			StopCoroutine(dragCounterCor);
		}
		if (counter < 0.5f)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("BagCardDesUI") as BagCardDesUI).ShowBigCard(currentCardCode);
		}
		else if (isDraging)
		{
			isDraging = false;
			bagcardDrag.EndDragCard();
			if (dragCor != null)
			{
				StopCoroutine(dragCor);
			}
			if (isEquiped)
			{
				bagcardDrag.TryAddCardToInventory(currentCardCode, isMain);
			}
			else
			{
				bagcardDrag.TryEquipCard(currentCardCode);
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
			scaleTween.KillTween();
			scaleTween = base.transform.DOScale(originalScale, 0.15f);
			isPointOn = false;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			Vector2 normalized = eventData.delta.normalized;
			if (Mathf.Abs(Vector2.Dot(Vector2.up, normalized)) >= 0.25f)
			{
				StartDrag();
				counter = 1f;
			}
			else
			{
				DragScorll.OnBeginDrag(eventData);
				isDraging = false;
				counter = 1f;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			DragScorll.OnEndDrag(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			DragScorll.OnDrag(eventData);
		}
	}

	private void CancelItemNew()
	{
		bagcardNewImg.gameObject.SetActive(value: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (currentCardCode.IsNullOrEmpty())
		{
			return;
		}
		bagcardDrag.RemoveNewCard(currentCardCode);
		CancelItemNew();
		scaleTween.KillTween();
		scaleTween = base.transform.DOScale(1.05f * originalScale, 0.1f).OnComplete(delegate
		{
			if (!bagcardDrag.IsDragingCard)
			{
				SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.CardDescription, new ItemHoverHintUI.UsualItemDescriptionHoverData(m_RectTransform, currentCardCode, isUnlocked: true, isPurchased: true));
			}
		});
	}
}
