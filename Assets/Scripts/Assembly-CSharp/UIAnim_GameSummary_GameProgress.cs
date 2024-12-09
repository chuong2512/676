using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim_GameSummary_GameProgress : UIAnimBase
{
	public GameSummary_GameProgress progress;

	public Transform player;

	public CanvasGroup allBg;

	public Image endStageBg;

	public Text endStageTxt;

	public Text hrTxt;

	public Image fillImg;

	public Image progressImg;

	public Transform playerStartPt;

	private List<Tween> tweenList = new List<Tween>();

	private bool isMoving;

	private Vector2 currentHeadPos;

	private float currentfillAmount;

	private static readonly int Threshold = Shader.PropertyToID("_Threshold");

	public override void StartAnim()
	{
		StopAllCoroutines();
		StartCoroutine(StartAnimCo());
	}

	private IEnumerator StartAnimCo()
	{
		tweenList.Add(allBg.DOFade(1f, 0.5f));
		yield return new WaitForSeconds(0.4f);
		tweenList.Add(endStageBg.DOFade(1f, 0.3f));
		tweenList.Add(hrTxt.DOFade(1f, 1f));
		player.DOScale(1f, 0.4f);
		yield return new WaitForSeconds(0.2f);
		tweenList.Add(endStageTxt.DOFade(1f, 0.5f));
		tweenList.Add(fillImg.material.DOFloat((currentfillAmount * 540f + 25f) / 566f, "_Threshold", 1.5f).SetEase(Ease.InOutCubic));
		player.Rotate(Vector3.back, 10f);
		int i = 0;
		tweenList.Add(player.DOMove(currentHeadPos, 1.5f).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			player.DORotate(Vector3.zero, 0.2f);
			for (; i < progress.headPointArray.Length; i++)
			{
				if ((i + 1) % 3 != 0 || i > 8 || i < 2)
				{
					if (i != 10)
					{
						continue;
					}
					tweenList.Add(progress.bossPoints[3].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
				}
				tweenList.Add(progress.bossPoints[(i + 1) / 3 - 1].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
			}
		})
			.OnUpdate(delegate
			{
				if (i < progress.headPointArray.Length && player.transform.position.x >= progress.headPointArray[i].transform.transform.position.x)
				{
					if ((i + 1) % 3 != 0 || i > 8)
					{
						if (i == 10)
						{
							tweenList.Add(progress.bossPoints[3].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
							SingletonDontDestroy<AudioManager>.Instance.PlaySound("战绩进度条大泡泡", 1f + 0.1f * (float)i);
						}
						else
						{
							SingletonDontDestroy<AudioManager>.Instance.PlaySound("战绩进度条小泡泡", 1f + 0.1f * (float)i);
						}
					}
					else
					{
						tweenList.Add(progress.bossPoints[(i + 1) / 3 - 1].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
						SingletonDontDestroy<AudioManager>.Instance.PlaySound("战绩进度条大泡泡", 1f + 0.1f * (float)i);
					}
					i++;
				}
			}));
		float time = 25f / currentfillAmount / 566f * 1.5f;
		yield return new WaitForSeconds(time);
		tweenList.Add(progressImg.material.DOFloat(currentfillAmount, "_Threshold", 1.5f - time).SetEase(Ease.InOutCubic));
	}

	public void Reset()
	{
		foreach (Tween tween in tweenList)
		{
			tween.KillTween();
		}
		fillImg.material.SetFloat("_Threshold", 0f);
		currentHeadPos = player.transform.position;
		currentfillAmount = progressImg.material.GetFloat(Threshold);
		player.transform.position = playerStartPt.position;
		progressImg.material.SetFloat(Threshold, 0f);
		player.transform.localScale = Vector3.zero;
		endStageTxt.WithCol(0f);
		endStageBg.WithCol(0f);
		allBg.alpha = 0f;
		hrTxt.WithCol(0f);
		Image[] bossPoints = progress.bossPoints;
		for (int i = 0; i < bossPoints.Length; i++)
		{
			bossPoints[i].transform.localScale = Vector3.zero;
		}
	}
}
