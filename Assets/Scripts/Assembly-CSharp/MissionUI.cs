using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : UIView
{
	private Text missionContent;

	private Mission currentMission;

	private Transform showingPoint;

	private Transform hidingPoint;

	private bool isShowing;

	private Transform bgRoot;

	private GuideTipsBtn _guideTipsBtn;

	public override string UIViewName => "MissionUI";

	public override string UILayerName => "TipsLayer";

	public Transform GuideTipsBtnTrans => _guideTipsBtn.transform;

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
		isShowing = false;
	}

	public override void OnDestroyUI()
	{
		Debug.Log("Destroy Mission UI ...");
	}

	public override void OnSpawnUI()
	{
		bgRoot = base.transform.Find("Root/Bg");
		showingPoint = base.transform.Find("Root/ShowingPoint");
		hidingPoint = base.transform.Find("Root/HidingPoint");
		missionContent = base.transform.Find("Root/Bg/MissionContent").GetComponent<Text>();
		_guideTipsBtn = base.transform.Find("Root/Bg/GuideTipsBtn").GetComponent<GuideTipsBtn>();
	}

	public void AddGuideTips(List<string> allGuideTips)
	{
		_guideTipsBtn.ActiveGuideTips();
		_guideTipsBtn.AddGuideTips(allGuideTips);
	}

	public void ShowMissionDes(Mission mission)
	{
		if (!isShowing)
		{
			isShowing = true;
			missionContent.text = mission.GetMissionDescription();
			currentMission = mission;
			bgRoot.transform.position = hidingPoint.position;
			bgRoot.DOMoveY(showingPoint.position.y, 0.5f).SetEase(Ease.OutBack);
		}
		else
		{
			StartCoroutine(ShowingMissionDes_IE(mission));
		}
	}

	private IEnumerator ShowingMissionDes_IE(Mission mission)
	{
		while (isShowing)
		{
			yield return null;
		}
		ShowMissionDes(mission);
	}

	public void HideMissionDes()
	{
		currentMission = null;
		bgRoot.DOMoveY(hidingPoint.position.y, 0.3f).SetEase(Ease.OutBack).OnComplete(delegate
		{
			isShowing = false;
		});
	}
}
