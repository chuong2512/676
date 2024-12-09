using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlowingText : MonoBehaviour
{
	private Text contentText;

	private Image iconImg;

	private CanvasGroup cg;

	private Image bgImg;

	private bool isRecycled;

	public float speed = 0.4f;

	private void Awake()
	{
		contentText = base.transform.Find("DamageFlowingText/Content").GetComponent<Text>();
		cg = base.transform.Find("DamageFlowingText").GetComponent<CanvasGroup>();
		bgImg = base.transform.Find("DamageFlowingText").GetComponent<Image>();
		iconImg = base.transform.Find("DamageFlowingText/Icon").GetComponent<Image>();
		isRecycled = false;
	}

	private void OnDisable()
	{
		RecycleText();
	}

	public void SetFlowingTextNotBeRecycled()
	{
		isRecycled = false;
	}

	public void ShowDamageFlowingText(GameHintManager manager, string content, float scale, Font font, Sprite bgSprite, bool isAbsDmg)
	{
		if (isAbsDmg)
		{
			iconImg.gameObject.SetActive(value: true);
			iconImg.sprite = manager.AbsDmgIconSprite;
		}
		else
		{
			iconImg.gameObject.SetActive(value: false);
		}
		bgImg.enabled = true;
		bgImg.sprite = bgSprite;
		contentText.font = font;
		cg.alpha = 1f;
		contentText.text = content;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(scale, 0.25f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(NormalDmgCounter_IE());
			}
		});
	}

	private IEnumerator NormalDmgCounter_IE()
	{
		yield return new WaitForSeconds(1.5f);
		cg.DOFade(0f, 0.4f).OnComplete(RecycleText);
	}

	public void ShowArmorDmgFlowingText(GameHintManager manager, string content, float scale, Font font, bool isAbsDmg)
	{
		if (isAbsDmg)
		{
			iconImg.gameObject.SetActive(value: true);
			iconImg.sprite = manager.AbsDmgIconSprite;
		}
		else
		{
			iconImg.gameObject.SetActive(value: false);
		}
		bgImg.enabled = false;
		contentText.font = font;
		cg.alpha = 1f;
		contentText.text = content;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(scale, 0.25f).SetEase(Ease.OutBack);
		MoveByEquation(isLeft: true, base.transform.position, new Vector2(0.5f, 0.75f), new Vector2(0.5f, 0.75f));
	}

	public void ShowPoisonFlowingText(GameHintManager manager, string content, float scale, Font font)
	{
		iconImg.gameObject.SetActive(value: true);
		iconImg.sprite = manager.PoisonDmgIconSprite;
		bgImg.enabled = false;
		contentText.font = font;
		cg.alpha = 1f;
		contentText.text = content;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(scale, 0.25f).SetEase(Ease.OutBack);
		MoveByEquation(isLeft: false, base.transform.position, new Vector2(0.5f, 0.75f), new Vector2(0.5f, 0.75f));
	}

	public void ShowBleedingFlowingText(GameHintManager manager, string content, float scale, Font font)
	{
		iconImg.gameObject.SetActive(value: true);
		iconImg.sprite = manager.BleedingDmgIconSprite;
		bgImg.enabled = false;
		contentText.font = font;
		cg.alpha = 1f;
		contentText.text = content;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(scale, 0.25f).SetEase(Ease.OutBack);
		MoveByEquation(isLeft: false, base.transform.position, new Vector2(0.5f, 0.75f), new Vector2(0.5f, 0.75f));
	}

	private void MoveByEquation(bool isLeft, Vector3 startPoint, Vector2 horizontalRange, Vector2 heightRange)
	{
		float num = startPoint.x + (isLeft ? Random.Range(0f - horizontalRange.y, 0f - horizontalRange.x) : Random.Range(horizontalRange.x, horizontalRange.y));
		float y = startPoint.y + Random.Range(heightRange.x, heightRange.y);
		SecondOrderEquation equation = SecondOrderFunc.GetSecondOrderEquation(point2: new Vector2(num, startPoint.y), point1: startPoint, y: y);
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(MoveByEquation_IE(equation, isLeft, num));
		}
	}

	private IEnumerator MoveByEquation_IE(SecondOrderEquation equation, bool isLeft, float endX)
	{
		if (isLeft)
		{
			while (endX < base.transform.position.x)
			{
				base.transform.position = equation.GetPos(base.transform.position.x - Time.deltaTime * speed);
				yield return null;
			}
		}
		else
		{
			while (base.transform.position.x < endX)
			{
				base.transform.position = equation.GetPos(base.transform.position.x + Time.deltaTime * speed);
				yield return null;
			}
		}
		RecycleText();
	}

	private void RecycleText()
	{
		if (!isRecycled)
		{
			isRecycled = true;
			Singleton<GameHintManager>.Instance.RecycleDamageFlowingText(this);
		}
	}
}
