using System;
using System.Collections.Generic;
using UnityEngine;

public static class RoomGenerator
{
	private class LineFunc
	{
		private float k;

		private float b;

		public LineFunc(float x1, float y1, float x2, float y2)
		{
			k = (y2 - y1) / (x2 - x1);
			b = y1 - k * x1;
		}

		public float GetY(float x)
		{
			return k * x + b;
		}

		public float GetX(float y)
		{
			return (y - b) / k;
		}

		public float GetK()
		{
			return k;
		}
	}

	public const int RoomWidth = 7;

	public const int RoomHeight = 5;

	public static readonly List<int> EntryExitIndex = new List<int>
	{
		0, 1, 2, 4, 5, 6, 7, 8, 12, 13,
		14, 20, 21, 22, 26, 27, 28, 29, 30, 32,
		33, 34
	};

	public static RoomInfo GenerateHiddenRoom(out List<Vector2Int> eventPosList, out List<Vector2Int> normalMonsterPosList)
	{
		int[,] array = new int[7, 5];
		List<int> list = new List<int>();
		array[0, 2] = 2;
		list.Add(14);
		array[6, 2] = 3;
		list.Add(20);
		eventPosList = new List<Vector2Int>
		{
			new Vector2Int(0, 0),
			new Vector2Int(3, 0),
			new Vector2Int(6, 0),
			new Vector2Int(0, 4),
			new Vector2Int(3, 4),
			new Vector2Int(6, 4)
		};
		for (int i = 0; i < eventPosList.Count; i++)
		{
			array[eventPosList[i].x, eventPosList[i].y] = 6;
			list.Add(7 * eventPosList[i].y + eventPosList[i].x);
		}
		array[5, 2] = 4;
		list.Add(19);
		int amount = 4;
		int[] array2 = SeparateNormalMonster(list, amount);
		List<Vector2Int> list2 = new List<Vector2Int>(array2.Length);
		for (int j = 0; j < array2.Length; j++)
		{
			int num = array2[j] % 7;
			int num2 = array2[j] / 7;
			array[num, num2] = 5;
			list2.Add(new Vector2Int(num, num2));
		}
		normalMonsterPosList = list2;
		int[] array3 = RandomBlock(list);
		for (int k = 0; k < array3.Length; k++)
		{
			int num3 = array3[k] % 7;
			int num4 = array3[k] / 7;
			array[num3, num4] = 1;
		}
		HandleRoomConnection(array);
		RoomInfo obj = new RoomInfo
		{
			seeds = GetRoomSeedsArray(),
			room = array
		};
		AdjustRoomBlocks(obj);
		return obj;
	}

	public static RoomInfo GenerateHiddenBossRoom()
	{
		RoomInfo roomInfo = new RoomInfo();
		int[,] array = new int[7, 5];
		array[0, 2] = 2;
		array[3, 0] = 11;
		int[,] array2 = (roomInfo.room = array);
		int[,] obj = new int[7, 5]
		{
			{ 1, 1, 1, 1, 1 },
			{ 1, 1, 2, 1, 1 },
			{ 1, 0, 0, 0, 1 },
			{ 0, 0, 0, 0, 1 },
			{ 1, 0, 0, 0, 1 },
			{ 1, 1, 3, 1, 1 },
			{ 1, 1, 1, 1, 1 }
		};
		obj[3, 0] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		roomInfo.seeds = obj;
		return roomInfo;
	}

	public static RoomInfo GenerateNormalRoom(out List<Vector2Int> eventPosList, out List<Vector2Int> normalMonsterPosList)
	{
		try
		{
			RoomInfo roomInfo = GenerateRoom(out eventPosList, out normalMonsterPosList);
			AdjustRoomBlocks(roomInfo);
			return FinalCheckRoomValid(roomInfo) ? roomInfo : GenerateNormalRoom(out eventPosList, out normalMonsterPosList);
		}
		catch (Exception)
		{
			return GenerateNormalRoom(out eventPosList, out normalMonsterPosList);
		}
	}

