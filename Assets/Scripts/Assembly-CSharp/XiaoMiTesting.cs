using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class XiaoMiTesting : MonoBehaviour
{
	private void Start()
	{
		RoomInfo roomInfo = new RoomInfo();
		roomInfo.room = new int[7, 5]
		{
			{ 0, 0, 0, 1, 0 },
			{ 2, 0, 1, 0, 0 },
			{ 0, 0, 1, 0, 0 },
			{ 0, 0, 1, 0, 1 },
			{ 0, 1, 0, 1, 0 },
			{ 0, 0, 1, 1, 3 },
			{ 0, 0, 0, 0, 0 }
		};
		UnityEngine.Debug.Log(FinalCheckRoomValid(roomInfo));
	}

	public static bool FinalCheckRoomValid(RoomInfo info)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		bool flag = false;
		bool flag2 = true;
		Vector2Int startPos = Vector2Int.zero;
		Vector2Int endPos = Vector2Int.zero;
		for (int i = 0; i < info.room.GetLength(0); i++)
		{
			for (int j = 0; j < info.room.GetLength(1); j++)
			{
				if (info.room[i, j] == 2)
				{
					flag = true;
					startPos = new Vector2Int(i, j);
				}
				else if (info.room[i, j] == 3)
				{
					flag2 = true;
					endPos = new Vector2Int(i, j);
				}
			}
		}
		if (!flag || !flag2)
		{
			return false;
		}
		bool result = CheckRoomConnectable(info, startPos, endPos);
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time cost : " + stopwatch.ElapsedMilliseconds);
		return result;
	}

	private static bool CheckRoomConnectable(RoomInfo info, Vector2Int startPos, Vector2Int endPos)
	{
		List<Vector2Int> list = new List<Vector2Int> { startPos };
		List<Vector2Int> list2 = new List<Vector2Int>();
		while (list.Count > 0)
		{
			Vector2Int vector2Int = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			list2.Add(vector2Int);
			if (vector2Int.x + 1 < 7)
			{
				Vector2Int vector2Int2 = vector2Int + Vector2Int.right;
				if (endPos == vector2Int2)
				{
					return true;
				}
				if (!list2.Contains(vector2Int2) && info.room[vector2Int2.x, vector2Int2.y] != 1)
				{
					list.Add(vector2Int2);
				}
			}
			if (vector2Int.x - 1 >= 0)
			{
				Vector2Int vector2Int3 = vector2Int + Vector2Int.left;
				if (endPos == vector2Int3)
				{
					return true;
				}
				if (!list2.Contains(vector2Int3) && info.room[vector2Int3.x, vector2Int3.y] != 1)
				{
					list.Add(vector2Int3);
				}
			}
			if (vector2Int.y + 1 < 5)
			{
				Vector2Int vector2Int4 = vector2Int + Vector2Int.up;
				if (endPos == vector2Int4)
				{
					return true;
				}
				if (!list2.Contains(vector2Int4) && info.room[vector2Int4.x, vector2Int4.y] != 1)
				{
					list.Add(vector2Int4);
				}
			}
			if (vector2Int.y - 1 >= 0)
			{
				Vector2Int vector2Int5 = vector2Int + Vector2Int.down;
				if (endPos == vector2Int5)
				{
					return true;
				}
				if (!list2.Contains(vector2Int5) && info.room[vector2Int5.x, vector2Int5.y] != 1)
				{
					list.Add(vector2Int5);
				}
			}
		}
		return false;
	}
}
