using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataUI : EscCloseUIView
{
	private UIAnim_UserDataUI anim;

	public Sprite SingleUserDataNormalSprite;

	public Sprite SingleUserDataNoUseSprite;

	public Sprite SingleUserDataChosenSprite;

	private SingleUserDataCtrl[] allSingleUserDataCtrls;

	private Transform chosenHighlightTrans;

	private bool isChanged;

	public override string UIViewName => "UserDataUI";

	public override string UILayerName => "TipsLayer";

	public override void ShowView(params object[] objs)
	{
		base.gameObject.SetActive(value: true);
		isChanged = false;
		OnShowUserDataUI();
		anim.StartAnim();
	}

	public override void HideView()
	{
		base.gameObject.SetActive(value: false);
	}

	public override void OnDestroyUI()
	{
	}

	protected override void OnHide()
	{
		base.OnHide();
		OnClickReturnBtn();
	}

	public override void OnSpawnUI()
	{
		base.transform.Find("Mask/ReturnBtnBg/ReturnBtn").GetComponent<Button>().onClick.AddListener(OnClickReturnBtn);
		allSingleUserDataCtrls = new SingleUserDataCtrl[3];
		Transform transform = base.transform.Find("Mask/SingleUserDataRoot");
		for (int i = 0; i < allSingleUserDataCtrls.Length; i++)
		{
			allSingleUserDataCtrls[i] = transform.GetChild(i).GetComponent<SingleUserDataCtrl>();
			allSingleUserDataCtrls[i].Init(this);
		}
		chosenHighlightTrans = base.transform.Find("Mask/SingleUserDataRoot/Highlight");
		anim = GetComponent<UIAnim_UserDataUI>();
		anim.Init();
	}

	private void OnShowUserDataUI()
	{
		foreach (KeyValuePair<int, string> item in SingletonDontDestroy<Game>.Instance.AppData.AllUserDataInfo)
		{
			if (item.Value.IsNullOrEmpty())
			{
				allSingleUserDataCtrls[item.Key].ClearUserData();
				continue;
			}
			UserData userData = GameSave.LoadUserData(item.Key);
			allSingleUserDataCtrls[item.Key].LoadUserData(userData.UserName, userData.UserPlaySeconds, userData.UserRebirthCount);
			if (userData.UserName == SingletonDontDestroy<Game>.Instance.CurrentUserData.UserName)
			{
				SetHighlight(allSingleUserDataCtrls[item.Key]);
			}
		}
	}

	private void SetHighlight(SingleUserDataCtrl ctrl)
	{
		Vector3 position = chosenHighlightTrans.position;
		position.y = ctrl.transform.position.y;
		chosenHighlightTrans.position = position;
	}

	public void OnConfirmDeleteCurrentChosenUserData(int index)
	{
		int num = SingletonDontDestroy<Game>.Instance.AppData.DeleteUserData(index);
		allSingleUserDataCtrls[index].ClearUserData();
		if (num != index)
		{
			isChanged = true;
			SetHighlight(allSingleUserDataCtrls[num]);
		}
	}

	public void OnConfirmChosenData(SingleUserDataCtrl ctrl)
	{
		if (SingletonDontDestroy<Game>.Instance.SetNewUserData(ctrl.index))
		{
			GameSave.DeleteOldSaveData();
			(SingletonDontDestroy<UIManager>.Instance.GetView("GameMenuUI") as GameMenuUI).OnConfirmSwitchUserData();
		}
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}

	private void OnClickReturnBtn()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("退出按钮");
		if (isChanged)
		{
			(SingletonDontDestroy<UIManager>.Instance.GetView("GameMenuUI") as GameMenuUI).OnConfirmSwitchUserData();
		}
		SingletonDontDestroy<UIManager>.Instance.HideView(this);
	}
}
