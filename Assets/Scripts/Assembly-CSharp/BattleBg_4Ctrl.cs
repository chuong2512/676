using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BattleBg_4Ctrl : MonoBehaviour
{
	public SpriteRenderer[] starts;

	private void Awake()
	{
		StopAllCoroutines();
		SpriteRenderer[] array = starts;
		foreach (SpriteRenderer spriteRenderer in array)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
			StartCoroutine(EyeCo(spriteRenderer));
		}
	}

	private IEnumerator EyeCo(SpriteRenderer eye)
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(3, 14));
			eye.DOFade(1f, 0.5f);
			yield return new WaitForSeconds(Random.Range(1.5f, 4.5f));
			eye.DOFade(0f, 0.5f);
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
