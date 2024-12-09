using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : SingletonDontDestroy<SceneManager>
{
	private Action<int> SceneLoadedEvent;

	public static bool FirstEnterMenu = true;

	public int SceneIndex { get; private set; }

	public string SceneName { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		if (!(SingletonDontDestroy<SceneManager>.Instance != this))
		{
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		}
	}

	private void Start()
	{
		Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
		SceneIndex = activeScene.buildIndex;
		SceneName = activeScene.name;
		FirstEnterMenu = true;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		SceneLoadedEvent?.Invoke(scene.buildIndex);
		SceneIndex = scene.buildIndex;
		SceneName = scene.name;
	}

	public void LoadScene(int buildIndex, Action<int> loadedEvent)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
		SceneLoadedEvent = loadedEvent;
	}

	public void LoadScene(string sceneName, Action<int> loadedEvent)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		SceneLoadedEvent = loadedEvent;
	}

	public AsyncOperation LoadSceneAsync(int buildIndex, Action<int> loadedEvent)
	{
		SceneLoadedEvent = loadedEvent;
		return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex);
	}

	public AsyncOperation LoadSceneAsync(string sceneName, Action<int> loadedEvent)
	{
		SceneLoadedEvent = loadedEvent;
		return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
	}
}
