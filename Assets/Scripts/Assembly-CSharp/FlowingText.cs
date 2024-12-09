using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlowingText : MonoBehaviour
{
	private Text flowingText;

	private Outline flowingOutline;

	private bool isRecycled;

	public float speed = 0.4f;

	private void Awake()
	{
		flowingText = base.transform.Find("FlowingText").GetComponent<Text>();
		flowingOutline = base.transform.Find("FlowingText").GetComponent<Outline>();
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

	public void LoadFlowingUpText(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, float scale, Action callback = null, float waitTime = 1f)
	{
		flowingText.text = content;
		flowingText.color = textColor;
		flowingOutline.effectColor = outlineColor;
		base.transform.position = target.position + offsetPos + Vector3.up * 0.4f;
		base.transform.localScale = Vector3.zero;
		if (isSetParent)
		{
			base.transform.SetParent(target, worldPositionStays: true);
		}
		base.transform.DOScale(scale, 0.2f).OnComplete(delegate
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(StandingMoveUpCount_IE(callback, waitTime));
			}
			else
			{
				callback?.Invoke();
			}
		});
	}

	public void LoadFlowingDownText(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Action callback = null)
	{
		flowingText.text = content;
		flowingText.color = textColor;
		flowingOutline.effectColor = outlineColor;
		base.transform.position = target.position + offsetPos;
		base.transform.localScale = Vector3.zero;
		if (isSetParent)
		{
			base.transform.SetParent(target, worldPositionStays: true);
		}
		base.transform.DOScale(0.01f, 0.2f).OnComplete(delegate
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(StandingMoveDownCount_IE(callback));
			}
			else
			{
				callback?.Invoke();
			}
		});
	}

	public void LoadFlowingText(string content, Color textColor, Color outlineColor, Transform target, bool isSetParent, Vector3 offsetPos, Vector2 horizontalRange, Vector2 heightRange, Action callback = null)
	{
		flowingText.text = content;
		flowingText.color = textColor;
		flowingOutline.effectColor = outlineColor;
		base.transform.position = target.position + offsetPos;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(0.01f, 0.2f);
		Vector3 vector = target.position + offsetPos;
		bool flag = UnityEngine.Random.value > 0.5f;
		float num = vector.x + (flag ? UnityEngine.Random.Range(0f - horizontalRange.y, 0f - horizontalRange.x) : UnityEngine.Random.Range(horizontalRange.x, horizontalRange.y));
		float y = vector.y + UnityEngine.Random.Range(heightRange.x, heightRange.y);
		SecondOrderEquation equation = SecondOrderFunc.GetSecondOrderEquation(point2: new Vector2(num, vector.y), point1: vector, y: y);
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(MoveByEquation_IE(equation, flag, num, callback));
		}
		else
		{
			callback?.Invoke();
		}
	}

	private IEnumerator MoveByEquation_IE(SecondOrderEquation equation, bool isLeft, float endX, Action callback)
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
		callback?.Invoke();
		RecycleText();
	}

	private IEnumerator StandingMoveUpCount_IE(Action callback, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		MoveUp(callback);
	}

	private IEnumerator StandingMoveDownCount_IE(Action callback)
	{
		yield return new WaitForSeconds(1f);
		MoveDown(callback);
	}

	private void MoveUp(Action callback)
	{
		float endValue = base.transform.position.y + 0.5f;
		base.transform.DOMoveY(endValue, 1f);
		flowingText.DOColor(Color.clear, 1f).OnComplete(delegate
		{
			callback?.Invoke();
			RecycleText();
		});
	}

	private void MoveDown(Action callback)
	{
		float endValue = base.transform.position.y - 0.5f;
		base.transform.DOMoveY(endValue, 1f);
		flowingText.DOColor(Color.clear, 1f).OnComplete(delegate
		{
			callback?.Invoke();
			RecycleText();
		});
	}

	public void RecycleText()
	{
		base.transform.DOKill();
		flowingText.DOKill();
		if (!isRecycled)
		{
			isRecycled = true;
			Singleton<GameHintManager>.Instance.RecycleFlowingText(this);
		}
	}
}
