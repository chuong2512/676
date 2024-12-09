using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class GameSave
{
	public const int MaxUserDataAmount = 3;

	private const string SaveName = "CDGData.dat";

	private const string AppDataPath = "AppData.dat";

	private const string UserDataPathFormat = "UserData_{0}.dat";

	public static bool IsHaveSaveData => File.Exists(Path.Combine(Application.persistentDataPath, "CDGData.dat"));

	public static void DeleteOldSaveData()
	{
		string path = Path.Combine(Application.persistentDataPath, "CDGData.dat");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void DeleteUserDataByIndex(int index)
	{
		string path = Path.Combine(Application.persistentDataPath, $"UserData_{index}.dat");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static bool SaveGame(bool isNeedCheckGameValue = true)
	{
		if (isNeedCheckGameValue && !SingletonDontDestroy<Game>.Instance.isNeedAutoSave)
		{
			return false;
		}
		FileStream fileStream = null;
		try
		{
			fileStream = File.Create(Path.Combine(Application.persistentDataPath, "CDGData.dat"));
			new BinaryFormatter().Serialize(fileStream, new GameSaveInfo(SingletonDontDestroy<Game>.Instance.CurrentUserData.UserDataIndex));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			Debug.LogError("Save game data failed!");
			throw;
		}
		finally
		{
			if (!fileStream.IsNull())
			{
				fileStream.Close();
			}
		}
		Debug.Log("Save game data success !");
		return true;
	}

	public static GameSaveInfo LoadGame()
	{
		FileStream fileStream = null;
		try
		{
			string path = Path.Combine(Application.persistentDataPath, "CDGData.dat");
			if (File.Exists(path))
			{
				fileStream = File.Open(path, FileMode.Open);
				return (GameSaveInfo)new BinaryFormatter().Deserialize(fileStream);
			}
			return null;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		finally
		{
			if (!fileStream.IsNull())
			{
				fileStream.Close();
			}
		}
	}

	public static UserData LoadUserData(int index)
	{
		string path = Path.Combine(Application.persistentDataPath, $"UserData_{index}.dat");
		if (File.Exists(path))
		{
			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(path, FileMode.Open);
				return (UserData)new BinaryFormatter().Deserialize(fileStream);
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				throw;
			}
			finally
			{
				fileStream?.Close();
			}
		}
		return null;
	}

	public static void SaveUserData()
	{
		int userDataIndex = SingletonDontDestroy<Game>.Instance.CurrentUserData.UserDataIndex;
		string path = Path.Combine(Application.persistentDataPath, $"UserData_{userDataIndex}.dat");
		FileStream fileStream = null;
		try
		{
			fileStream = File.Open(path, FileMode.Create);
			new BinaryFormatter().Serialize(fileStream, SingletonDontDestroy<Game>.Instance.CurrentUserData);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public static void SaveUserData(int index, UserData userData)
	{
		string path = Path.Combine(Application.persistentDataPath, $"UserData_{index}.dat");
		FileStream fileStream = null;
		try
		{
			fileStream = File.Open(path, FileMode.Create);
			new BinaryFormatter().Serialize(fileStream, userData);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public static void TryDeleteAllExistUserDatas()
	{
		DeleteUserDataByIndex(0);
		DeleteUserDataByIndex(1);
		DeleteUserDataByIndex(2);
	}

	public static bool GetAppData(out AppData appData)
	{
		string path = Path.Combine(Application.persistentDataPath, "AppData.dat");
		if (File.Exists(path))
		{
			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(path, FileMode.Open);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				appData = (AppData)binaryFormatter.Deserialize(fileStream);
				return appData != null;
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
				throw;
			}
			finally
			{
				fileStream?.Close();
			}
		}
		appData = null;
		return false;
	}

	public static void SaveAppData()
	{
		string path = Path.Combine(Application.persistentDataPath, "AppData.dat");
		FileStream fileStream = null;
		try
		{
			fileStream = File.Open(path, FileMode.Create);
			new BinaryFormatter().Serialize(fileStream, SingletonDontDestroy<Game>.Instance.AppData);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public static void DeleteAppData()
	{
		string path = Path.Combine(Application.persistentDataPath, "AppData.dat");
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}
}
