using System;
using System.Collections.Generic;

[Serializable]
public class GameEventData
{
	public string EventCode;

	public BaseGameEvent.RepeatType RepeatType;

	public string TitleKey;

	public string ContentKey;

	public List<OptionData> OptionDatas;

	public string IllustrationName;

	public string GameEventPrefab;

	public List<string> MapLimit;
}
