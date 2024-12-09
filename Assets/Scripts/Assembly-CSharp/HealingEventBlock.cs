using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealingEventBlock : SpecialEventBlock
{
	public int MinMoveY;

	public int MaxMoveY;

	private Tween healingMoveTween;

	private Image healingImg;

	protected override void OnAwake()
	{
		base.OnAwake();
		healingImg = base.transform.Find("Shadow/Img").GetComponent<Image>();
	}

	private void OnEnable()
	{
		float duration = Random.Range(1f, 1.5f);
		healingImg.transform.localPosition = new Vector3(0f, MinMoveY, 0f);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(healingImg.transform.DOLocalMoveY(MaxMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.Append(healingImg.transform.DOLocalMoveY(MinMoveY, duration).SetEase(Ease.InOutQuad));
		sequence.SetLoops(-1);
		healingMoveTween = sequence;
	}

	private void OnDisable()
	{
		if (healingMoveTween != null && healingMoveTween.IsActive())
		{
			healingMoveTween.Kill();
		}
	}
}
