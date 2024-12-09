using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RewardCaseBlock : SpecialEventBlock
{
	private Image caseImg;

	private Tween shakeTween;

	public float shakeDuration = 0.4f;

	public Vector3 shakeStrength = Vector3.back;

	public int shakeVibroto = 10;

	public float randomness = 90f;

	protected override void OnAwake()
	{
		base.OnAwake();
		caseImg = base.transform.Find("CaseImg").GetComponent<Image>();
	}

	private void OnEnable()
	{
		shakeTween = caseImg.transform.DOShakeRotation(shakeDuration, shakeStrength, shakeVibroto, randomness).SetLoops(-1);
	}

	private void OnDisable()
	{
		if (shakeTween != null && shakeTween.IsActive())
		{
			shakeTween.Kill();
		}
	}
}
