using DG.Tweening;
using UnityEngine;

public class SkillAltarEventBlock : SpecialEventBlock
{
	private Transform[] allLights;

	private Tween[] allLightTweens;

	protected override void OnAwake()
	{
		base.OnAwake();
		allLights = new Transform[4];
		allLightTweens = new Tween[4];
		for (int i = 0; i < 4; i++)
		{
			allLights[i] = base.transform.Find("Img").GetChild(i);
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < allLights.Length; i++)
		{
			allLights[i].localScale = Vector3.one * Random.value;
			allLights[i].localPosition = new Vector3(Random.Range(-40, 40), (float)Random.Range(-20, 20) + 133f);
			float endValue = allLights[i].localPosition.y + (float)Random.Range(5, 50);
			float y = allLights[i].localPosition.y;
			float duration = Random.Range(2f, 8f);
			Sequence sequence = DOTween.Sequence();
			sequence.Append(allLights[i].DOLocalMoveY(endValue, duration)).SetEase(Ease.Linear);
			sequence.Append(allLights[i].DOLocalMoveY(y, duration)).SetEase(Ease.Linear);
			sequence.SetLoops(-1);
			allLightTweens[i] = sequence;
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < allLightTweens.Length; i++)
		{
			allLightTweens[i].Kill();
		}
	}
}
