using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleUserDataCtrl : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private const string SingleUserContentFormatKey = "SingleUserContentFormatKey";

	public Sprite addNormalSprite;

	public Sprite addHighlightSprite;

	public int index;

	private Text contentText;

	private Image bgImg;

	private UserDataUI _userDataUi;

	private bool isContainData;

	private string currentUserName;

	private ulong currentPlaySeconds;

	private int currentPlayTime;

	private Button deleteBtn;

	private Button varifyBtn;

	private Image addImg;

	public void Init(UserDataUI userDataUi)
	{
		_userDataUi = userDataUi;
		contentText = base.transform.Find("Content").GetComponent<Text>();
		bgImg = GetComponent<Image>();
		GetComponent<Button>().onClick.AddListener(OnChooseThisData);
		varifyBtn = base.transform.Find("VarifyBtn").GetComponent<Button>();
		varifyBtn.onClick.AddListener(EditNewName);
		deleteBtn = base.transform.Find("DeleteBtn").GetComponent<Button>();
		deleteBtn.onClick.AddListener(DeleteUserData);
		isContainData = false;
		addImg = base.transform.Find("AddImg").GetComponent<Image>();
	}

	public void LoadUserData(string userName, ulong playSeconds, int playTime)
	{
		SetDataNormal();
		addImg.gameObject.SetActive(value: false);
		isContainData = true;
		currentUserName = userName;
		currentPlaySeconds = playSeconds;
		currentPlayTime = playTime;
		SetContent(userName, playSeconds, playTime);
	}

	private void SetContent(string userName, ulong playSeconds, int playTime)
	{
		ulong num = (ulong)Mathf.Min(999f, playSeconds / 3600uL);
		ulong num2 = (playSeconds - num * 3600) / 60uL;
		ulong num3 = playSeconds - num * 3600 - num2 * 60;
		if (num > 99)
		{
			num = 99uL;
			num2 = (num3 = 59uL);
		}
		contentText.text = string.Format("SingleUserContentFormatKey".LocalizeText(), userName, num.ToString("D2"), num2.ToString("D2"), num3.ToString("D2"), playTime);
	}

	private void OnEndInput(int index, string newName)
	{
		if (AppData.CheckNewName(newName).IsNullOrEmpty())
		{
			currentUserName = newName;
			SetContent(currentUserName, currentPlaySeconds, currentPlayTime);
			SingletonDontDestroy<Game>.Instance.AppData.VarifyNewName(index, newName);
		}
	}

	private void EditNewName()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("修改按钮");
		SingletonDontDestroy<UIManager>.Instance.ShowView("CreateNewUserUI", index, false, new Action<int, string>(OnEndInput), currentUserName);
	}

	private void DeleteUserData()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("删除按钮");
		if (SingletonDontDestroy<Game>.Instance.AppData.UserDataCount == 1)
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowOneChosenSystemHint("OnlySingleUserDataCannotDelete".LocalizeText(), null);
		}
		else
		{
			(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ComfirmDeleteUserData".LocalizeText(), OnComfirmDeleteCurrentChosenUserData);
		}
	}

	private void OnComfirmDeleteCurrentChosenUserData()
	{
		_userDataUi.OnConfirmDeleteCurrentChosenUserData(index);
	}

	public void ClearUserData()
	{
		SetDataNoUse();
		addImg.gameObject.SetActive(value: true);
		contentText.text = string.Empty;
		isContainData = false;
		varifyBtn.gameObject.SetActive(value: false);
		deleteBtn.gameObject.SetActive(value: false);
	}

	private void OnChooseThisData()
	{
		SingletonDontDestroy<AudioManager>.Instance.PlaySound("点击档案");
		if (isContainData)
		{
			TrySwithNewUserData();
		}
		else
		{
			TryAddNewUserData();
		}
	}

	private void TryAddNewUserData()
	{
		SingletonDontDestroy<UIManager>.Instance.ShowView("CreateNewUserUI", index, false, new Action<int, string>(OnComfirmCreateNewUserData), string.Empty);
	}

	private void TrySwithNewUserData()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("ConfirmSwitchUserData".LocalizeText(), OnConfirmSwitchNewUserData);
	}

	private void OnConfirmSwitchNewUserData()
	{
		_userDataUi.OnConfirmChosenData(this);
	}

	private void OnComfirmCreateNewUserData(int index, string userName)
	{
		LoadUserData(userName, 0uL, 0);
		varifyBtn.gameObject.SetActive(value: true);
		deleteBtn.gameObject.SetActive(value: true);
		SingletonDontDestroy<Game>.Instance.AppData.AddNewUserData(index, userName);
	}

	private void SetDataNormal()
	{
		bgImg.sprite = _userDataUi.SingleUserDataNormalSprite;
	}

	private void SetDataNoUse()
	{
		bgImg.sprite = _userDataUi.SingleUserDataNoUseSprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		addImg.sprite = addHighlightSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		addImg.sprite = addNormalSprite;
	}
}
