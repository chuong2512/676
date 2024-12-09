using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager
{
	private static RoomManager _instance;

	private Dictionary<string, RoomInfo> allRoomInfo = new Dictionary<string, RoomInfo>();

	public static RoomManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new RoomManager();
			}
			return _instance;
		}
	}

	public Dictionary<string, RoomInfo> AllRoomInfo => allRoomInfo;

	private RoomManager()
	{
	}

	public void GenerateRoom(PlayerOccupation playerOccupation)
	{
		if (SingletonDontDestroy<Game>.Instance.isTest && !SingletonDontDestroy<Game>.Instance.isRandomSeed)
		{
			UnityEngine.Random.InitState(SingletonDontDestroy<Game>.Instance.RandomSeed);
		}
		List<BaseGameEvent> allPossibleEvents = GameEventManager.Instace.GetAllPossibleEvents();
		HashSet<BaseGameEvent> allCannotRepeatEvents = new HashSet<BaseGameEvent>();
		GenerateNormalRoom(1, 1, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateNormalRoom(1, 2, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateBossRoom(1, 3, allCannotRepeatEvents);
		GenerateNormalRoom(2, 1, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateNormalRoom(2, 2, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateBossRoom(2, 3, allCannotRepeatEvents);
		GenerateNormalRoom(3, 1, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateNormalRoom(3, 2, allPossibleEvents, allCannotRepeatEvents, ResetLayerRepeatEvent);
		GenerateBossRoom(3, 3, allCannotRepeatEvents);
		ProcessNormalRoomPlot(playerOccupation);
	}

	private void ProcessNormalRoomPlot(PlayerOccupation playerOccupation)
	{
		Dictionary<string, PlotData> allPlotDatas = DataManager.Instance.AllPlotDatas;
		if (allPlotDatas.Count <= SingletonDontDestroy<Game>.Instance.CurrentUserData.UnlockedPlotAmount)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, 4);
		if (num <= 0)
		{
			return;
		}
		List<PlotData> list = new List<PlotData>();
		foreach (KeyValuePair<string, PlotData> item in allPlotDatas)
		{
			if (!SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked(item.Key))
			{
				list.Add(item.Value);
			}
		}
		string[] array = allRoomInfo.Keys.ToList().RandomFromList(num);
		for (int i = 0; i < array.Length; i++)
		{
			ProcessSingleNormalRoomPlot(array[i], playerOccupation, list);
		}
	}

	private void ProcessSingleNormalRoomPlot(string mapStr, PlayerOccupation playerOccupation, List<PlotData> allPossiblePlots)
	{
		List<PlotData> list = new List<PlotData>();
		for (int i = 0; i < allPossiblePlots.Count; i++)
		{
			PlotData plotData = allPossiblePlots[i];
			bool num = plotData.PlayerOccupation == PlayerOccupation.None || plotData.PlayerOccupation == playerOccupation;
			bool flag = plotData.LimitPlot.IsNullOrEmpty() || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked(plotData.LimitPlot);
			bool flag2 = plotData.StageLimit == null || plotData.StageLimit.Count == 0 || plotData.StageLimit.Contains(mapStr);
			if (num && flag && flag2)
			{
				list.Add(plotData);
			}
		}
		if (list.Count > 0)
		{
			PlotData plotData2 = list[UnityEngine.Random.Range(0, list.Count)];
			allPossiblePlots.Remove(plotData2);
			Vector2Int randomEmptyBlockPos = allRoomInfo[mapStr].GetRandomEmptyBlockPos();
			allRoomInfo[mapStr].SetPlotInfo(new Vec2Seri(randomEmptyBlockPos.x, randomEmptyBlockPos.y), plotData2.PlotCode);
			allRoomInfo[mapStr].room[randomEmptyBlockPos.x, randomEmptyBlockPos.y] = 10;
		}
	}

	public void GenerateHiddenRoom(PlayerOccupation playerOccupation)
	{
		allRoomInfo["4_1"] = RoomGenerator.GenerateHiddenRoom(out var eventPosList, out var normalMonsterPosList);
		ProcessRoomEvent(allRoomInfo["4_1"], "4_1", eventPosList, GameEventManager.Instace.GetAllSpecificMapEvents("4_1").RandomListSort(), new HashSet<BaseGameEvent>());
		ProcessRoomMonster(allRoomInfo["4_1"], 4, 1, normalMonsterPosList);
		ProcessHidenRoomPlot("4_1", playerOccupation);
		GenerateHiddenBossRoom();
	}

	private void GenerateHiddenBossRoom()
	{
		allRoomInfo["4_2"] = RoomGenerator.GenerateHiddenBossRoom();
	}

	private void ProcessHidenRoomPlot(string mapStr, PlayerOccupation playerOccupation)
	{
		Dictionary<string, PlotData> allPlotDatas = DataManager.Instance.AllPlotDatas;
		if (allPlotDatas.Count <= SingletonDontDestroy<Game>.Instance.CurrentUserData.UnlockedPlotAmount || !(UnityEngine.Random.value > 0.5f))
		{
			return;
		}
		List<PlotData> list = new List<PlotData>();
		foreach (KeyValuePair<string, PlotData> item in allPlotDatas)
		{
			PlotData value = item.Value;
			bool num = SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked(item.Key);
			bool flag = value.PlayerOccupation == PlayerOccupation.None || value.PlayerOccupation == playerOccupation;
			bool flag2 = value.LimitPlot.IsNullOrEmpty() || SingletonDontDestroy<Game>.Instance.CurrentUserData.IsPlotUnlocked(value.LimitPlot);
			bool flag3 = value.StageLimit == null || value.StageLimit.Count == 0 || value.StageLimit.Contains(mapStr);
			if (!num && flag && flag2 && flag3)
			{
				list.Add(item.Value);
			}
		}
		if (list.Count > 0)
		{
			PlotData plotData = list[UnityEngine.Random.Range(0, list.Count)];
			Vector2Int randomEmptyBlockPos = allRoomInfo[mapStr].GetRandomEmptyBlockPos();
			allRoomInfo[mapStr].SetPlotInfo(new Vec2Seri(randomEmptyBlockPos.x, randomEmptyBlockPos.y), plotData.PlotCode);
			allRoomInfo[mapStr].room[randomEmptyBlockPos.x, randomEmptyBlockPos.y] = 10;
		}
	}

	private void GenerateBossRoom(int mapLevel, int mapLayer, HashSet<BaseGameEvent> allCannotRepeatEvents)
	{
		string key = mapLevel + "_" + mapLayer;
		allRoomInfo[key] = RoomGenerator.GenerateBossRoom();
		ProcessRoomBossMonster(allRoomInfo[key], mapLevel, mapLayer);
		ResetLevelRepeatEvent(allCannotRepeatEvents);
	}

	private void GenerateNormalRoom(int mapLevel, int mapLayer, List<BaseGameEvent> allPossibleEvents, HashSet<BaseGameEvent> allCannotRepeatEvents, Action<HashSet<BaseGameEvent>> repeatResetHandler)
	{
		string text = mapLevel + "_" + mapLayer;
		allRoomInfo[text] = RoomGenerator.GenerateNormalRoom(out var eventPosList, out var normalMonsterPosList);
		ProcessRoomEvent(allRoomInfo[text], text, eventPosList, allPossibleEvents, allCannotRepeatEvents);
		repeatResetHandler(allCannotRepeatEvents);
		ProcessRoomMonster(allRoomInfo[text], mapLevel, mapLayer, normalMonsterPosList);
	}

	private void ProcessRoomBossMonster(RoomInfo roomInfo, int mapLevel, int mapLayer)
	{
		List<EnemyHeapData> list = new List<EnemyHeapData>();
		foreach (EnemyHeapData value in DataManager.Instance.AllEnemyHeapDatas.Values)
		{
			if (value.EnemyLevelLimit == mapLevel && value.EnemyLayerLimit == mapLayer && value.EnemyTypeLimit == 3)
			{
				list.Add(value);
			}
		}
		roomInfo.specialMonsterHeapCode = list[UnityEngine.Random.Range(0, list.Count)].EnemyHeapCode;
	}

	private void ProcessRoomMonster(RoomInfo roomInfo, int mapLevel, int mapLayer, List<Vector2Int> allNormalMonsterPosList)
	{
		Dictionary<Vec2Seri, string> dictionary = new Dictionary<Vec2Seri, string>(allNormalMonsterPosList.Count);
		List<EnemyHeapData> list = new List<EnemyHeapData>();
		List<EnemyHeapData> list2 = new List<EnemyHeapData>();
		foreach (EnemyHeapData value in DataManager.Instance.AllEnemyHeapDatas.Values)
		{
			if (value.EnemyLevelLimit == mapLevel && value.EnemyLayerLimit == mapLayer)
			{
				if (value.EnemyTypeLimit == 1)
				{
					list.Add(value);
				}
				else if (value.EnemyTypeLimit == 2)
				{
					list2.Add(value);
				}
			}
		}
		EnemyHeapData[] array = list.RandomFromList(allNormalMonsterPosList.Count);
		for (int i = 0; i < array.Length; i++)
		{
			dictionary.Add(new Vec2Seri(allNormalMonsterPosList[i].x, allNormalMonsterPosList[i].y), array[i].EnemyHeapCode);
		}
		roomInfo.monsterHeapInfos = dictionary;
		roomInfo.specialMonsterHeapCode = list2[UnityEngine.Random.Range(0, list2.Count)].EnemyHeapCode;
	}

	private void ProcessRoomEvent(RoomInfo roomInfo, string mapStr, List<Vector2Int> allEventPosList, List<BaseGameEvent> allPossibleEvents, HashSet<BaseGameEvent> allCannotRepeatEvents)
	{
		List<BaseGameEvent> list = new List<BaseGameEvent>();
		for (int i = 0; i < allPossibleEvents.Count; i++)
		{
			if (allPossibleEvents[i].IsCanTrigger(mapStr) && !allCannotRepeatEvents.Contains(allPossibleEvents[i]))
			{
				list.Add(allPossibleEvents[i]);
			}
		}
		Dictionary<Vec2Seri, string> dictionary = new Dictionary<Vec2Seri, string>(allEventPosList.Count);
		BaseGameEvent[] array = list.RandomFromList(allEventPosList.Count);
		for (int j = 0; j < array.Length; j++)
		{
			dictionary.Add(new Vec2Seri(allEventPosList[j].x, allEventPosList[j].y), array[j].GameEventCode);
			allCannotRepeatEvents.Add(array[j]);
		}
		roomInfo.eventInfos = dictionary;
	}

	private void ResetLayerRepeatEvent(HashSet<BaseGameEvent> allCannotRepeatEvents)
	{
		List<BaseGameEvent> list = new List<BaseGameEvent>();
		foreach (BaseGameEvent allCannotRepeatEvent in allCannotRepeatEvents)
		{
			if (DataManager.Instance.GetGameEventData(allCannotRepeatEvent.GameEventCode).RepeatType == BaseGameEvent.RepeatType.LayerRepeat)
			{
				list.Add(allCannotRepeatEvent);
			}
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				allCannotRepeatEvents.Remove(list[i]);
			}
		}
	}

	private void ResetLevelRepeatEvent(HashSet<BaseGameEvent> allCannotRepeatEvents)
	{
		List<BaseGameEvent> list = new List<BaseGameEvent>();
		foreach (BaseGameEvent allCannotRepeatEvent in allCannotRepeatEvents)
		{
			GameEventData gameEventData = DataManager.Instance.GetGameEventData(allCannotRepeatEvent.GameEventCode);
			if (gameEventData.RepeatType == BaseGameEvent.RepeatType.LayerRepeat || gameEventData.RepeatType == BaseGameEvent.RepeatType.LevelRepeat)
			{
				list.Add(allCannotRepeatEvent);
			}
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				allCannotRepeatEvents.Remove(list[i]);
			}
		}
	}

	public void LoadRoom(GameSaveProcessInfo info)
	{
		allRoomInfo = info.allLockedRoomInfo;
	}

	public RoomInfo GetRoomInfo(int level, int layer, bool isAutoDelete = true)
	{
		string text = level + "_" + layer;
		RoomInfo value = null;
		if (!allRoomInfo.TryGetValue(text, out value))
		{
			Debug.LogError("Can not get room info from RoomManager : " + text);
		}
		if (isAutoDelete)
		{
			allRoomInfo.Remove(text);
		}
		return value;
	}
}
