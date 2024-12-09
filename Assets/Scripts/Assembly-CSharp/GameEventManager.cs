using System.Collections.Generic;
using UnityEngine;

public class GameEventManager
{
	private static GameEventManager _instance;

	private Dictionary<string, BaseGameEvent> allEvents = new Dictionary<string, BaseGameEvent>();

	private Dictionary<Vector2Int, BaseGameEvent> allEventsOnMap = new Dictionary<Vector2Int, BaseGameEvent>();

	public static GameEventManager Instace => _instance ?? (_instance = new GameEventManager());

	private GameEventManager()
	{
	}

	private void LoadAllEvent()
	{
		EventManager.RegisterEvent(EventEnum.E_SwitchToNextLayer, OnNextLayer);
		EventManager.RegisterEvent(EventEnum.E_SwitchToNextLevel, OnNextLevel);
		allEvents.Clear();
		allEventsOnMap.Clear();
		foreach (BaseGameEvent item in FactoryManager.GetAllGameEventInstance())
		{
			allEvents.Add(item.GameEventCode, item);
		}
	}

	public void ChangeEventOnMap(Vector2Int pos, BaseGameEvent gameEvent)
	{
		allEventsOnMap[pos] = gameEvent;
	}

	public void InitGameEventManager()
	{
		LoadAllEvent();
	}

	public void InitGameEventManager(GameSaveProcessInfo info)
	{
		LoadAllEvent();
		List<GameEventSaveData> allGameEventsOnMap = info.allGameEventsOnMap;
		if (!allGameEventsOnMap.IsNull())
		{
			allEventsOnMap = new Dictionary<Vector2Int, BaseGameEvent>();
			for (int i = 0; i < allGameEventsOnMap.Count; i++)
			{
				BaseGameEvent baseGameEvent = allEvents[allGameEventsOnMap[i].eventCode];
				baseGameEvent.InitEventByData(allGameEventsOnMap[i]);
				allEventsOnMap.Add(new Vector2Int(allGameEventsOnMap[i].x, allGameEventsOnMap[i].y), baseGameEvent);
			}
		}
	}

	public List<BaseGameEvent> GetAllPossibleEvents()
	{
		List<BaseGameEvent> list = new List<BaseGameEvent>();
		foreach (KeyValuePair<string, BaseGameEvent> allEvent in allEvents)
		{
			if (DataManager.Instance.GetGameEventData(allEvent.Key).RepeatType != BaseGameEvent.RepeatType.FollowUp)
			{
				list.Add(allEvent.Value);
			}
		}
		return list;
	}

	public List<BaseGameEvent> GetAllSpecificMapEvents(string mapStr)
	{
		List<BaseGameEvent> list = new List<BaseGameEvent>();
		foreach (KeyValuePair<string, BaseGameEvent> allEvent in allEvents)
		{
			GameEventData gameEventData = DataManager.Instance.GetGameEventData(allEvent.Key);
			if (gameEventData.RepeatType != BaseGameEvent.RepeatType.FollowUp && gameEventData.MapLimit != null && gameEventData.MapLimit.Count == 1 && gameEventData.MapLimit[0] == mapStr)
			{
				list.Add(allEvent.Value);
			}
		}
		return list;
	}

	public BaseGameEvent GetEvent(string eventCode)
	{
		if (!allEvents.TryGetValue(eventCode, out var value))
		{
			return new GameEvent_0();
		}
		return value;
	}

	public BaseGameEvent GetEventOnMap(Vector2Int pos)
	{
		if (!allEventsOnMap.TryGetValue(pos, out var value))
		{
			return null;
		}
		return value;
	}

	public void TriggeredEvent(Vector2Int pos, BaseGameEvent gameEvent)
	{
		if (DataManager.Instance.GetGameEventData(gameEvent.GameEventCode).RepeatType != BaseGameEvent.RepeatType.FollowUp)
		{
			allEventsOnMap[pos] = gameEvent;
		}
	}

	public void OnEventOver(Vector2Int pos, BaseGameEvent gameEvent)
	{
		if (DataManager.Instance.GetGameEventData(gameEvent.GameEventCode).RepeatType != BaseGameEvent.RepeatType.FollowUp)
		{
			allEventsOnMap.Remove(pos);
		}
	}

	private void OnNextLayer(EventData data)
	{
		ResetGameEvent();
	}

	private void OnNextLevel(EventData data)
	{
		ResetGameEvent();
	}

	public void ResetGameEvent()
	{
		if (allEventsOnMap.Count <= 0)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, BaseGameEvent> item in allEventsOnMap)
		{
			item.Value.ClearEvent();
		}
		allEventsOnMap.Clear();
	}

	public List<GameEventSaveData> GetGameEventDatas()
	{
		List<GameEventSaveData> list = new List<GameEventSaveData>();
		foreach (KeyValuePair<Vector2Int, BaseGameEvent> item in allEventsOnMap)
		{
			list.Add(item.Value.GetGameEventData());
		}
		return list;
	}
}
