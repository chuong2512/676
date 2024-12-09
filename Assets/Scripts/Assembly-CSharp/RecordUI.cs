using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : EscCloseUIView
{
	public Sprite GameFailedSprite;

	public Sprite GamePassSprite;

	private Transform recordRoot;

	private RectTransform recordRootRect;

	private UIAnim_Record anim;

	private List<SingleRecordCtrl> allShowingRecords = new List<SingleRecordCtrl>();

	private Queue<SingleRecordCtrl> allRecordsPools = new Queue<SingleRecordCtrl>();

	public override string UIViewName => "RecordUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		ShowAllRecards();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		RecycleAllRecords();
	}

	public override void OnDestroyUI()
	{
	}

	public override void OnSpawnUI()
	{
		InitUI();
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickCloseBtn();
	}

	private void InitUI()
	{
		base.transform.Find("Mask/ReturnBtnBg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickCloseBtn);
		recordRoot = base.transform.Find("Mask/Bg/Mask/RecordRoot");
		recordRootRect = recordRoot.GetComponent<RectTransform>();
		anim = GetComponent<UIAnim_Record>();
		anim.Init();
	}

	private void ShowAllRecards()
	{
		List<RecordData> allRecordDatas = SingletonDontDestroy<Game>.Instance.CurrentUserData.AllRecordDatas;
		List<CanvasGroup> list = new List<CanvasGroup>();
		for (int i = 0; i < allRecordDatas.Count; i++)
		{
			SingleRecordCtrl singleRecord = GetSingleRecord();
			singleRecord.transform.SetSiblingIndex(i);
			int index = allRecordDatas.Count - 1 - i;
			singleRecord.LoadRecord(this, allRecordDatas[index], index);
			allShowingRecords.Add(singleRecord);
			list.Add(singleRecord.CanvasGroup);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(recordRootRect);
		anim.SetCgs(list);
		anim.StartAnim();
	}

	private SingleRecordCtrl GetSingleRecord()
	{
		if (allRecordsPools.Count > 0)
		{
			SingleRecordCtrl singleRecordCtrl = allRecordsPools.Dequeue();
			singleRecordCtrl.gameObject.SetActive(value: true);
			return singleRecordCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleRecord", "Prefabs", recordRoot).GetComponent<SingleRecordCtrl>();
	}

	private void RecycleAllRecords()
	{
		if (allShowingRecords.Count > 0)
		{
			for (int i = 0; i < allShowingRecords.Count; i++)
			{
				allShowingRecords[i].gameObject.SetActive(value: false);
				allRecordsPools.Enqueue(allShowingRecords[i]);
			}
			allShowingRecords.Clear();
		}
	}

	public void OnClickShowRecordDetail(int index)
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("GameSummaryUI", SingletonDontDestroy<Game>.Instance.CurrentUserData.GetRecordDataByIndex(index), null, true);
	}

	private void OnClickCloseBtn()
	{
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
	}
}
