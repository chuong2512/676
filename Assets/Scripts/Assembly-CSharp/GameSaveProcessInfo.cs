using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveProcessInfo
{
	public int currentMapLayer;

	public int currentMapLevel;

	public Dictionary<string, RoomInfo> allLockedRoomInfo;

	public RoomInfo currentRoomInfo;

	public Dictionary<string, List<Vec2Seri>> allBlockLoadHandleInfos;

	public List<GameEventSaveData> allGameEventsOnMap;

	public ShopData shopData;

	public List<string> allClearBossHeapIDList;

	public GameSaveProcessInfo()
	{
		currentMapLayer = Singleton<GameManager>.Instance.CurrentMapLayer;
		currentMapLevel = Singleton<GameManager>.Instance.CurrentMapLevel;
		allLockedRoomInfo = RoomManager.Instance.AllRoomInfo;
		RoomUI roomUI = SingletonDontDestroy<UIManager>.Instance.ForceGetView("RoomUI") as RoomUI;
		currentRoomInfo = roomUI.CurrentRoomInfo;
		allGameEventsOnMap = GameEventManager.Instace.GetGameEventDatas();
		allBlockLoadHandleInfos = roomUI.GetAllBlockLoadHandleInfos();
		allClearBossHeapIDList = Singleton<GameManager>.Instance.AllClearBossHeapIdList;
		shopData = GameTempData.ShopData;
	}
}
