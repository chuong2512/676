using System;
using System.Collections.Generic;
using UnityEngine;

public class PlotIlluBookPanel : MonoBehaviour, IllustratedBooksUI.IIlluPanel
{
	public Material ShowMat;

	public Material HideMat;

	private Transform contentRoot;

	private UIAnim_Common anim;

	private Queue<SingleIlluPlotCtrl> allPlotPools = new Queue<SingleIlluPlotCtrl>();

	private List<SingleIlluPlotCtrl> allShowingCtrls = new List<SingleIlluPlotCtrl>();

	public IllustratedBooksUI ParentPanel { get; private set; }

	private void Awake()
	{
		contentRoot = base.transform.Find("PlotShowPanel/Mask/Content");
		anim = GetComponent<UIAnim_Common>();
		anim.Init();
	}

	private SingleIlluPlotCtrl GetSingleIlluPlotCtrl()
	{
		if (allPlotPools.Count > 0)
		{
			SingleIlluPlotCtrl singleIlluPlotCtrl = allPlotPools.Dequeue();
			singleIlluPlotCtrl.gameObject.SetActive(value: true);
			return singleIlluPlotCtrl;
		}
		return SingletonDontDestroy<ResourceManager>.Instance.LoadPrefabInstace("SingleIlluPlotCtrl", "Prefabs", contentRoot).GetComponent<SingleIlluPlotCtrl>();
	}

	private void RecycleAllShowingCtrls()
	{
		if (allShowingCtrls.Count > 0)
		{
			for (int i = 0; i < allShowingCtrls.Count; i++)
			{
				allShowingCtrls[i].gameObject.SetActive(value: false);
				allPlotPools.Enqueue(allShowingCtrls[i]);
			}
			allShowingCtrls.Clear();
		}
	}

	public void Show(IllustratedBooksUI parentPanel, Action<int, int> unlockProgressAction)
	{
		ParentPanel = parentPanel;
		base.gameObject.SetActive(value: true);
		RecycleAllShowingCtrls();
		int num = 0;
		Dictionary<string, PlotData> allPlotDatas = DataManager.Instance.AllPlotDatas;
		foreach (KeyValuePair<string, PlotData> item in allPlotDatas)
		{
			SingleIlluPlotCtrl singleIlluPlotCtrl = GetSingleIlluPlotCtrl();
			bool flag = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked(item.Key);
			singleIlluPlotCtrl.transform.SetAsLastSibling();
			singleIlluPlotCtrl.LoadPlot(item.Value, flag, flag ? ShowMat : HideMat);
			allShowingCtrls.Add(singleIlluPlotCtrl);
			if (flag)
			{
				num++;
			}
		}
		unlockProgressAction(allPlotDatas.Count, num);
		anim.StartAnim();
		List<CanvasGroup> list = new List<CanvasGroup>();
		foreach (SingleIlluPlotCtrl allShowingCtrl in allShowingCtrls)
		{
			list.Add(allShowingCtrl.CanvasGroup);
		}
		anim.SetSlotsAnim(list);
		ParentPanel.SetScrolllbar();
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
