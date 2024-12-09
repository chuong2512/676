using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSummary_GameProgress : MonoBehaviour
{
	[Header("参数")]
	public Sprite GameFailedEndStageBottomSprite;

	public Sprite GamePassEndStageBottomSprite;

	public Sprite NormalBottomSprite;

	public Sprite HiddenBottomSprite;

	public Sprite NormalProgressSprite;

	public Sprite HiddenProgressSprite;

	public Sprite NormalProgressBgSprite;

	public Sprite HiddenProgressBgSPrite;

	public Sprite BossNotTouchedSprite;

	public Transform NormalStagePoint;

	public Transform HiddenStagePoint;

	private Text timeText;

	private Image progressBottomImg;

	private Image progressImg;

	private Text endStageText;

	private Image headPointImg;

	private Image endStageBottomImg;

	[HideInInspector]
	public Image[] bossPoints;

	private Image progressBgImg;

	public float[] progressImgFillAmountArray;

	public Transform[] headPointArray;

	private void Awake()
	{
		timeText = base.transform.Find("Time").GetComponent<Text>();
		progressImg = base.transform.Find("Bottom/Progress").GetComponent<Image>();
		endStageText = base.transform.Find("EndStageBottom/Text").GetComponent<Text>();
		endStageBottomImg = base.transform.Find("EndStageBottom").GetComponent<Image>();
		headPointImg = base.transform.Find("Point").GetComponent<Image>();
		progressBottomImg = base.transform.Find("Bottom").GetComponent<Image>();
		bossPoints = new Image[4];
		for (int i = 0; i < bossPoints.Length; i++)
		{
			bossPoints[i] = base.transform.Find("BossPoint").GetChild(i).GetComponent<Image>();
		}
		progressBgImg = GetComponent<Image>();
	}

	public void LoadGameProgress(RecordData data)
	{
		int num = (data.mapLevel - 1) * 3 + data.mapLayer - 1;
		progressImg.material.SetFloat("_Threshold", (num < progressImgFillAmountArray.Length) ? progressImgFillAmountArray[num] : 1f);
		headPointImg.transform.position = headPointArray[num].position;
		endStageBottomImg.sprite = (data.isGamePass ? GamePassEndStageBottomSprite : GameFailedEndStageBottomSprite);
		endStageText.text = (data.isGamePass ? "GamePass".LocalizeText() : (data.mapLevel + "-" + data.mapLayer));
		int num2 = data.timeUsed / 3600;
		int num3 = (data.timeUsed - num2 * 3600) / 60;
		int num4 = data.timeUsed - num2 * 3600 - num3 * 60;
		if (num2 > 99)
		{
			timeText.text = $"{99}:{59}:{59}";
		}
		else
		{
			timeText.text = string.Format("{0}:{1}:{2}", num2.ToString("D2"), num3.ToString("D2"), num4.ToString("D2"));
		}
		AdjustLevelInfo(data.mapLevel);
		LoadBossInfo(data);
	}

	private void LoadBossInfo(RecordData data)
	{
		bossPoints[bossPoints.Length - 1].gameObject.SetActive(data.mapLevel == 4);
		List<string> allClearBossHeapIDList = data.allClearBossHeapIDList;
		int num = allClearBossHeapIDList?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			BossHeapData bossHeapData = DataManager.Instance.GetBossHeapData(allClearBossHeapIDList[i]);
			bossPoints[i].sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(bossHeapData.PassIcon, "Sprites/SettlementBoss");
		}
		for (int j = num; j < bossPoints.Length; j++)
		{
			bossPoints[j].sprite = BossNotTouchedSprite;
		}
	}

	private void AdjustLevelInfo(int level)
	{
		if (level == 4)
		{
			progressImg.sprite = HiddenProgressSprite;
			progressBottomImg.sprite = HiddenBottomSprite;
			endStageBottomImg.transform.position = HiddenStagePoint.position;
			progressBgImg.sprite = HiddenProgressBgSPrite;
		}
		else
		{
			progressImg.sprite = NormalProgressSprite;
			progressBottomImg.sprite = NormalBottomSprite;
			endStageBottomImg.transform.position = NormalStagePoint.position;
			progressBgImg.sprite = NormalProgressBgSprite;
		}
	}

	public void SetOccupation(OccupationData occupationData)
	{
		headPointImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.GameSummaryHeadPointSpriteName, occupationData.DefaultSpritePath);
	}
}
