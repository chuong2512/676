using System;
using UnityEngine;

[Serializable]
public class GameSaveInfo
{
	public int UserDataIndex;

	public int timeUsed;

	public PlayerSaveInfo PlayerSaveInfo;

	public GameSaveProcessInfo GameSaveProcessInfo;

	public GameSaveInfo(int UserDataIndex)
	{
		this.UserDataIndex = UserDataIndex;
		timeUsed = Mathf.FloorToInt(Singleton<GameManager>.Instance.timeUsed);
		PlayerSaveInfo = Singleton<GameManager>.Instance.Player.GetPlayerSaveInfo();
		GameSaveProcessInfo = new GameSaveProcessInfo();
	}
}
