using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Game : SingletonDontDestroy<Game>
{
	public const int BuildInScene_Open = 0;

	public const int BuildInScene_Menu = 1;

	public const int BuildInScene_Game = 2;

	public const int steam_AppID = 1520270;

	private Camera currentWorldMainCamera;

	private GameState currentGameState;

	public int RandomSeed;

	public string StartState;

	public string GameVersion;

	private bool initOver;

	[Header("测试参数：")]
	public bool IsTryLoadSaveData;

	public bool isTest;

	public string testGameEventCode;

	public bool isRandomSeed;

	public bool isShowingSwithAnim;

	public bool isNeedAutoSave;

	public bool isStepOverProphesyCard;

	public bool isForcedtoPlayCG;

	public string[] testProphesyCardCodes;

	public bool isCanJudgeUnlockHiddenStage;

	public bool isSatisfiedHiddenStage;

	public bool isOpenHidenRoom;

	public bool isMustShowProphesyBook;

	[Header("游戏初始化参数：")]
	public PlayerOccupation playerOccupation;

	public int presuppositionIndex;

	public List<string> ProphesyCardCodes;

	private Camera CurrentWorldMainCamera
	{
		get
		{
			if (currentWorldMainCamera == null)
			{
				currentWorldMainCamera = Camera.main;
			}
			return currentWorldMainCamera;
		}
	}

	public GameState CurrentGameState => currentGameState;

	public UserData CurrentUserData { get; private set; }

	public AppData AppData { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		if (!(SingletonDontDestroy<Game>.Instance != this))
		{
			Application.runInBackground = true;
		}
	}

	private void Start()
	{
		initOver = false;
		StartCoroutine(Init_IE());
	}

	private void Update()
	{
		if (initOver)
		{
			currentGameState?.OnUpdate();
		}
	}

	private IEnumerator Init_IE()
	{
		SingletonDontDestroy<SettingManager>.Instance.LoadPlayerSetting();
		GameFrameSetting.Instance.SetSettingGameFrame();
		ConfigScreenSetting();
		yield return null;
		SingletonDontDestroy<UIManager>.Instance.ShowView("VersionUI");
		SingletonDontDestroy<UIManager>.Instance.RemoveUIFromShowingList("VersionUI");
		yield return null;
		SingletonDontDestroy<UIManager>.Instance.ShowView("MaskUI");
		yield return null;
		InitializeGame();
		yield return null;
		LoadUserData();
		yield return null;
		StartGame();
	}

	private void SetCurrentUserData(UserData data)
	{
		CurrentUserData = data;
		CurrentUserData.VerifyUserDataIntegrity();
		GuideSystem.Instance.InitGameGuide();
	}

	private void LoadUserData()
	{
		if (GameSave.GetAppData(out var appData))
		{
			AppData = appData;
			UserData userData = GameSave.LoadUserData(AppData.CurrentUserDataIndex);
			if (userData != null)
			{
				SetCurrentUserData(userData);
				return;
			}
			AppData = null;
			GameSave.DeleteAppData();
			GameSave.TryDeleteAllExistUserDatas();
		}
		else
		{
			GameSave.TryDeleteAllExistUserDatas();
		}
	}

	public bool SetNewUserData(int index)
	{
		if (CurrentUserData != null && CurrentUserData.UserDataIndex == index)
		{
			return false;
		}
		AppData.SetNewCurrentUserDataIndex(index);
		SetCurrentUserData(GameSave.LoadUserData(index));
		(SingletonDontDestroy<UIManager>.Instance.GetView("GameMenuUI") as GameMenuUI).SetUserName(CurrentUserData.UserName);
		return true;
	}

	public void CreateNewAppData()
	{
		AppData = new AppData(GameVersion);
		SingletonDontDestroy<UIManager>.Instance.ShowView("CreateNewUserUI", 0, true, new Action<int, string>(OnConfirmCreateNewUserData), string.Empty);
	}

	public void OnConfirmCreateNewUserData(int userIndex, string userName)
	{
		SetCurrentUserData(AppData.AddNewUserData(userIndex, userName));
		GameSave.SaveUserData();
		(SingletonDontDestroy<UIManager>.Instance.GetView("GameMenuUI") as GameMenuUI).SetUserName(CurrentUserData.UserName);
		ShowFirstTimePlayingGame();
	}

	private void ShowFirstTimePlayingGame()
	{
		(SingletonDontDestroy<UIManager>.Instance.ShowView("SystemHintUI") as SystemHintUI).ShowTwoChosenSystemHint("FirstTimePlayGameQuestion".LocalizeText(), "EnterBattleGuide".LocalizeText(), "NoNeed".LocalizeText(), StartBattleGuide);
	}

	private void StartBattleGuide()
	{
		GuideSystem.Instance.StartBattleGuide();
	}

	private void ShowClickReflection(Vector3 pos)
	{
		VfxBase vfxBase = Singleton<VfxManager>.Instance.LoadVfx("ClickReflection");
		Vector3 position = CurrentWorldMainCamera.ScreenToWorldPoint(pos);
		position.z = -5f;
		vfxBase.transform.position = position;
		vfxBase.Play();
	}

	private void InitializeGame()
	{
		LocalizationManager.Instance.InitLocalizationManager("CD_EN.json");
		DataManager.Instance.LoadData();
	}

	private void ConfigScreenSetting()
	{
		SingletonDontDestroy<WindowsSetting>.Instance.SetWindowType(broadcastEvent: false);
	}

	private void StartGame()
	{
		GameState state = Assembly.GetExecutingAssembly().CreateInstance(StartState, ignoreCase: false, BindingFlags.Default, null, null, null, null) as GameState;
		SwitchGameState(state);
	}

	public void SwitchScene(int sceneBuildinIndex)
	{
		StopAllCoroutines();
		switch (sceneBuildinIndex)
		{
		case 1:
			SingletonDontDestroy<SceneManager>.Instance.LoadScene(1, delegate
			{
				SwitchGameState(new GameMenuState());
			});
			break;
		case 2:
			SingletonDontDestroy<SceneManager>.Instance.LoadScene(2, delegate
			{
				SwitchGameState(new GamePlayState());
			});
			break;
		}
	}

	public void SwitchGameState(GameState state)
	{
		if (state != null)
		{
			if (currentGameState != null)
			{
				currentGameState.OnExit();
			}
			currentGameState = state;
			currentGameState.OnEnter();
		}
	}
}
