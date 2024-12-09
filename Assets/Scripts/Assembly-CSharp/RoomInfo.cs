using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomInfo
{
	public int[,] room;

	public int[,] seeds;

	public Dictionary<Vec2Seri, string> eventInfos;

	public Dictionary<Vec2Seri, string> monsterHeapInfos;

	public string plotCode;

	public Vec2Seri plotPos;

	public string specialMonsterHeapCode;

	public Vector2Int GetRandomBlockPos()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < room.GetLength(0); i++)
		{
			for (int j = 0; j < room.GetLength(1); j++)
			{
				if (room[i, j] == 1)
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public Vector2Int[] GetRandomBlockPos(int amount)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < room.GetLength(0); i++)
		{
			for (int j = 0; j < room.GetLength(1); j++)
			{
				if (room[i, j] == 1)
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		return list.RandomFromList(amount);
	}

	public Vector2Int GetRandomEmptyBlockPos()
	{
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < room.GetLength(0); i++)
		{
			for (int j = 0; j < room.GetLength(1); j++)
			{
				if (room[i, j] == 0)
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public void SetPlotInfo(Vec2Seri pos, string plotCode)
	{
		plotPos = pos;
		this.plotCode = plotCode;
	}

	public void SetEventCode(Vector2Int pos, string evenCode)
	{
		if (eventInfos == null)
		{
			eventInfos = new Dictionary<Vec2Seri, string>();
		}
		room[pos.x, pos.y] = 6;
		eventInfos[new Vec2Seri(pos.x, pos.y)] = evenCode;
	}
}
