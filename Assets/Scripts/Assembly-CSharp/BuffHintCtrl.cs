using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuffHintCtrl : MonoBehaviour
{
	private const string BuffIconPath = "Sprites/BuffIcon";

	private Image buffIconImg;

	private Text buffNameText;

	private GameHintManager gameHintManager;

	private bool isRecycled;

	private CanvasGroup _canvasGroup;

	private Transform target;

	private Coroutine moveCor;

	private bool isMovingUp;

	private Tween moveUpTween;

	private void Awake()
	{
		buffIconImg = base.transform.Find("BuffHintCtrl").GetComponent<Image>();
		buffNameText = base.transform.Find("BuffHintCtrl/BuffName").GetComponent<Text>();
		_canvasGroup = GetComponent<CanvasGroup>();
		isRecycled = false;
	}

	private void OnDisable()
	{
		Recycle();
	}

	public void SetBuffHintNotBeRecycled()
	{
		isRecycled = false;
	}

	public void ShowBuff(GameHintManager hintManager, Transform targetTrans, float scale, BuffType buffType, Color nameColor)
	{
		target = targetTrans;
		gameHintManager = hintManager;
		buffIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(buffType.ToString(), "Sprites/BuffIcon");
		buffNameText.text = (buffType.ToString() + "_Name").LocalizeText();
		buffNameText.color = nameColor;
		_canvasGroup.alpha = 0f;
		if (base.gameObject.activeSelf)
		{
			moveCor = StartCoroutine(BuffHintMove(scale));
		}
		else
		{
			Recycle();
		}
	}

	private IEnumerator BuffHintMove(float scale)
	{
		base.transform.localScale = Vector3.zero;
		base.transform.position = target.position;
		yield return new WaitForSeconds(0.4f);
		_canvasGroup.DOFade(1f, 0.4f);
		base.transform.DOScale(1f * scale, 0.4f).SetEase(Ease.OutBack);
		yield return new WaitForSeconds(1.6f);
		while (isMovingUp)
		{
			yield return null;
		}
		_canvasGroup.DOFade(0f, 0.3f).OnComplete(Recycle);
	}

	private void Recycle()
	{
		if (!isRecycled)
		{
			isRecycled = true;
			gameHintManager.RecycleBuffHintCtrl(target, this);
		}
	}

	public void MoveUp()
	{
		if (moveUpTween != null && moveUpTween.IsActive())
		{
			moveUpTween.Complete();
		}
		isMovingUp = true;
		float endValue = base.transform.position.y + 0.45f;
		moveUpTween = base.transform.DOMoveY(endValue, 0.2f).OnComplete(delegate
		{
			isMovingUp = false;
		});
	}
}
