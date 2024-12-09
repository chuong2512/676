using System;
using System.Collections.Generic;

public static class EventManager
{
	public delegate void EventHandler(EventData e);

	private static Dictionary<EventEnum, EventHandler> allEventMap = new Dictionary<EventEnum, EventHandler>();

	private static Dictionary<EventEnum, EventHandler> allPermanentEventMap = new Dictionary<EventEnum, EventHandler>();

	private static Dictionary<object, Dictionary<EventEnum, EventHandler>> allObjectEventMap = new Dictionary<object, Dictionary<EventEnum, EventHandler>>();

	public static void RegisterEvent(EventEnum eventEnum, EventHandler handler)
	{
		lock (allEventMap)
		{
			if (allEventMap.TryGetValue(eventEnum, out var value))
			{
				value = (EventHandler)Delegate.Combine(value, handler);
				allEventMap[eventEnum] = value;
			}
			else
			{
				allEventMap[eventEnum] = handler;
			}
		}
	}

	public static void UnregisterEvent(EventEnum eventEnum, EventHandler handler)
	{
		lock (allEventMap)
		{
			if (allEventMap.TryGetValue(eventEnum, out var value))
			{
				value = (EventHandler)Delegate.Remove(value, handler);
				if (value.IsNull())
				{
					allEventMap.Remove(eventEnum);
				}
				else
				{
					allEventMap[eventEnum] = value;
				}
			}
		}
	}

	public static void BroadcastEvent(EventEnum eventEnum, EventData e)
	{
		lock (allEventMap)
		{
			if (allEventMap.TryGetValue(eventEnum, out var value))
			{
				value?.Invoke(e);
			}
		}
		lock (allPermanentEventMap)
		{
			if (allPermanentEventMap.TryGetValue(eventEnum, out var value2))
			{
				value2?.Invoke(e);
			}
		}
	}

	public static void RegisterPermanentEvent(EventEnum eventEnum, EventHandler handler)
	{
		lock (allPermanentEventMap)
		{
			if (allPermanentEventMap.TryGetValue(eventEnum, out var value))
			{
				value = (EventHandler)Delegate.Combine(value, handler);
				allPermanentEventMap[eventEnum] = value;
			}
			else
			{
				allPermanentEventMap[eventEnum] = handler;
			}
		}
	}

	public static void UnregisterPermanentEvent(EventEnum eventEnum, EventHandler handler)
	{
		lock (allPermanentEventMap)
		{
			if (allPermanentEventMap.TryGetValue(eventEnum, out var value))
			{
				value = (EventHandler)Delegate.Remove(value, handler);
				if (value.IsNull())
				{
					allPermanentEventMap.Remove(eventEnum);
				}
				else
				{
					allPermanentEventMap[eventEnum] = value;
				}
			}
		}
	}

	public static void RegisterObjRelatedEvent(object obj, EventEnum eventEnum, EventHandler handler)
	{
		lock (allObjectEventMap)
		{
			if (allObjectEventMap.TryGetValue(obj, out var value))
			{
				if (value.TryGetValue(eventEnum, out var value2))
				{
					value2 = (value[eventEnum] = (EventHandler)Delegate.Combine(value2, handler));
				}
				else
				{
					value[eventEnum] = handler;
				}
			}
			else
			{
				value = new Dictionary<EventEnum, EventHandler>();
				value[eventEnum] = handler;
				allObjectEventMap[obj] = value;
			}
		}
	}

	public static void UnregisterObjRelatedEvent(object obj, EventEnum eventEnum, EventHandler handler)
	{
		lock (allObjectEventMap)
		{
			if (obj == null || !allObjectEventMap.TryGetValue(obj, out var value) || !value.TryGetValue(eventEnum, out var value2))
			{
				return;
			}
			value2 = (EventHandler)Delegate.Remove(value2, handler);
			if (value2.IsNull())
			{
				value.Remove(eventEnum);
				if (value.Count == 0)
				{
					allObjectEventMap.Remove(obj);
				}
			}
			else
			{
				value[eventEnum] = value2;
			}
		}
	}

	public static void BroadcastEvent(object obj, EventEnum eventEnum, EventData e)
	{
		lock (allObjectEventMap)
		{
			if (allObjectEventMap.TryGetValue(obj, out var value) && value.TryGetValue(eventEnum, out var value2))
			{
				value2?.Invoke(e);
			}
		}
	}

	public static void ClearAllEvent()
	{
		lock (allObjectEventMap)
		{
			allObjectEventMap.Clear();
		}
		lock (allEventMap)
		{
			allEventMap.Clear();
		}
	}
}