	public static bool FinalCheckRoomValid(RoomInfo info)
	{
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
		return CheckRoomConnectable(info, startPos, endPos);
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

	public static void AdjustRoomBlocks(RoomInfo roomInfo)
	{
		int length = roomInfo.room.GetLength(0);
		int length2 = roomInfo.room.GetLength(1);
		bool[,] array = new bool[length, length2];
		for (int i = 0; i < length; i++)
		{
			for (int j = 0; j < length2; j++)
			{
				if (array[i, j])
				{
					continue;
				}
				if (IsBlock(roomInfo, i, j))
				{
					if (IsThisBlockBeBlocked(roomInfo, i, j, length, length2))
					{
						Vector2Int vector2Int = MakeBlockConnectable(roomInfo, i, j, length, length2);
						array[vector2Int.x, vector2Int.y] = true;
						MakeRoundCheced(array, vector2Int.x, vector2Int.y, length, length2);
					}
					else
					{
						array[i, j] = true;
					}
				}
				else
				{
					array[i, j] = true;
					MakeRoundCheced(array, i, j, length, length2);
				}
			}
		}
	}

	private static bool IsBlock(RoomInfo roomInfo, int x, int y)
	{
		return roomInfo.room[x, y] == 1;
	}

	private static void MakeRoundCheced(bool[,] checkArray, int x, int y, int width, int height)
	{
		if (x - 1 >= 0)
		{
			checkArray[x - 1, y] = true;
		}
		if (x + 1 < width)
		{
			checkArray[x + 1, y] = true;
		}
		if (y - 1 >= 0)
		{
			checkArray[x, y - 1] = true;
		}
		if (y + 1 < height)
		{
			checkArray[x, y + 1] = true;
		}
	}

	private static bool IsThisBlockBeBlocked(RoomInfo roomInfo, int x, int y, int width, int height)
	{
		if (x - 1 >= 0 && roomInfo.room[x - 1, y] != 1)
		{
			return false;
		}
		if (x + 1 < width && roomInfo.room[x + 1, y] != 1)
		{
			return false;
		}
		if (y - 1 >= 0 && roomInfo.room[x, y - 1] != 1)
		{
			return false;
		}
		if (y + 1 < height && roomInfo.room[x, y + 1] != 1)
		{
			return false;
		}
		return true;
	}

	private static Vector2Int MakeBlockConnectable(RoomInfo roomInfo, int x, int y, int width, int height)
	{
		if (x - 2 >= 0 && roomInfo.room[x - 2, y] != 1)
		{
			roomInfo.room[x - 1, y] = 0;
			return new Vector2Int(x - 1, y);
		}
		if (x + 2 < width && roomInfo.room[x + 2, y] != 1)
		{
			roomInfo.room[x + 1, y] = 0;
			return new Vector2Int(x + 1, y);
		}
		if (y - 2 >= 0 && roomInfo.room[x, y - 2] != 1)
		{
			roomInfo.room[x, y - 1] = 0;
			return new Vector2Int(x, y - 1);
		}
		if (y + 2 < height && roomInfo.room[x, y + 2] != 1)
		{
			roomInfo.room[x, y + 1] = 0;
			return new Vector2Int(x, y + 1);
		}
		throw new Exception("Cannot make block connectable");
	}

	private static RoomInfo GenerateRoom(out List<Vector2Int> eventPosList, out List<Vector2Int> normalMonsterPosList)
	{
		int[,] array = new int[7, 5];
		List<int> list = new List<int>();
		int num = EntryExitIndex[UnityEngine.Random.Range(0, EntryExitIndex.Count)];
		array[num % 7, num / 7] = 2;
		list.Add(num);
		int exitIndex = GetExitIndex(num);
		array[exitIndex % 7, exitIndex / 7] = 3;
		list.Add(exitIndex);
		int keepDoorMonster = GetKeepDoorMonster(exitIndex);
		array[keepDoorMonster % 7, keepDoorMonster / 7] = 4;
		list.Add(keepDoorMonster);
		int num2 = UnityEngine.Random.Range(3, 5);
		int[] array2 = SeparateNormalMonster(list, num2);
		List<Vector2Int> list2 = new List<Vector2Int>(array2.Length);
		for (int i = 0; i < array2.Length; i++)
		{
			int num3 = array2[i] % 7;
			int num4 = array2[i] / 7;
			array[num3, num4] = 5;
			list2.Add(new Vector2Int(num3, num4));
		}
		normalMonsterPosList = list2;
		int amount = 7 - num2;
		int[] array3 = SeparateNormalMonster(list, amount);
		List<Vector2Int> list3 = new List<Vector2Int>(array3.Length);
		for (int j = 0; j < array3.Length; j++)
		{
			int num5 = array3[j] % 7;
			int num6 = array3[j] / 7;
			array[num5, num6] = 6;
			list3.Add(new Vector2Int(num5, num6));
		}
		eventPosList = list3;
		int randomNorOccupiedPoint = GetRandomNorOccupiedPoint(list);
		array[randomNorOccupiedPoint % 7, randomNorOccupiedPoint / 7] = 7;
		int[] array4 = RandomBlock(list);
		for (int k = 0; k < array4.Length; k++)
		{
			int num7 = array4[k] % 7;
			int num8 = array4[k] / 7;
			array[num7, num8] = 1;
		}
		HandleRoomConnection(array);
		return new RoomInfo
		{
			seeds = GetRoomSeedsArray(),
			room = array
		};
	}

	private static int[,] GetRoomSeedsArray()
	{
		int[,] array = new int[7, 5];
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				array[i, j] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			}
		}
		return array;
	}

	private static void HandleRoomConnection(int[,] room)
	{
		List<List<int>> list = new List<List<int>>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int item = j * 7 + i;
				if (room[i, j] != 1)
				{
					list2.Add(item);
				}
			}
		}
		while (list2.Count > 0)
		{
			int num = list2[list2.Count - 1];
			List<int> list3 = new List<int> { num };
			list2.RemoveAt(list2.Count - 1);
			CalculateRoundRegion(num, list3, list2);
			list.Add(list3);
		}
		while (list.Count > 1)
		{
			int regionIndex = 0;
			int regionIndex2 = 0;
			int index = 0;
			int num2 = int.MaxValue;
			int index2 = list.Count - 1;
			for (int k = 0; k < list.Count - 1; k++)
			{
				int region1Index = 0;
				int region2Index = 0;
				int num3 = CalculateRegionDistance(list[index2], list[k], out region1Index, out region2Index);
				if (num3 < num2)
				{
					regionIndex = region1Index;
					regionIndex2 = region2Index;
					num2 = num3;
					index = k;
				}
			}
			List<int> list4 = list[index2];
			for (int l = 0; l < list4.Count; l++)
			{
				list[index].Add(list4[l]);
			}
			list.RemoveAt(index2);
			Stack<int> stack = ConnectToRegion(regionIndex, regionIndex2, room);
			while (stack.Count > 0)
			{
				list[index].Add(stack.Pop());
			}
		}
	}

	private static Stack<int> ConnectToRegion(int regionIndex1, int regionIndex2, int[,] room)
	{
		int num = regionIndex1 % 7;
		int num2 = regionIndex1 / 7;
		int num3 = regionIndex2 % 7;
		int num4 = regionIndex2 / 7;
		Stack<int> stack = new Stack<int>();
		if (num == num3)
		{
			int num5 = Mathf.Min(num2, num4);
			int num6 = Mathf.Max(num2, num4);
			for (int i = num5 + 1; i < num6; i++)
			{
				stack.Push(i * 7 + num);
				room[num, i] = 0;
			}
			return stack;
		}
		if (num2 == num4)
		{
			int num7 = Mathf.Min(num, num3);
			int num8 = Mathf.Max(num, num3);
			for (int j = num7 + 1; j < num8; j++)
			{
				stack.Push(num2 * 7 + j);
				room[j, num2] = 0;
			}
			return stack;
		}
		LineFunc lineFunc = new LineFunc(num, num2, num3, num4);
		if (Mathf.Abs(lineFunc.GetK()) < 1f)
		{
			int num9 = Mathf.Min(num, num3);
			int num10 = Mathf.Max(num, num3);
			for (int k = num9 + 1; k < num10; k++)
			{
				float y = lineFunc.GetY(k);
				float num11 = 2f * y;
				int num12 = Mathf.FloorToInt(num11);
				if (num11 == (float)num12)
				{
					stack.Push((int)((y - 0.5f) * 7f + (float)k));
					stack.Push((int)((y + 0.5f) * 7f + (float)k));
					room[k, (int)(y - 0.5f)] = 0;
					room[k, (int)(y + 0.5f)] = 0;
				}
				else
				{
					stack.Push(num12 * 7 + k);
					room[k, num12] = 0;
				}
			}
		}
		else if (Mathf.Abs(lineFunc.GetK()) == 1f)
		{
			if (lineFunc.GetK() == 1f)
			{
				int num13 = Mathf.Min(num, num3);
				int num14 = Mathf.Max(num, num3);
				int num15 = Mathf.Min(num2, num4);
				stack.Push(num15 * 7 + num13 + 1);
				room[num13 + 1, num15] = 0;
				num13++;
				num15++;
				while (num13 < num14)
				{
					stack.Push(num15 * 7 + num13 + 1);
					stack.Push(num15 * 7 + num13);
					room[num13 + 1, num15] = 0;
					room[num13, num15] = 0;
					num13++;
					num15++;
				}
			}
			else
			{
				int num16 = Mathf.Max(num, num3);
				int num17 = Mathf.Min(num, num3);
				int num18 = Mathf.Max(num2, num4);
				stack.Push(num18 * 7 + num17 + 1);
				room[num17 + 1, num18] = 0;
				num17++;
				num18--;
				while (num17 < num16)
				{
					stack.Push(num18 * 7 + num17 + 1);
					stack.Push(num18 * 7 + num17);
					room[num17 + 1, num18] = 0;
					room[num17, num18] = 0;
					num17++;
					num18--;
				}
			}
		}
		else
		{
			int num19 = Mathf.Max(num2, num4);
			for (int l = Mathf.Min(num2, num4) + 1; l < num19; l++)
			{
				float x = lineFunc.GetX(l);
				float num20 = 2f * x;
				int num21 = Mathf.FloorToInt(num20);
				if (num20 == (float)num21)
				{
					stack.Push((int)((float)(l * 7) + num20 - 0.5f));
					stack.Push((int)((float)(l * 7) + num20 + 0.5f));
					room[(int)(num20 - 0.5f), l] = 0;
					room[(int)(num20 + 0.5f), l] = 0;
				}
				else
				{
					stack.Push(l * 7 + num21);
					room[num21, l] = 0;
				}
			}
		}
		return stack;
	}

	private static int CalculateRegionDistance(List<int> region1, List<int> region2, out int region1Index, out int region2Index)
	{
		int num = int.MaxValue;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < region1.Count; i++)
		{
			int num4 = region1[i] % 7;
			int num5 = region1[i] / 7;
			for (int j = 0; j < region2.Count; j++)
			{
				int num6 = region2[j] % 7;
				int num7 = region2[j] / 7;
				int num8 = Mathf.Abs(num4 - num6) + Mathf.Abs(num5 - num7);
				if (num8 == 2)
				{
					region1Index = region1[i];
					region2Index = region2[j];
					return 2;
				}
				if (num8 < num)
				{
					num = num8;
					num2 = region1[i];
					num3 = region2[j];
				}
			}
		}
		region1Index = num2;
		region2Index = num3;
		return num;
	}

	private static void CalculateRoundRegion(int index, List<int> region, List<int> allIndex)
	{
		int num = index % 7;
		int num2 = index / 7;
		if (num + 1 < 7 && allIndex.Contains(index + 1))
		{
			int num3 = index + 1;
			region.Add(num3);
			allIndex.Remove(num3);
			CalculateRoundRegion(num3, region, allIndex);
		}
		if (num - 1 >= 0 && allIndex.Contains(index - 1))
		{
			int num4 = index - 1;
			region.Add(num4);
			allIndex.Remove(num4);
			CalculateRoundRegion(num4, region, allIndex);
		}
		if (num2 - 1 >= 0 && allIndex.Contains(index - 7))
		{
			int num5 = index - 7;
			region.Add(num5);
			allIndex.Remove(num5);
			CalculateRoundRegion(num5, region, allIndex);
		}
		if (num2 + 1 < 5 && allIndex.Contains(index + 7))
		{
			int num6 = index + 7;
			region.Add(num6);
			allIndex.Remove(num6);
			CalculateRoundRegion(num6, region, allIndex);
		}
	}

	private static int[] RandomBlock(List<int> occupiedList)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				int item = j * 7 + i;
				if (!occupiedList.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		int[] array = new int[12];
		for (int k = 0; k < 12; k++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			array[k] = list[index];
			list.RemoveAt(index);
		}
		return array;
	}

	private static int GetRandomNorOccupiedPoint(List<int> occupiedList)
	{
		bool flag = false;
		int num = 0;
		do
		{
			int num2 = UnityEngine.Random.Range(0, 7);
			num = UnityEngine.Random.Range(0, 5) * 7 + num2;
		}
		while (occupiedList.Contains(num));
		occupiedList.Add(num);
		return num;
	}

	private static int[] SeparateNormalMonster(List<int> occupiedList, int amount)
	{
		int[] array = new int[amount];
		switch (amount)
		{
		case 4:
		{
			bool flag4 = false;
			int num8 = 0;
			for (int j = 0; j < 2; j++)
			{
				do
				{
					int num9 = UnityEngine.Random.Range(2, 5);
					num8 = UnityEngine.Random.Range(0, 5) * 7 + num9;
				}
				while (occupiedList.Contains(num8));
				array[j] = num8;
				occupiedList.Add(num8);
			}
			do
			{
				int num10 = UnityEngine.Random.Range(0, 2);
				num8 = UnityEngine.Random.Range(0, 5) * 7 + num10;
			}
			while (occupiedList.Contains(num8));
			array[2] = num8;
			occupiedList.Add(num8);
			do
			{
				int num11 = UnityEngine.Random.Range(5, 7);
				num8 = UnityEngine.Random.Range(0, 5) * 7 + num11;
			}
			while (occupiedList.Contains(num8));
			array[3] = num8;
			occupiedList.Add(num8);
			break;
		}
		case 3:
		{
			if (UnityEngine.Random.Range(1, 3) == 2)
			{
				bool flag = false;
				int num = 0;
				for (int i = 0; i < 2; i++)
				{
					do
					{
						int num2 = UnityEngine.Random.Range(2, 5);
						num = UnityEngine.Random.Range(0, 5) * 7 + num2;
					}
					while (occupiedList.Contains(num));
					array[i] = num;
					occupiedList.Add(num);
				}
				bool flag2 = (double)UnityEngine.Random.value > 0.5;
				do
				{
					int num3 = (flag2 ? UnityEngine.Random.Range(0, 2) : UnityEngine.Random.Range(5, 7));
					num = UnityEngine.Random.Range(0, 5) * 7 + num3;
				}
				while (occupiedList.Contains(num));
				array[2] = num;
				occupiedList.Add(num);
				break;
			}
			bool flag3 = false;
			int num4 = 0;
			do
			{
				int num5 = UnityEngine.Random.Range(0, 2);
				num4 = UnityEngine.Random.Range(0, 5) * 7 + num5;
			}
			while (occupiedList.Contains(num4));
			array[0] = num4;
			occupiedList.Add(num4);
			do
			{
				int num6 = UnityEngine.Random.Range(2, 5);
				num4 = UnityEngine.Random.Range(0, 5) * 7 + num6;
			}
			while (occupiedList.Contains(num4));
			array[1] = num4;
			occupiedList.Add(num4);
			do
			{
				int num7 = UnityEngine.Random.Range(5, 7);
				num4 = UnityEngine.Random.Range(0, 5) * 7 + num7;
			}
			while (occupiedList.Contains(num4));
			array[2] = num4;
			occupiedList.Add(num4);
			break;
		}
		}
		return array;
	}

	private static int GetKeepDoorMonster(int exitIndex)
	{
		int num = exitIndex % 7;
		int num2 = exitIndex / 7;
		List<int> list = new List<int>();
		if (num + 1 < 7)
		{
			list.Add(num2 * 7 + num + 1);
		}
		if (num - 1 >= 0)
		{
			list.Add(num2 * 7 + num - 1);
		}
		if (num2 + 1 < 5)
		{
			list.Add((num2 + 1) * 7 + num);
		}
		if (num2 - 1 >= 0)
		{
			list.Add((num2 - 1) * 7 + num);
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	private static int GetExitIndex(int startIndex)
	{
		int num = startIndex % 7;
		int num2 = startIndex / 7;
		List<int> list = new List<int>();
		for (int i = 0; i < EntryExitIndex.Count; i++)
		{
			int num3 = EntryExitIndex[i] % 7;
			int num4 = EntryExitIndex[i] / 7;
			if (Mathf.Abs(num3 - num) + Mathf.Abs(num4 - num2) >= 7)
			{
				list.Add(EntryExitIndex[i]);
			}
		}
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	public static RoomInfo GenerateBossRoom()
	{
		int[,] array = new int[7, 5];
		array[0, 2] = 2;
		array[3, 3] = 9;
		int[,] room = array;
		RoomInfo roomInfo = new RoomInfo();
		int[,] obj = new int[7, 5]
		{
			{ 1, 1, 1, 1, 1 },
			{ 1, 1, 2, 1, 1 },
			{ 1, 0, 0, 0, 1 },
			{ 1, 0, 0, 0, 1 },
			{ 1, 0, 0, 0, 1 },
			{ 1, 1, 3, 1, 1 },
			{ 1, 1, 1, 1, 1 }
		};
		obj[2, 1] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		obj[3, 3] = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		roomInfo.seeds = obj;
		roomInfo.room = room;
		return roomInfo;
	}
}
