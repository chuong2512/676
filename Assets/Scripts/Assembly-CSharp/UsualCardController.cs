using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UsualCardController : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	private enum CardSize
	{
		Big,
		Middle,
		Mini
	}

	private bool isUsualCardAniming;

	[HideInInspector]
	public bool IsMainHandCard;

	private Vector3 preCastPosition;

	private Quaternion preCastRotation;

	private int preSiblingIndex;

	private bool isApCostSatisfied;

	private Text apCoasText;

	private Image cardOutlightImg;

	[HideInInspector]
	public Transform rightKeyPoint;

	[HideInInspector]
	public Transform leftKeyPoint;

	private BattleUI _battleUi;

	private string failResult;

	private Coroutine checkCor;

	private static readonly int HighlightLerpValue = Shader.PropertyToID("_HighlightLerpValue");

	private static readonly int AlphaThreshold = Shader.PropertyToID("_AlphaThreshold");

	private static readonly int BurnThreshold = Shader.PropertyToID("_BurnThreshold");

	private const float BigSizeCardScale = 1f;

	private const float MiddleSizeCardScale = 0.5f;

	private const float MiniSizeCardScale = 0.3f;

	private const string CardPath = "Sprites/Cards";

	private static readonly Color CanCastApCostColor = "FFFFEBFF".HexColorToColor();

	private static readonly Color CannotCastApCostColor = "E63A21FF".HexColorToColor();

	private CardSize currentCardSize;

	private Tween cardScaleTween;

	private Tween cardMoveTween;

	private Tween cardRotateTween;

	private Image cardImg;

	private Text nameText;

	private Text bigNameText;

	private Text desText;

	private bool isPointDown;

	private bool isPointEnter;

	private Tween highlightMoveTween;

	private bool isForward;

	private bool isThisCardCasting;

	private int fontSize;

	private float lineSpacing;

	public UsualCard CurrentCard { get; private set; }

	private Transform CardTrans => cardImg.transform;

	public bool IsThisCardCasting => isThisCardCasting;

	private void Awake()
	{
		InitCardController();
		apCoasText = base.transform.Find("Card/ApCost").GetComponent<Text>();
		cardOutlightImg = base.transform.Find("Card/OutLight").GetComponent<Image>();
		cardImg.material = UnityEngine.Object.Instantiate(cardImg.material);
		rightKeyPoint = base.transform.Find("Card/RightKeyPoint");
		leftKeyPoint = base.transform.Find("Card/LeftKeyPoint");
	}

	public void LoadCard(BattleUI battleUi, string cardCode, bool isMainHand)
	{
		CurrentCard = FactoryManager.GetUsualCard(cardCode);
		_battleUi = battleUi;
		isThisCardCasting = false;
		IsMainHandCard = isMainHand;
		EventManager.RegisterEvent(EventEnum.E_UpdateApAmount, OnPlayerApAmountUpdate);
		UsualCardAttr usualCardAttr = DataManager.Instance.GetUsualCardAttr(cardCode);
		SetCardImg(SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(usualCardAttr.IllustrationName, "Sprites/Cards"));
		SetCardName(CurrentCard.CardName);
		SetCardBigName(CurrentCard.CardName);
		SetDescription(CurrentCard.GetOnBattleDes(Singleton<GameManager>.Instance.Player, isMainHand));
		ResetCard();
		OnPlayerApAmountUpdate(null);
		EventManager.RegisterEvent(EventEnum.E_CardDescriptionUpdate, CardDiscriptionUpdate);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (!hasFocus && isThisCardCasting)
		{
			isPointDown = false;
			isPointEnter = false;
			if (checkCor != null)
			{
				StopCoroutine(checkCor);
			}
			CurrentCard?.HandleEndCastHint();
			CancelHighlightCard();
			TryCancelUseCard();
		}
	}

	private void CardDiscriptionUpdate(EventData data)
	{
		SetDescription(CurrentCard.GetOnBattleDes(Singleton<GameManager>.Instance.Player, IsMainHandCard));
	}

	private void SetApCost(int number)
	{
		apCoasText.color = (isApCostSatisfied ? CanCastApCostColor : CannotCastApCostColor);
		apCoasText.text = number.ToString();
	}

	private void OnPlayerApAmountUpdate(EventData e)
	{
		if (CurrentCard.IsSatisfiedAp(Singleton<GameManager>.Instance.Player, out var finalValue, out var _))
		{
			SetApCostSatisfied(finalValue);
		}
		else
		{
			SetApCostNotSatisfied(finalValue);
		}
	}

	private void SetApCostSatisfied(int value)
	{
		isApCostSatisfied = true;
		SetApCost(value);
	}

	private void SetApCostNotSatisfied(int value)
	{
		isApCostSatisfied = false;
		SetApCost(value);
	}

	public void RecycleCard(bool isNeedAnim, bool isDrop, Action callback)
	{
		if (isThisCardCasting)
		{
			CancelHighlightCard();
			CurrentCard?.HandleEndCastHint();
		}
		bool isComsumeable = CurrentCard.IsComsumeable;
		CurrentCard = null;
		EventManager.UnregisterEvent(EventEnum.E_UpdateApAmount, OnPlayerApAmountUpdate);
		EventManager.UnregisterEvent(EventEnum.E_CardDescriptionUpdate, CardDiscriptionUpdate);
		if (isNeedAnim)
		{
			RecycleCardAnim(isComsumeable, isDrop, callback);
		}
		else
		{
			HideUsualCard(callback);
		}
	}

	private void HideUsualCard(Action callback)
	{
		isUsualCardAniming = false;
		base.transform.SetAsLastSibling();
		isThisCardCasting = false;
		base.gameObject.SetActive(value: false);
		callback?.Invoke();
	}

	private void OnPointerDownHandler()
	{
		if (CurrentCard != null && Singleton<GameManager>.Instance.BattleSystem.BattleRound == Round.PlayerRound)
		{
			CurrentCard.OnPointDown();
			CurrentCard.IsCanCast(Singleton<GameManager>.Instance.Player, IsMainHandCard, out var _, out failResult);
			preSiblingIndex = (IsMainHandCard ? _battleUi.GetMainSiblingIndex(this) : _battleUi.GetSupSiblingIndex(this));
			preCastPosition = base.transform.position;
			preCastRotation = base.transform.rotation;
			_battleUi.PlayerChooseACardReadyCast(this);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拾取");
		}
	}

	public void OnReadyCastCard()
	{
		SetBigSize();
		_battleUi.AddKeyDescription(CurrentCard.GetKeyDescription(), IsMainHandCard ? BattleUI.KeyDesType.Right : BattleUI.KeyDesType.Left, IsMainHandCard ? leftKeyPoint : rightKeyPoint);
		Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = true;
		isThisCardCasting = true;
		checkCor = StartCoroutine(CheckPoint());
	}

	public void OnCancelCastCard()
	{
		SetMiniSize();
		if (cardMoveTween != null && cardMoveTween.IsActive())
		{
			cardMoveTween.Complete();
		}
		base.transform.SetSiblingIndex(preSiblingIndex);
		_battleUi.HideAllKeyCtrl();
		Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = false;
		isThisCardCasting = false;
		base.transform.localPosition = CalculateCancelLocalPosition(preSiblingIndex);
		CardTrans.localPosition = Vector3.zero;
		CardTrans.position += 0.4f * UIManager.WorldScale * Vector3.up;
		isForward = true;
		CardBackward();
	}

	private Vector3 CalculateCancelLocalPosition(int siblingIndex)
	{
		Player player = Singleton<GameManager>.Instance.Player;
		int num = (IsMainHandCard ? player.PlayerBattleInfo.MainHandCardAmount : player.PlayerBattleInfo.SupHandCardAmount);
		float cardAdjustPosX = BattleUI.GetCardAdjustPosX(IsMainHandCard ? siblingIndex : (num - siblingIndex - 1), num);
		base.transform.position = new Vector3(preCastPosition.x, 0f, 0f);
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = cardAdjustPosX * (float)(IsMainHandCard ? 1 : (-1));
		localPosition.y = 0f;
		return localPosition;
	}

	private IEnumerator CheckPoint()
	{
		if (highlightMoveTween != null && highlightMoveTween.IsActive())
		{
			highlightMoveTween.Kill();
		}
		base.transform.rotation = Quaternion.identity;
		Camera _camera = SingletonDontDestroy<CameraController>.Instance.MainCamera;
		CardTrans.localPosition = Vector3.zero;
		Vector3 checkPos = new Vector3(y: _battleUi.MainHandCardRoot.transform.position.y + 2.5f * UIManager.WorldScale, x: base.transform.position.x, z: 0f);
		MoveTo(checkPos);
		while (isPointDown)
		{
			Vector3 mousePosition = Input.mousePosition;
			Vector3 pointViewRect = _camera.ScreenToViewportPoint(mousePosition);
			if (pointViewRect.y - UIManager.MaskY <= 0.34f * UIManager.WorldScale)
			{
				TryTurnBigCardTypeWhenCasting(_battleUi, checkPos);
			}
			else if (!TryTurnMiddleCardTypeWhenCasting(_battleUi))
			{
				CurrentCard.HandleEndCastHint();
				BattleEnvironmentManager.Instance.HideAllEnemyHint();
				yield break;
			}
			CurrentCard?.HandlePointDown(pointViewRect);
			yield return null;
		}
		CurrentCard.HandleEndCastHint();
		CancelHighlightCard();
		Vector3 mousePosition2 = Input.mousePosition;
		Vector3 pointViewRect2 = _camera.ScreenToViewportPoint(mousePosition2);
		CurrentCard.HandPointUp(pointViewRect2, TryUseCard, TryCancelUseCard);
	}

	private void TryTurnBigCardTypeWhenCasting(BattleUI battleUi, Vector3 checkPos)
	{
		if (currentCardSize != 0)
		{
			TurnToBigSize();
			battleUi.AddKeyDescription(CurrentCard.GetKeyDescription(), IsMainHandCard ? BattleUI.KeyDesType.Right : BattleUI.KeyDesType.Left, IsMainHandCard ? leftKeyPoint : rightKeyPoint);
			CurrentCard.HandleEndCastHint();
			CancelHighlightCard();
			MoveTo(checkPos);
		}
	}

	private bool TryTurnMiddleCardTypeWhenCasting(BattleUI battleUi)
	{
		if (currentCardSize != CardSize.Middle)
		{
			if (!failResult.IsNullOrEmpty())
			{
				(SingletonDontDestroy<UIManager>.Instance.ShowView("BubbleTalkUI") as BubbleTalkUI).ShowPLeftBubble(failResult, battleUi.bubbleHintPoint.position);
				TryCancelUseCard();
				return false;
			}
			TurnToMiddleSize();
			battleUi.HideAllKeyCtrl();
			CurrentCard.HandleShowCastHint(base.transform);
			HighlightCard();
			MoveTo(IsMainHandCard ? battleUi.ReadyCastRightPoint.position : battleUi.ReadyCastLeftPoint.position);
			SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动放置");
		}
		return true;
	}

	private void HighlightCard()
	{
		cardOutlightImg.gameObject.SetActive(value: true);
	}

	private void CancelHighlightCard()
	{
		cardOutlightImg.gameObject.SetActive(value: false);
	}

	private void TryCancelUseCard()
	{
		_battleUi.PlayerCancelCardCast(this);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("卡牌拖动取消");
	}

	private void TryUseCard()
	{
		if (CurrentCard.IsCanCast(Singleton<GameManager>.Instance.Player, IsMainHandCard, out var _, out var _))
		{
			_battleUi.PlayerUseCard(this, null);
		}
		else
		{
			_battleUi.PlayerCancelCardCast(this);
		}
		_battleUi.HideAllKeyCtrl();
		Singleton<GameManager>.Instance.Player.IsPlayerCastingCard = false;
	}

	private void MoveTo(Vector3 pos)
	{
		MoveCard(pos, 0.3f);
	}

	public void SetSkillHighLight()
	{
		CardForward();
		HighlightCard();
	}

	public void SetSkillNormal()
	{
		CardBackward();
		CancelHighlightCard();
	}

	private void RecycleCardAnim(bool isComsumeable, bool isDrop, Action callback)
	{
		apCoasText.gameObject.SetActive(value: false);
		nameText.gameObject.SetActive(value: false);
		desText.gameObject.SetActive(value: false);
		bigNameText.gameObject.SetActive(value: false);
		isUsualCardAniming = true;
		if (isComsumeable && !isDrop)
		{
			cardImg.material.SetFloat(HighlightLerpValue, 0f);
			cardImg.material.SetFloat(AlphaThreshold, 0f);
			cardImg.material.DOFloat(0f, "_BurnThreshold", 2f).OnComplete(delegate
			{
				HideUsualCard(callback);
			});
			VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_xiaohao");
			vfxBase.transform.position = base.transform.position;
			vfxBase.Play();
		}
		else
		{
			Sequence sequence = DOTween.Sequence();
			cardImg.material.SetFloat(BurnThreshold, 0f);
			sequence.Append(cardImg.material.DOFloat(1f, "_HighlightLerpValue", 0.4f));
			sequence.Append(cardImg.material.DOFloat(0f, "_AlphaThreshold", 0.5f));
			sequence.OnComplete(delegate
			{
				HideUsualCard(callback);
			});
		}
	}

	private void ResetCard()
	{
		cardImg.transform.localPosition = Vector3.zero;
		apCoasText.gameObject.SetActive(value: true);
		nameText.gameObject.SetActive(value: true);
		desText.gameObject.SetActive(value: true);
		bigNameText.gameObject.SetActive(value: true);
		cardImg.material.SetFloat(HighlightLerpValue, 0f);
		cardImg.material.SetFloat(AlphaThreshold, 1f);
		cardImg.material.SetFloat(BurnThreshold, 1f);
	}

	private void InitCardController()
	{
		cardImg = base.transform.Find("Card").GetComponent<Image>();
		nameText = base.transform.Find("Card/Name").GetComponent<Text>();
		bigNameText = base.transform.Find("Card/BigName").GetComponent<Text>();
		desText = base.transform.Find("Card/Description").GetComponent<Text>();
		fontSize = desText.fontSize;
		lineSpacing = desText.lineSpacing;
	}

	private void CardForward()
	{
		if (highlightMoveTween != null && highlightMoveTween.IsActive())
		{
			highlightMoveTween.Kill();
		}
		isForward = true;
		float y = CardTrans.position.y;
		highlightMoveTween = CardTrans.DOMoveY(y + 0.4f * UIManager.WorldScale, 0.2f);
	}

	private void CardBackward()
	{
		if (isForward)
		{
			if (highlightMoveTween != null && highlightMoveTween.IsActive())
			{
				highlightMoveTween.Kill();
			}
			isForward = false;
			highlightMoveTween = CardTrans.DOLocalMoveY(0f, 0.2f);
		}
	}

	private void SetCardImg(Sprite sprite)
	{
		cardImg.sprite = sprite;
	}

	private void SetCardName(string cardName)
	{
		nameText.text = cardName;
	}

	private void SetCardBigName(string cardName)
	{
		bigNameText.text = cardName;
	}

	private void SetDescription(string description)
	{
		desText.text = description;
		if (SingletonDontDestroy<SettingManager>.Instance.Language == 0)
		{
			desText.alignment = TextAnchor.UpperLeft;
			desText.lineSpacing = lineSpacing * 1.1f;
			desText.fontSize = Mathf.RoundToInt((float)fontSize * 1.1f);
		}
		else
		{
			desText.alignment = TextAnchor.UpperCenter;
			desText.lineSpacing = lineSpacing;
			desText.fontSize = fontSize;
		}
	}

	private void ShowBigName()
	{
		bigNameText.gameObject.SetActive(value: true);
		nameText.gameObject.SetActive(value: false);
		desText.gameObject.SetActive(value: false);
	}

	private void ShowNormal()
	{
		bigNameText.gameObject.SetActive(value: false);
		nameText.gameObject.SetActive(value: true);
		desText.gameObject.SetActive(value: true);
	}

	private void SetBigSize()
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Kill();
		}
		base.transform.localScale = Vector3.one * 1f;
		currentCardSize = CardSize.Big;
		ShowNormal();
	}

	public void SetFixedSize(float scaleValue)
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Kill();
		}
		base.transform.localScale = Vector3.one * scaleValue;
		currentCardSize = CardSize.Big;
	}

	private void SetMiniSize()
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Kill();
		}
		base.transform.localScale = Vector3.one * 0.3f;
		currentCardSize = CardSize.Mini;
		ShowBigName();
	}

	protected void TurnToBigSize()
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Complete();
		}
		cardScaleTween = base.transform.DOScale(1f, 0.3f);
		currentCardSize = CardSize.Big;
		ShowNormal();
	}

	private void TurnToMiddleSize()
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Complete();
		}
		cardScaleTween = base.transform.DOScale(0.5f, 0.3f);
		currentCardSize = CardSize.Middle;
	}

	public void TurnToMiniSize()
	{
		if (cardScaleTween != null && cardScaleTween.IsActive())
		{
			cardScaleTween.Complete();
		}
		cardScaleTween = base.transform.DOScale(0.3f, 0.3f);
		currentCardSize = CardSize.Mini;
		ShowBigName();
	}

	private void MoveCard(Vector3 position, float time, Action callback = null)
	{
		if (cardMoveTween != null && cardMoveTween.IsActive())
		{
			cardMoveTween.Pause();
		}
		cardMoveTween = base.transform.DOMove(position, time).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			callback?.Invoke();
		});
	}

	public void LocalMoveCard(Vector3 localPosition, float time, Action callback = null)
	{
		if (cardMoveTween != null && cardMoveTween.IsActive())
		{
			cardMoveTween.Kill();
		}
		cardMoveTween = base.transform.DOLocalMove(localPosition, time).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			callback?.Invoke();
		});
	}

	public void MoveRotateCard(Quaternion startQua, Quaternion endQua, float time, Action callback = null)
	{
		if (cardRotateTween != null && cardRotateTween.IsActive())
		{
			cardRotateTween.Complete();
		}
		base.transform.localRotation = startQua;
		cardRotateTween = base.transform.DOLocalRotate(endQua.eulerAngles, time).OnComplete(delegate
		{
			callback?.Invoke();
		});
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.pointerId.IsPointerInputLegal() && !Singleton<GameManager>.Instance.Player.IsPlayerCastingCard && !isThisCardCasting && !isUsualCardAniming && Application.isFocused)
		{
			isPointDown = true;
			OnPointerDownHandler();
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
		isPointEnter = true;
		if (!Singleton<GameManager>.Instance.Player.IsPlayerCastingCard && !isUsualCardAniming)
		{
			CardForward();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isPointEnter = false;
		if (!isThisCardCasting)
		{
			CardBackward();
		}
	}
}
