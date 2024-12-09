using System;
using System.Collections.Generic;

[Serializable]
public class BaseCardAttr
{
	public string CardCode;

	public string NameKey;

	public string DesKeyOnBattle;

	public string DesKeyNormal;

	public List<KeyValuePair> AllKeys;
}
