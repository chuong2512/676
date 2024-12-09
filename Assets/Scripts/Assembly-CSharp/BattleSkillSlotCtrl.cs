using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkillSlotCtrl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
	public static bool isLocked;

	public const string SkillIconPath = "Sprites/SkillIcon";

	private ImageExtent skillIconImg;

	private SkillCard currentSkill;

	private BattleUI battleUI;

	private bool isPointDown;

	private bool isPointEnter;

	private bool isCanCast;

	private Image lockImg;

	private VfxBase highlightVfx;

	private Tween iconMoveTween;

	private bool isTryCastingSkill;

	private Coroutine checkCor;

	private bool isMouseBottom;

	private void Awake()
	{
		skillIconImg = base.transform.Find("Icon").GetComponent<ImageExtent>();
		skillIconImg.material = Object.Instantiate(skillIconImg.material);
		lockImg = base.transform.Find("Lock").GetComponent<Image>();
	}

	public void LoadSkill(string skillCode, BattleUI battleUi)
	{
		currentSkill = FactoryManager.GetSkillCard(Singleton<GameManager>.Instance.Player.PlayerOccupation, skillCode);
		skillIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(currentSkill.SkillCardAttr.IllustrationName, "Sprites/SkillIcon");
		battleUI = battleUi;
		EventManager.RegisterEvent(EventEnum.E_UpdateApAmount, OnPlayerApAmountUpdate);
		EventManager.RegisterEvent(EventEnum.E_UpdateSpecialAttr, OnPlayerSpecialAttrUpdate);
		EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnPlayerGetBuff);
		EventManager.RegisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
		EventManager.RegisterEvent(EventEnum.E_PlayerHandCardChanged, OnPlayerHandCardChanged);
		isTryCastingSkill = false;
		if (isLocked)
		{
			LockSkill();
		}
		else
		{
			UnlockSkill(isNeedAnim: false);
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && isTryCastingSkill)
		{
			currentSkill?.HandleEndCastHint();
			isTryCastingSkill = false;
			isPointDown = false;
			isPointEnter = false;
			if (checkCor != null)
			{
				StopCoroutine(checkCor);
			}
			isMouseBottom = true;
			if ((bool)highlightVfx)
			{
				highlightVfx.Recycle();
				highlightVfx = null;
			}
			battleUI.EndSkillDescription();
			battleUI.HideAllKeyCtrl();
			SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
			CancelUseSkill();
		}
	}

	private void LockSkill()
	{
		lockImg.gameObject.SetActive(value: true);
	}

	public void UnlockSkill(bool isNeedAnim)
	{
		lockImg.gameObject.SetActive(value: false);
		if (isNeedAnim)
		{
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_jinengjiesuo");
			vfxBase.transform.position = base.transform.position;
			vfxBase.Play();
		}
		CheckCastCondition();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!isLocked && Singleton<GameManager>.Instance.BattleSystem.BattleRound == Round.PlayerRound && eventData.pointerId.IsPointerInputLegal() && !Singleton<GameManager>.Instance.Player.IsPlayerCastingCard && Application.isFocused)
		{
			isPointDown = true;
			isCanCast = currentSkill.IsCanCast(Singleton<GameManager>.Instance.Player, out var failResult);
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SkillDescriptionUI") as SkillDescriptionUI).ShowSkillDescription(currentSkill.CardCode, battleUI.SkillDesPoint.position, isOnBattle: true);
			checkCor = StartCoroutine(SkillPointDown_IE());
			currentSkill.OnPointDown();
			if (!isCanCast)
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble(failResult, battleUI.bubbleHintPoint.position);
			}
			else
			{
				battleUI.HighlightSkillCards(currentSkill.CardCode);
			}
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("战斗选中技能");
			Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			isPointDown = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			if (iconMoveTween != null && iconMoveTween.IsActive())
			{
				iconMoveTween.Complete();
			}
			isPointEnter = true;
			iconMoveTween = skillIconImg.transform.DOScale(1.1f, 0.2f);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal())
		{
			if (iconMoveTween != null && iconMoveTween.IsActive())
			{
				iconMoveTween.Complete();
			}
			isPointEnter = true;
			iconMoveTween = skillIconImg.transform.DOScale(1f, 0.2f);
			isPointEnter = false;
			if (isPointDown)
			{
				battleUI.EndSkillDescription();
				battleUI.HideAllKeyCtrl();
				SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
			}
		}
	}

	private IEnumerator SkillPointDown_IE()
	{
		isTryCastingSkill = true;
		Camera _camera = SingletonDontDestroy<CameraController>.Instance.MainCamera;
		Vector3 rect = Vector3.zero;
		highlightVfx = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_liuguangbian");
		highlightVfx.transform.position = base.transform.position;
		highlightVfx.Play();
		while (isPointDown)
		{
			rect = _camera.ScreenToViewportPoint(Input.mousePosition);
			if (rect.y - UIManager.MaskY <= 0.34f * UIManager.WorldScale)
			{
				if (!isMouseBottom)
				{
					currentSkill.HandleEndCastHint();
					isMouseBottom = true;
				}
			}
			else if (isMouseBottom)
			{
				currentSkill.HandleShowCastHint(base.transform);
				isMouseBottom = false;
			}
			currentSkill.HandlePointDown(rect);
			yield return null;
		}
		isMouseBottom = true;
		if ((bool)highlightVfx)
		{
			highlightVfx.Recycle();
			highlightVfx = null;
		}
		isTryCastingSkill = false;
		currentSkill.HandleEndCastHint();
		battleUI.EndSkillDescription();
		battleUI.HideAllKeyCtrl();
		SingletonDontDestroy<UIManager>.Instance.HideView("SkillDescriptionUI");
		currentSkill.HandlePointUp(rect, TryUseSkill, CancelUseSkill);
	}

	private void TryUseSkill()
	{
		if (currentSkill.IsCanCast(Singleton<GameManager>.Instance.Player, out var _))
		{
			Singleton<GameManager>.Instance.Player.PlayerUseASkillCard(currentSkill);
		}
		Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = false;
	}

	private void CancelUseSkill()
	{
		Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = false;
	}

	private void OnPlayerApAmountUpdate(EventData e)
	{
		CheckCastCondition();
	}

	private void OnPlayerSpecialAttrUpdate(EventData e)
	{
		CheckCastCondition();
	}

	private void OnPlayerGetBuff(EventData e)
	{
		CheckCastCondition();
	}

	private void OnPlayerRemoveBuff(EventData e)
	{
		CheckCastCondition();
	}

	private void OnPlayerHandCardChanged(EventData e)
	{
		CheckCastCondition();
	}

	private void CheckCastCondition()
	{
		if (!isLocked && currentSkill.IsCanCast(Singleton<GameManager>.Instance.Player, out var _))
		{
			SetCanCast();
		}
		else
		{
			SetCanNotCast();
		}
	}

	private void SetCanCast()
	{
		skillIconImg.toggleTint = false;
		skillIconImg.SetAllDirty();
	}

	private void SetCanNotCast()
	{
		skillIconImg.toggleTint = true;
		skillIconImg.SetAllDirty();
	}

	public void RecycleSkillSlot()
	{
		if (isTryCastingSkill)
		{
			currentSkill.HandleEndCastHint();
		}
		EventManager.UnregisterEvent(EventEnum.E_UpdateApAmount, OnPlayerApAmountUpdate);
		EventManager.UnregisterEvent(EventEnum.E_UpdateSpecialAttr, OnPlayerSpecialAttrUpdate);
		EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_GetNewBuff, OnPlayerGetBuff);
		EventManager.UnregisterObjRelatedEvent(Singleton<GameManager>.Instance.Player, EventEnum.E_RemoveBuff, OnPlayerRemoveBuff);
		EventManager.UnregisterEvent(EventEnum.E_PlayerHandCardChanged, OnPlayerHandCardChanged);
		currentSkill = null;
	}
}
