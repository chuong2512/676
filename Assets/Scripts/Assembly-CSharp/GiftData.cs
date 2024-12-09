using System;
using System.Collections.Generic;

[Serializable]
public class GiftData
{
	public BaseGift.GiftType GiftType;

	public BaseGift.GiftName GiftName;

	public string NameKey;

	public string DesKey;

	public List<KeyValuePair> AllKeys;

	public bool IsCanBeRandom;

	public string GiftIcon;

	public string EffectConfigName;
}
