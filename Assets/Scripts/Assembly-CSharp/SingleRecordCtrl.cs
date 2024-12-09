using UnityEngine;
using UnityEngine.UI;

public class SingleRecordCtrl : MonoBehaviour
{
	private Image occupationIconImg;

	private Text timeText;

	private Image endStateImg;

	private Text stageText;

	private int currentShowingRecordIndex;

	private RecordUI _recordUi;

	public CanvasGroup CanvasGroup { get; set; }

	private void Awake()
	{
		occupationIconImg = base.transform.Find("OccupationIcon").GetComponent<Image>();
		timeText = base.transform.Find("Time").GetComponent<Text>();
		endStateImg = base.transform.Find("EndState").GetComponent<Image>();
		stageText = base.transform.Find("EndState/StageText").GetComponent<Text>();
		GetComponent<Button>().onClick.AddListener(OnClickReadBtn);
		CanvasGroup = base.transform.GetComponent<CanvasGroup>();
	}

	public void LoadRecord(RecordUI recordUi, RecordData data, int index)
	{
		OccupationData occupationData = DataManager.Instance.GetOccupationData(data.PlayerOccupation);
		_recordUi = recordUi;
		currentShowingRecordIndex = index;
		occupationIconImg.sprite = SingletonDontDestroy<ResourceManager>.Instance.LoadSprite(occupationData.RecordIllusrationName, occupationData.DefaultSpritePath);
		timeText.text = string.Format("RecordTimeFormat".LocalizeText(), data.Year, data.Month.ToString("D2"), data.Day.ToString("D2"), data.Hour.ToString("D2"), data.Minute.ToString("D2"));
		endStateImg.sprite = (data.isGamePass ? recordUi.GamePassSprite : recordUi.GameFailedSprite);
		stageText.text = (data.isGamePass ? "GamePass".LocalizeText() : (data.mapLevel + "-" + data.mapLayer));
	}

	private void OnClickReadBtn()
	{
		_recordUi.OnClickShowRecordDetail(currentShowingRecordIndex);
	}
}
