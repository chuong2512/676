using System;
using System.Collections.Generic;

[Serializable]
public class AppData
{
	private const string CannotEmptyKey = "CannotEmptyKey";

	private const string ContainSameUserNameKey = "ContainSameUserNameKey";

	public string GameVersion;

	public Dictionary<int, string> AllUserDataInfo;

	public bool IsEverShowSpaceTimeUI;

	public int CurrentUserDataIndex { get; private set; }

	public int UserDataCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, string> item in AllUserDataInfo)
			{
				if (!item.Value.IsNullOrEmpty())
				{
					num++;
				}
			}
			return num;
		}
	}

	public AppData(string gameVersion)
	{
		GameVersion = gameVersion;
		CurrentUserDataIndex = 0;
		AllUserDataInfo = new Dictionary<int, string>(3);
		for (int i = 0; i < 3; i++)
		{
			AllUserDataInfo.Add(i, string.Empty);
		}
		IsEverShowSpaceTimeUI = false;
	}

	public UserData AddNewUserData(int index, string userName)
	{
		UserData userData = new UserData(SingletonDontDestroy<Game>.Instance.GameVersion, index, userName);
		GameSave.SaveUserData(index, userData);
		AllUserDataInfo[index] = userName;
		GameSave.SaveAppData();
		return userData;
	}

	public int DeleteUserData(int index)
	{
		AllUserDataInfo[index] = string.Empty;
		int num = index;
		if (index != CurrentUserDataIndex)
		{
			GameSave.SaveAppData();
		}
		else
		{
			GameSave.DeleteOldSaveData();
			num = GetNotEmptyUserDataIndex();
			SingletonDontDestroy<Game>.Instance.SetNewUserData(num);
		}
		GameSave.DeleteUserDataByIndex(index);
		return num;
	}

	public void SetNewCurrentUserDataIndex(int index)
	{
		CurrentUserDataIndex = index;
		GameSave.SaveAppData();
	}

	private int GetNotEmptyUserDataIndex()
	{
		for (int i = 0; i < 3; i++)
		{
			if (!AllUserDataInfo[i].IsNullOrEmpty())
			{
				return i;
			}
		}
		return -1;
	}

	public bool ContainUserName(string userName)
	{
		return AllUserDataInfo.ContainsValue(userName);
	}

	public void VarifyNewName(int index, string newName)
	{
		AllUserDataInfo[index] = newName;
		UserData userData = GameSave.LoadUserData(index);
		userData.SetUserName(newName);
		GameSave.SaveUserData(index, userData);
	}

	public static string CheckNewName(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			return "CannotEmptyKey".LocalizeText();
		}
		if (SingletonDontDestroy<Game>.Instance.AppData.ContainUserName(newName))
		{
			return "ContainSameUserNameKey".LocalizeText();
		}
		return string.Empty;
	}
}
