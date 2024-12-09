using System;
using System.Collections.Generic;

[Serializable]
public class EnemyHeapData
{
	public string EnemyHeapCode;

	public List<string> MonsterInfo;

	public int DropType;

	public float DropRate;

	public float CardRate;

	public int MinDropCoin;

	public int MaxDropCoin;

	public int EnemyLevelLimit;

	public int EnemyLayerLimit;

	public int EnemyTypeLimit;

	public int HeapExp;

	public string HeapBgHandlerPrefabName;

	public string HeapBattleUISpriteName;

	public string HeapBGMName;
}
