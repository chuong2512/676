using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SingleArrowCtrl : MonoBehaviour
{
	private static readonly Dictionary<Arrow.ArrowType, string> arrowAssetPathDic = new Dictionary<Arrow.ArrowType, string>
	{
		{
			Arrow.ArrowType.Normal,
			"普通箭头"
		},
		{
			Arrow.ArrowType.Freeze,
			"冰霜箭头"
		},
		{
			Arrow.ArrowType.Poison,
			"剧毒箭头"
		},
		{
			Arrow.ArrowType.Fire,
			"火焰箭头"
		},
		{
			Arrow.ArrowType.Blunt,
			"钝箭头"
		},
		{
			Arrow.ArrowType.Sawtooth,
			"锯齿箭头"
		}
	};

	private const string ArrowPath = "Sprites/Arrow";

	private Tween xMoveTween;

	private Tween yMoveTween;

	private Image dropImg;

	private Image arrowImg;

	private void Awake()
	{
		arrowImg = GetComponent<Image>();
		dropImg = base.transform.Find("Drop").GetComponent<Image>();
		dropImg.gameObject.SetActive(value: false);
	}

	public void MoveY(float y)
	{
		if (yMoveTween != null && yMoveTween.IsActive())
		{
			yMoveTween.Complete();
		}
		yMoveTween = base.transform.DOMoveY(y, 0.2f);
	}

	public void CancelHighlight()
	{
		if (yMoveTween != null && yMoveTween.IsActive())
		{
			yMoveTween.Complete();
		}
		yMoveTween = base.transform.DOLocalMoveY(0f, 0.2f);
	}

	public void ResetLocalPositionY()
	{
		if (yMoveTween != null && yMoveTween.IsActive())
		{
			yMoveTween.Complete();
		}
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = 0f;
		base.transform.localPosition = localPosition;
	}

	public void MoveToZeroLocalPosY()
	{
		if (yMoveTween != null && yMoveTween.IsActive())
		{
			yMoveTween.Complete();
		}
		yMoveTween = base.transform.DOLocalMoveY(0f, 0.2f);
	}

	public void MoveX(float x)
	{
		if (xMoveTween != null && xMoveTween.IsActive())
		{
			xMoveTween.Complete();
		}
		xMoveTween = base.transform.DOMoveX(x, 0.2f);
	}

	public void ShowDropHint(bool isActive)
	{
		dropImg.gameObject.SetActive(isActive);
	}

	public void SetSpecialArrow(Arrow.ArrowType arrowType)
	{
		arrowImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(arrowAssetPathDic[arrowType], "Sprites/Arrow");
	}

	public void HideArrow()
	{
		arrowImg.enabled = false;
	}

	public void ShowArrow()
	{
		arrowImg.enabled = true;
	}
}
