using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotCtrl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private static bool isDragRect;

	private const string AssetPath = "Sprites/SkillIcon";

	[SerializeField]
	private Sprite skillSlotNormalBg;

	[SerializeField]
	private Sprite skillSlotHighlightBg;

	[SerializeField]
	private Sprite skillSlotNormalBottom;

	[SerializeField]
	private Sprite skillSlotHighlightBottom;

	[SerializeField]
	private int EquipedIndex;

	private Image iconImg;

	private Image skillSlotBgImg;

	private Image skillSlotBottomImg;

	private Image lockedImg;

	private Image cannotUseImg;

	private bool isPointDown;

	private bool isEquiped;

	private string currentSkillCode;

	private Coroutine dragCor;

	private BagUI_SkillPanel parentUI;

	private Image newIconImg;

	private bool isCanCast;

	private ScrollRect DragScorll;

	private bool isDraging;

	private bool isSlotLocked;

	private bool isNewSkill;

	private RectTransform m_RectTransform;

	private Coroutine skillDragCountCor;

	public CanvasGroup CanvasGroup { get; set; }

	public int Index => EquipedIndex;

	public bool IsEquiped => isEquiped;

	public string CurrentSkillCode => currentSkillCode;

	private void Awake()
	{
		skillSlotBottomImg = base.transform.Find("Bottom").GetComponent<Image>();
		skillSlotBgImg = base.transform.Find("Bg").GetComponent<Image>();
		iconImg = base.transform.Find("Icon").GetComponent<Image>();
		lockedImg = base.transform.Find("Locked").GetComponent<Image>();
		newIconImg = base.transform.Find("NewImg").GetComponent<Image>();
		cannotUseImg = base.transform.Find("CannotUseIcon").GetComponent<Image>();
		CanvasGroup = GetComponent<CanvasGroup>();
		m_RectTransform = GetComponent<RectTransform>();
	}

	public void LoadSkill(string skillCode, BagUI_SkillPanel parentUI, bool isEquiped, bool isNew, ScrollRect scrollRect)
	{
		DragScorll = scrollRect;
		this.parentUI = parentUI;
		this.isEquiped = isEquiped;
		currentSkillCode = skillCode;
		skillSlotBottomImg.gameObject.SetActive(isEquiped);
		if (skillCode.IsNullOrEmpty())
		{
			ResetSkillSlot();
			return;
		}
		isNewSkill = isNew;
		SetNewIcon(isNew);
		lockedImg.gameObject.SetActive(value: false);
		isSlotLocked = false;
		SkillCard skillCard = FactoryManager.GetSkillCard(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode);
		iconImg.gameObject.SetActive(value: true);
		iconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(skillCard.SkillCardAttr.IllustrationName, "Sprites/SkillIcon");
		SkillCardAttr skillCardAttr = skillCard.SkillCardAttr;
		isCanCast = false;
		if (!skillCardAttr.MainHandCardCode.IsNullOrEmpty())
		{
			if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedMainHandCards.TryGetValue(skillCardAttr.MainHandCardCode, out var value) && value >= skillCardAttr.MainHandCardConsumeAmount)
			{
				isCanCast = true;
			}
			else
			{
				isCanCast = false;
			}
		}
		else
		{
			isCanCast = true;
		}
		if (isCanCast && !skillCardAttr.SupHandCardCode.IsNullOrEmpty())
		{
			if (Singleton<GameManager>.Instance.Player.PlayerBattleInfo.AllEquipedSupHandCards.TryGetValue(skillCardAttr.SupHandCardCode, out var value2) && value2 >= skillCardAttr.SupHandCardConsumeAmount)
			{
				isCanCast = true;
			}
			else
			{
				isCanCast = false;
			}
		}
		cannotUseImg.gameObject.SetActive(!isCanCast);
	}

	public void LockSkillSlot(BagUI_SkillPanel bagUi)
	{
		isEquiped = false;
		currentSkillCode = string.Empty;
		parentUI = bagUi;
		iconImg.gameObject.SetActive(value: false);
		lockedImg.gameObject.SetActive(value: true);
		isSlotLocked = true;
		cannotUseImg.gameObject.SetActive(value: false);
	}

	public void SetNewIcon(bool isNew)
	{
		newIconImg.gameObject.SetActive(isNew);
	}

	public void ResetSkillSlot()
	{
		currentSkillCode = string.Empty;
		iconImg.gameObject.SetActive(value: false);
		SetNewIcon(isNew: false);
		cannotUseImg.gameObject.SetActive(value: false);
		lockedImg.gameObject.SetActive(value: false);
		isSlotLocked = false;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal() && !isSlotLocked)
		{
			isPointDown = true;
			skillDragCountCor = StartCoroutine(PointerDown_IE());
		}
	}

	private IEnumerator PointerDown_IE()
	{
		float counter = 0f;
		while (isPointDown)
		{
			counter += Time.deltaTime;
			if (counter > 0.3f)
			{
				StartDrag();
				break;
			}
			yield return null;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal() || isSlotLocked)
		{
			return;
		}
		isPointDown = false;
		parentUI.EndDragSkill();
		if (isDraging && !currentSkillCode.IsNullOrEmpty())
		{
			if (isEquiped)
			{
				if (parentUI.CurrentPointInSlot != null)
				{
					if (!parentUI.TrySwapSkill(this, parentUI.CurrentPointInSlot))
					{
						LoadSkill(currentSkillCode, parentUI, isEquiped, isNewSkill, DragScorll);
					}
				}
				else if (!parentUI.TryReleaseSkill(Index))
				{
					LoadSkill(currentSkillCode, parentUI, isEquiped, isNewSkill, DragScorll);
				}
				SingletonDontDestroy<AudioManager>.Instance.PlaySound("技能拖动放置");
			}
			else
			{
				if (parentUI.CurrentPointInSlot != null && parentUI.CurrentPointInSlot != this)
				{
					if (!parentUI.TrySwapSkill(this, parentUI.CurrentPointInSlot))
					{
						LoadSkill(currentSkillCode, parentUI, isEquiped, isNewSkill, DragScorll);
					}
				}
				else
				{
					LoadSkill(currentSkillCode, parentUI, isEquiped, isNewSkill, DragScorll);
				}
				SingletonDontDestroy<AudioManager>.Instance.PlaySound("技能拖动_摁槽");
			}
		}
		isDraging = false;
		isDragRect = false;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal() && !isSlotLocked)
		{
			parentUI.SetCurrentPointInSlot(null);
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
			CancelHighlight();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal() || isSlotLocked)
		{
			return;
		}
		parentUI.SetCurrentPointInSlot(this);
		if (!parentUI.IsDragingSkillItem && !isDragRect && !currentSkillCode.IsNullOrEmpty())
		{
			SingletonDontDestroy<UIManager>.Instance.ShowView("ItemHoverHintUI", ItemHoverHintUI.HoverType.SkillDescription, new ItemHoverHintUI.SkillItemDescriptionHoverData(m_RectTransform, currentSkillCode, Singleton<GameManager>.Instance.Player.PlayerOccupation, isUnlocked: true, isPurchased: true, isCheckCardStat: true));
			if (isNewSkill)
			{
				isNewSkill = false;
				SetNewIcon(isNew: false);
				parentUI.CancelNewSkillSlot(this);
			}
			SetHighlight();
		}
	}

	private IEnumerator Drag_IE(GameObject obj)
	{
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

	private void CancelHighlight()
	{
		skillSlotBgImg.sprite = skillSlotNormalBg;
		skillSlotBottomImg.sprite = skillSlotNormalBottom;
	}

	private void SetHighlight()
	{
		skillSlotBgImg.sprite = skillSlotHighlightBg;
		skillSlotBottomImg.sprite = skillSlotHighlightBottom;
	}

	private void StartDrag()
	{
		if (!isDraging && !currentSkillCode.IsNullOrEmpty())
		{
			isDraging = true;
			GameObject obj = parentUI.StartDragSkill(currentSkillCode);
			dragCor = StartCoroutine(Drag_IE(obj));
			SetDragEmpty();
			CancelHighlight();
			SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
		}
	}

	private void SetDragEmpty()
	{
		iconImg.gameObject.SetActive(value: false);
		SetNewIcon(isNew: false);
		cannotUseImg.gameObject.SetActive(value: false);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!eventData.pointerId.IsPointerInputLegal() || isSlotLocked)
		{
			return;
		}
		if (DragScorll != null)
		{
			Vector2 normalized = eventData.delta.normalized;
			if (Mathf.Abs(Vector2.Dot(Vector2.up, normalized)) >= 0.25f)
			{
				StartDrag();
			}
			else if (!isDraging)
			{
				isDragRect = true;
				DragScorll.OnBeginDrag(eventData);
				if (skillDragCountCor != null)
				{
					StopCoroutine(skillDragCountCor);
					skillDragCountCor = null;
				}
				SingletonDontDestroy<UIManager>.Instance.HideView("ItemHoverHintUI");
			}
		}
		else
		{
			StartDrag();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal() && !isSlotLocked && DragScorll != null)
		{
			DragScorll.OnEndDrag(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal() && !isSlotLocked && DragScorll != null)
		{
			DragScorll.OnDrag(eventData);
		}
	}
}
