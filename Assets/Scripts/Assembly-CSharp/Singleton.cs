using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if ((Object)_instance == (Object)null)
			{
				_instance = Object.FindObjectOfType<T>();
				if (Object.FindObjectsOfType<T>().Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopening the scene might fix it.");
					return _instance;
				}
				if ((Object)_instance == (Object)null)
				{
					GameObject obj = new GameObject(typeof(T).ToString());
					_instance = obj.AddComponent<T>();
					obj.name = typeof(T).ToString();
					obj.transform.position = Vector3.zero;
				}
				return _instance;
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if ((Object)_instance != (Object)null && (Object)_instance != (Object)(this as T))
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = Instance;
		}
	}
}
