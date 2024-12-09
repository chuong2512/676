using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimePiecesCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private Text amountText;

	private RectTransform m_RectTransform;

	private bool isEnter;

	private bool isShow;

	private Transform vfxPoint;

	private Tween moveTween;

	private void Awake()
	{
		m_RectTransform = GetComponent<RectTransform>();
		amountText = base.transform.Find("Amount").GetComponent<Text>();
		isShow = false;
		vfxPoint = base.transform.Find("VfxPoint");
	}

	public void SetAmount(int amount)
	{
		amountText.text = amount.ToString();
	}

	public void UpdateAmount(int amount)
	{
		StartCoroutine(Update_IE(amount));
	}

	private IEnumerator Update_IE(int amount)
	{
		Show();
		yield return new WaitForSeconds(0.3f);
		SetAmount(amount);
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("effect_ui_shikongsuipianhuode");
		vfxBase.transform.position = vfxPoint.position;
		vfxBase.Play();
		yield return new WaitForSeconds(1f);
		if (!isEnter)
		{
			Hide();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isEnter = true;
		Show();
	}

	public void Show()
	{
		if (!isShow)
		{
			isShow = true;
			if (moveTween != null && moveTween.IsActive())
			{
				moveTween.Kill();
			}
			moveTween = m_RectTransform.DOAnchorPosX(-50f, 0.2f);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isEnter = false;
		Hide();
	}

	public void Hide()
	{
		if (isShow)
		{
			isShow = false;
			if (moveTween != null && moveTween.IsActive())
			{
				moveTween.Kill();
			}
			moveTween = m_RectTransform.DOAnchorPosX(0f, 0.2f);
		}
	}
}
